using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Streamish.Models;
using Streamish.Repositories;
using Streamish.Controllers;
using Streamish.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace Streamish.Tests
{
    public class UserProfileControllerTest
    {
        [Fact]
        public void Get_By_Id_With_Videos()
        {
            var testData = CreateTestProfiles(20);
            var testId = testData[0].Id;
            
            var repo = new InMemoryUserRepository(testData);
            var controller = new UserProfileController(repo);

            var result = controller.GetByIdWithVideos(testId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualUser = Assert.IsType<UserProfile>(okResult.Value);

            Assert.Equal(testId, actualUser.Id);
        }

        private List<UserProfile> CreateTestProfiles(int count)
        {
            var users = new List<UserProfile>();
            for (var i = 1; i <= count; i++)
            {
                users.Add(CreateTestUserProfile(i));
            }
            return users;
        }

        private UserProfile CreateTestUserProfile(int id)
        {
            return new UserProfile()
            {
                Id = id,
                Name = $"User {id}",
                Email = $"user{id}@example.com",
                DateCreated = DateTime.Today.AddDays(-id),
                ImageUrl = $"http://user.url/{id}",
            };
        }
    }
}
