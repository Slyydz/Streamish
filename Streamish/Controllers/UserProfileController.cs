using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Streamish.Models;
using Streamish.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Streamish.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileRepository _upRepo;
        public UserProfileController(IUserProfileRepository userProfileRepository)
        {
            _upRepo = userProfileRepository;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            List<UserProfile> up = _upRepo.Get();
            if (up == null)
            {
                return NotFound();
            }
            return Ok(up);
        }

        [Authorize]
        [HttpGet("GetByIdWithVideos")]
        public IActionResult GetByIdWithVideos(int id)
        {
            UserProfile up = _upRepo.GetByIdWithVideo(id);
            if(up == null)
            {
                return NotFound();
            }
            return Ok(up);
        }

        [HttpGet("DoesUserExist/{firebaseUserId}")]
        public IActionResult DoesUserExist(string firebaseUserId)
        {
            var userProfile = _upRepo.GetByFirebaseIdWithVideo(firebaseUserId);
            if (userProfile == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [Authorize]
        [HttpGet("GetByFirebaseUserId")]
        public IActionResult GetByFireBaseUserId(string firebaseId)
        {
            UserProfile up = _upRepo.GetByFirebaseIdWithVideo(firebaseId);
            if(up == null)
            {
                return NotFound();
            }
            return Ok(up);
        }

        [HttpPost]
        public IActionResult Register(UserProfile userProfile)
        { 
            _upRepo.Add(userProfile);
            return CreatedAtAction(
                nameof(GetByFireBaseUserId), new { firebaseUserId = userProfile.FirebaseUserId }, userProfile);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _upRepo.DeleteUser(id);

            return NoContent();
        }
    }
}
