﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Streamish.Models;
using Streamish.Utils;

namespace Streamish.Repositories
{

    public class VideoRepository : BaseRepository, IVideoRepository
    {
        public VideoRepository(IConfiguration configuration) : base(configuration) { }

        public List<Video> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
               SELECT v.Id, v.Title, v.Description, v.Url, v.DateCreated 'VideoDateCreated', v.UserProfileId,

                      up.Name, up.Email, up.DateCreated AS UserProfileDateCreated,
                      up.ImageUrl AS UserProfileImageUrl
                        
                 FROM Video v 
                      JOIN UserProfile up ON v.UserProfileId = up.Id
             ORDER BY VideoDateCreated";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        var videos = new List<Video>();
                        while (reader.Read())
                        {
                            videos.Add(newVideo(reader));
                        }

                        return videos;
                    }
                }
            }
        }

        public List<Video> GetAllWithComments()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT v.Id, v.Title, v.Description, v.Url, 
                       v.DateCreated AS VideoDateCreated, v.UserProfileId,

                       up.Name, up.Email, up.DateCreated AS UserProfileDateCreated,
                       up.ImageUrl AS UserProfileImageUrl,
                        
                       c.Id AS CommentId, c.Message, c.UserProfileId AS CommentUserProfileId
                  FROM Video v 
                       JOIN UserProfile up ON v.UserProfileId = up.Id
                       LEFT JOIN Comment c on c.VideoId = v.id
             ORDER BY  v.DateCreated
            ";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        var videos = new List<Video>();
                        while (reader.Read())
                        {
                            var videoId = DbUtils.GetInt(reader, "Id");

                            var existingVideo = videos.FirstOrDefault(p => p.Id == videoId);
                            if (existingVideo == null)
                            {
                                existingVideo = newVideo(reader);

                                existingVideo.Comments = new List<Comment>();

                                videos.Add(existingVideo);

                            }

                            if (DbUtils.IsNotDbNull(reader, "CommentId"))
                            {
                                existingVideo.Comments.Add(newComment(reader, videoId));
                            }
                        }

                        return videos;
                    }
                }
            }
        }


        public Video GetById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                          SELECT v.Id, v.Title, v.Description, v.Url, v.DateCreated, v.UserProfileId,

                      up.Name, up.Email, up.DateCreated AS UserProfileDateCreated,
                      up.ImageUrl AS UserProfileImageUrl
                        
                 FROM Video v 
                      JOIN UserProfile up ON v.UserProfileId = up.Id
                           WHERE v.Id = @Id";

                    DbUtils.AddParameter(cmd, "@Id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        Video video = null;
                        if (reader.Read())
                        {
                            video = newVideo(reader);
                        }

                        return video;
                    }
                }
            }
        }

        public Video GetVideoByIdWithComments(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT v.Id, v.Title, v.Description, v.Url, 
                       v.DateCreated AS VideoDateCreated, v.UserProfileId,

                       up.Name, up.Email, up.DateCreated AS UserProfileDateCreated,
                       up.ImageUrl AS UserProfileImageUrl,
                        
                       c.Id AS CommentId, c.Message, c.UserProfileId AS CommentUserProfileId
                  FROM Video v 
                       JOIN UserProfile up ON v.UserProfileId = up.Id
                       LEFT JOIN Comment c on c.VideoId = v.Id
                       WHERE v.Id = @id";

                    DbUtils.AddParameter(cmd, "@Id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        var videos = new List<Video>();
                        var video = new Video();

                        while (reader.Read())
                        {
                            var videoId = DbUtils.GetInt(reader, "Id");

                            var existingVideo = videos.FirstOrDefault(v => v.Id == videoId);

                            if(existingVideo == null)
                            {
                                video = newVideo(reader);
                                videos.Add(video);
                                video.Comments = new List<Comment>();
                            }

                            if (DbUtils.IsNotDbNull(reader, "CommentId"))
                            {

                                    video.Comments.Add(newComment(reader, videoId));
                                
                                
                            }
                        }

                        return video;
                    }
                }
            }
        }

        public List<Video> Search(string criterion, bool sortDescending)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    var sql = @"
              SELECT v.Id, v.Title, v.Description, v.Url, v.DateCreated AS VideoDateCreated, v.UserProfileId,

                     up.Name, up.Email, up.DateCreated AS UserProfileDateCreated,
                     up.ImageUrl AS UserProfileImageUrl
                        
                FROM Video v 
                     JOIN UserProfile up ON v.UserProfileId = up.Id
               WHERE v.Title LIKE @Criterion OR v.Description LIKE @Criterion";

                    if (sortDescending)
                    {
                        sql += " ORDER BY v.DateCreated DESC";
                    }
                    else
                    {
                        sql += " ORDER BY v.DateCreated";
                    }

                    cmd.CommandText = sql;
                    DbUtils.AddParameter(cmd, "@Criterion", $"%{criterion}%");
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        var videos = new List<Video>();
                        while (reader.Read())
                        {
                            videos.Add(newVideo(reader));
                        }

                        return videos;
                    }
                }
            }
        }

        public List<Video> SearchByDate(string searchDate)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    var sql = @"
              SELECT v.Id, v.Title, v.Description, v.Url, v.DateCreated AS VideoDateCreated, v.UserProfileId,

                     up.Name, up.Email, up.DateCreated AS UserProfileDateCreated,
                     up.ImageUrl AS UserProfileImageUrl
                        
                FROM Video v 
                     JOIN UserProfile up ON v.UserProfileId = up.Id
               WHERE v.DateCreated >= @searchDate";

                    cmd.CommandText = sql;
                    DbUtils.AddParameter(cmd, "@searchDate", searchDate);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        var videos = new List<Video>();
                        while (reader.Read())
                        {
                            videos.Add(newVideo(reader));
                        }

                        return videos;
                    }
                }
            }
        }

        public void Add(Video video)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Video (Title, Description, DateCreated, Url, UserProfileId)
                        OUTPUT INSERTED.ID
                        VALUES (@Title, @Description, @DateCreated, @Url, @UserProfileId)";

                    DbUtils.AddParameter(cmd, "@Title", video.Title);
                    DbUtils.AddParameter(cmd, "@Description", video.Description);
                    DbUtils.AddParameter(cmd, "@DateCreated", video.DateCreated);
                    DbUtils.AddParameter(cmd, "@Url", video.Url);
                    DbUtils.AddParameter(cmd, "@UserProfileId", video.UserProfileId);

                    video.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Video video)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Video
                           SET Title = @Title,
                               Description = @Description,
                               DateCreated = @DateCreated,
                               Url = @Url,
                               UserProfileId = @UserProfileId
                         WHERE Id = @Id";

                    DbUtils.AddParameter(cmd, "@Title", video.Title);
                    DbUtils.AddParameter(cmd, "@Description", video.Description);
                    DbUtils.AddParameter(cmd, "@DateCreated", video.DateCreated);
                    DbUtils.AddParameter(cmd, "@Url", video.Url);
                    DbUtils.AddParameter(cmd, "@UserProfileId", video.UserProfileId);
                    DbUtils.AddParameter(cmd, "@Id", video.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Video WHERE Id = @Id";
                    DbUtils.AddParameter(cmd, "@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private Video newVideo(SqlDataReader reader)
        {
            var video = new Video()
            {
                Id = DbUtils.GetInt(reader, "Id"),
                Title = DbUtils.GetString(reader, "Title"),
                Description = DbUtils.GetString(reader, "Description"),
                DateCreated = DbUtils.GetDateTime(reader, "VideoDateCreated"),
                Url = DbUtils.GetString(reader, "Url"),
                UserProfileId = DbUtils.GetInt(reader, "UserProfileId"),
                UserProfile = new UserProfile()
                {
                    Id = DbUtils.GetInt(reader, "UserProfileId"),
                    Name = DbUtils.GetString(reader, "Name"),
                    Email = DbUtils.GetString(reader, "Email"),
                    DateCreated = DbUtils.GetDateTime(reader, "UserProfileDateCreated"),
                    ImageUrl = DbUtils.GetString(reader, "UserProfileImageUrl"),
                },
            };

            return video;
        }

        private Comment newComment(SqlDataReader reader, int videoId)
        {
            var comment = new Comment()
            {
                Id = DbUtils.GetInt(reader, "CommentId"),
                Message = DbUtils.GetString(reader, "Message"),
                VideoId = videoId,
                UserProfileId = DbUtils.GetInt(reader, "CommentUserProfileId"),
                UserProfile = new UserProfile(){
                    Id = DbUtils.GetInt(reader, "CommentUserProfileId"),
                    Name = DbUtils.GetString(reader, "Name"),
                    Email = DbUtils.GetString(reader, "Email"),
                    DateCreated = DbUtils.GetDateTime(reader, "UserProfileDateCreated"),
                    ImageUrl = DbUtils.GetString(reader, "UserProfileImageUrl"),
                }
            };

            return comment;
        }
    }
}
