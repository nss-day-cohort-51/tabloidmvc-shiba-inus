using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;
using System.Data;
using System.Reflection.PortableExecutable;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class PostTagRepository : BaseRepository, IPostTagRepository
    {
        public PostTagRepository(IConfiguration config) : base(config) { }

        public PostTag GetById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT PostId, TagID
                         FROM PostTag
                        WHERE Id = @id;";

                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    PostTag postTag = new PostTag();

                    if (reader.Read())
                    {
                        postTag.Id = id;
                        postTag.PostId = reader.GetInt32(reader.GetOrdinal("PostId"));
                        postTag.TagId = reader.GetInt32(reader.GetOrdinal("TagId"));
                    }

                    reader.Close();

                    return postTag;
                }
            }
        }

        
        public List<PostTag> GetPostTagsByPostId(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT pt.Id, pt.PostId, pt.TagId, t.Name 
                         FROM PostTag pt
                              LEFT JOIN Tag t ON t.Id = pt.TagId
                              LEFT JOIN Post p ON p.id= pt.PostId
                        WHERE p.id = @id";

                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    List<PostTag> postTags = new List<PostTag> { }; 

                    while (reader.Read())
                    {
                        PostTag postTag = new PostTag()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            TagId = reader.GetInt32(reader.GetOrdinal("TagId")),
                            Tag = new Tag()
                            {
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                            }
                        };

                        postTags.Add(postTag);
                    }

                    reader.Close();

                    return postTags;
                }
            }
        }


        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM PostTag WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    
    }
}
