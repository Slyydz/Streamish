using Streamish.Models;
using Streamish.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streamish.Tests.Mocks
{
    class InMemoryUserRepository : IUserProfileRepository
    {
        private readonly List<UserProfile> _data;

        public List<UserProfile> InternalData
        {
            get
            {
                return _data;
            }
        }

        public InMemoryUserRepository(List<UserProfile> startData)
        {
            _data = startData;
        }

        public void Add(UserProfile userProfile)
        {
            var lastVideo = _data.Last();
            userProfile.Id = lastVideo.Id + 1;
            _data.Add(userProfile);
        }

        public void DeleteUser(int id)
        {
            var deleteVideo = _data.FirstOrDefault(up => up.Id == id);
            if(deleteVideo == null)
            {
                return;
            }
            _data.Remove(deleteVideo);
        }

        public List<UserProfile> Get()
        {
            return _data;
        }

        public UserProfile GetByFirebaseIdWithVideo(string fireId)
        {
            return _data.FirstOrDefault(up => up.FirebaseUserId == fireId);
        }

        public UserProfile GetByIdWithVideo(int id)
        {
            return _data.FirstOrDefault(up => up.Id == id);
        }
    }
}
