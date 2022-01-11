using Streamish.Models;
using System.Collections.Generic;

namespace Streamish.Repositories
{
    public interface IVideoRepository
    {
        void Add(Video video);
        void Delete(int id);
        List<Video> GetAll();
        List<Video> GetAllWithComments();
        Video GetById(int id);
        Video GetVideoByIdWithComments(int id);
        List<Video> Search(string criterion, bool sortDescending);
        List<Video> SearchByDate(string searchDate);
        void Update(Video video);
    }
}