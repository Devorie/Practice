using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace HomeWork_02._21.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Commenter { get; set; }
        public string Content { get; set; }
        public DateTime DateTime { get; set; }
    }
    public class Manager
    {
        private readonly string _connectionString;

        public Manager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Post> GetPosts()
        {
            var connection = new SqlConnection(_connectionString);
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM Posts";
            connection.Open();
            var posts = new List<Post>();
            var reader = command.ExecuteReader();
            while(reader.Read())
            {
                posts.Add(new()
                {
                    Id = (int)reader["Id"],
                    Title = (string)reader["Title"],
                    Content = (string)reader["Content"],
                    DateTime = (DateTime)reader["DateTime"]
                });
            };

             return posts;
        }

        public List<Post> GetPosts200()
        {
            var posts = GetPosts();
            foreach(Post p in posts)
            {
                if (p.Content.Length > 150)
                {
                    p.Content = p.Content.Substring(1, 150);
                }
            }

            return posts;
        }

        public Post GetPost(int postId)
        {
            var connection = new SqlConnection(_connectionString);
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM Posts
                                    WHERE Id = @postId";
            command.Parameters.AddWithValue("@postId", postId);
            connection.Open();
            var reader = command.ExecuteReader();
            if(!reader.Read())
            {
                return null;
            }
            return (new()
            {
                Id = (int)reader["Id"],
                Title = (string)reader["Title"],
                Content = (string)reader["Content"],
                DateTime = (DateTime)reader["DateTime"]
            });
        }

        public List<Comment> GetComments(int postId)
        {
            var connection = new SqlConnection(_connectionString);
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT c.Id,c.Commenter,c.Content,c.DateTime FROM Comments c
                                    JOIN Posts p
                                    ON p.Id = c.PostId
                                    WHERE c.PostId = @postId";
            command.Parameters.AddWithValue("@postId", postId);
            connection.Open();
            var comments = new List<Comment>();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                comments.Add(new()
                {
                    Id = (int)reader["Id"],
                    PostId = (int)reader["Id"],
                    Commenter = (string)reader["Title"],
                    Content = (string)reader["Content"],
                    DateTime = (DateTime)reader["DateTime"]
                });
            };

            return comments;
        }


        public decimal AddPost(string content, string title)
        {
            var connection = new SqlConnection(_connectionString);
            var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO Posts
                                    VALUES (@content, @title, GETDATE())
                                    SELECT SCOPE_IDENTITY() AS 'Id'";
            command.Parameters.AddWithValue("@content", content);
            command.Parameters.AddWithValue("@title", title);
            connection.Open();
            var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return 0;
            };

            return (decimal)reader["Id"];
        }

        public void AddComment(int postId, string commenter, string content)
        {
            var connection = new SqlConnection(_connectionString);
            var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO comments
                                    VALUES (@postId, @Commenter, @Content, GETDATE())";
            command.Parameters.AddWithValue("@postId", postId);
            command.Parameters.AddWithValue("@Commenter", commenter);
            command.Parameters.AddWithValue("@Content", content);
            
            connection.Open();
            command.ExecuteNonQuery();
        }

        public int GetOldest()
        {
            var connection = new SqlConnection(_connectionString);
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT Id FROM Posts
                                    ORDER BY DateTime Desc";
            connection.Open();
            var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return 0;
            };

            return (int)reader["Id"];
        }
    }

    public class BlogViewModel
    {
        public List<Post> Posts { get; set; }
        public string Title { get; set; }
    }

    public class PostViewModel
    {
        public Post Post { get; set; }
        public List<Comment> Comments { get; set; }
        public string CommenterName { get; set; }
    }
}
