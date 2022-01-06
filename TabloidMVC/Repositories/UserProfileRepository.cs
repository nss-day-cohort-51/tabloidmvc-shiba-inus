using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using TabloidMVC.Utils;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;

namespace TabloidMVC.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration config) : base(config) { }

        public List<UserProfile> GetAllUsers()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT u.Id as userId, u.FirstName as firstName, u.LastName as lastName, u.Email as email, u.DisplayName as displayName, t.Id as userTypeId, t.Name as userTypeName
FROM UserProfile as u
JOIN UserType t ON t.Id = u.UserTypeId
ORDER BY DisplayName ASC";

                    var reader = cmd.ExecuteReader();

                    var users = new List<UserProfile>();

                    while (reader.Read())
                    {
                        users.Add(new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("userId")),
                            FirstName = reader.GetString(reader.GetOrdinal("firstName")),
                            LastName = reader.GetString(reader.GetOrdinal("lastName")),
                            Email = reader.GetString(reader.GetOrdinal("email")),
                            DisplayName = reader.GetString(reader.GetOrdinal("displayName")),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("userTypeId")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("userTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("userTypeName"))
                            }
                        });
                    }
                    reader.Close();

                    return users;
                }
            }
        }


        public UserProfile GetByEmail(string email)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                              u.CreateDateTime, u.ImageLocation, u.UserTypeId,
                              ut.[Name] AS UserTypeName
                         FROM UserProfile u
                              LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                        WHERE email = @email";
                    cmd.Parameters.AddWithValue("@email", email);

                    UserProfile userProfile = null;
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                            },
                        };
                    }

                    reader.Close();

                    return userProfile;
                }
            }
        }

        public UserProfile GetById(int id)
        {
            using (var conn = Connection)
            {

                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                              u.CreateDateTime, u.ImageLocation, u.UserTypeId,
                              ut.[Name] AS UserTypeName
                         FROM UserProfile u
                              LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                        WHERE u.id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    UserProfile userProfile = null;
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ImageLocation = reader.IsDBNull(reader.GetOrdinal("ImageLocation")) ? null : reader.GetString(reader.GetOrdinal("ImageLocation")),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                            }
                        };
                    }

                    reader.Close();

                    return userProfile;
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
                    cmd.CommandText = @"DELETE FROM UserProfile WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateUserType(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE UserProfile 
                                           SET UserTypeId = @userTypeId
                                         WHERE id = @id";

                    cmd.Parameters.AddWithValue("@userTypeId", userProfile.UserTypeId);
                    cmd.Parameters.AddWithValue("@id", userProfile.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Register(UserProfile userProfile)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO UserProfile (FirstName, LastName, DisplayName, Email, CreateDateTime, UserTypeId)
                                                    OUTPUT INSERTED.ID VALUES (@FirstName, @LastName, @DisplayName, @Email, GETDATE(), @UserTypeId)";
                    cmd.Parameters.AddWithValue("@FirstName", userProfile.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", userProfile.LastName);
                    cmd.Parameters.AddWithValue("@DisplayName", userProfile.DisplayName);
                    cmd.Parameters.AddWithValue("@Email", userProfile.Email);
                    cmd.Parameters.AddWithValue("@UserTypeId", userProfile.UserTypeId);

                    userProfile.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Subscribe(int subscriberId, int providerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Subscription (SubscriberUserProfileId, ProviderUserProfileId, BeginDateTime)
                                        VALUES (@SubscriberId, @ProviderId, GETDATE())";
                    cmd.Parameters.AddWithValue("@SubscriberId", subscriberId);
                    cmd.Parameters.AddWithValue("@ProviderId", providerId);

                    cmd.ExecuteNonQuery();
                }
            }

        }
        public void Unsubscribe(int subscriberId, int providerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Subscription WHERE SubscriberUserProfileId = @SubscriberId AND ProviderUserProfileId = @ProviderId";
                    cmd.Parameters.AddWithValue("@SubscriberId", subscriberId);
                    cmd.Parameters.AddWithValue("@ProviderId", providerId);

                    cmd.ExecuteNonQuery();
                }
            }

        }

        public UserProfile GetUserProfileById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(UserProfile profile)
        {
            throw new NotImplementedException();
        }

        public void CreateUserProfile(UserProfile userProfile)
        {
            throw new NotImplementedException();
        }

        public List<UserProfile> GetAllAdmins()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT u.Id as userId, u.FirstName as firstName, u.LastName as lastName, u.Email as email, u.DisplayName as displayName, t.Id as userTypeId, t.Name as userTypeName
                                        FROM UserProfile as u
                                        JOIN UserType t ON t.Id = u.UserTypeId
                                        WHERE userTypeName = 'Admin'";

                    var reader = cmd.ExecuteReader();

                    var admins = new List<UserProfile>();

                    while (reader.Read())
                    {
                        admins.Add(new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("userId")),
                            FirstName = reader.GetString(reader.GetOrdinal("firstName")),
                            LastName = reader.GetString(reader.GetOrdinal("lastName")),
                            Email = reader.GetString(reader.GetOrdinal("email")),
                            DisplayName = reader.GetString(reader.GetOrdinal("displayName")),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("userTypeId")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("userTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("userTypeName"))
                            }
                        });
                    }
                    reader.Close();

                    return admins;
                }
            }
        }

    }
}
