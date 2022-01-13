using Streamish.Models;

namespace Streamish.Repositories
{
    public interface IUserProfileRepository
    {
        void Add(UserProfile userProfile);
        UserProfile GetByFirebaseIdWithVideo(string fireId);
        UserProfile GetByIdWithVideo(int id);
    }
}