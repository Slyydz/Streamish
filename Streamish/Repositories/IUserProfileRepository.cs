using Streamish.Models;
using System.Collections.Generic;

namespace Streamish.Repositories
{
    public interface IUserProfileRepository
    {
        void Add(UserProfile userProfile);
        void DeleteUser(int id);
        List<UserProfile> Get();
        UserProfile GetByFirebaseIdWithVideo(string fireId);
        UserProfile GetByIdWithVideo(int id);
    }
}