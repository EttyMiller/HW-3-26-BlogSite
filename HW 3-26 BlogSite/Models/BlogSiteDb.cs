using Microsoft.Data.SqlClient;
using System.Reflection.Metadata;

namespace HW_3_26_BlogSite.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public DateTime DatePosted { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class BlogComment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime DateCommented { get; set; }
        public int PostId { get; set; }
    }

    public class BlogSiteDb
    {
        private readonly string _connectionString;
        public BlogSiteDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Blog> GetBlogs(int page)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = $@"SELECT * FROM Posts
            ORDER BY DatePosted desc
            OFFSET {(page - 1) * 3} ROWS
            FETCH NEXT 3 ROWS ONLY";

            connection.Open();

            return PostReader(cmd.ExecuteReader());
        }

        public int GetTotalPages()
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT Count(*) FROM Posts";

            connection.Open();
            var totalPosts = (int)cmd.ExecuteScalar();
            var pages = (totalPosts + 2) / 3;

            return pages;
        }

        public int AddBlog(Blog blog)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Posts
            VALUES (@datePosted, @title, @post);
            SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@datePosted", blog.DatePosted);
            cmd.Parameters.AddWithValue("@title", blog.Title);
            cmd.Parameters.AddWithValue("@post", blog.Content);
            connection.Open();

            return (int)(decimal)cmd.ExecuteScalar();
        }

        public Blog GetBlogById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Posts WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();

            var posts = PostReader(cmd.ExecuteReader());
            return posts[0];
        }

        public List<Blog> PostReader(SqlDataReader reader)
        {
            List<Blog> posts = new();
            while (reader.Read())
            {
                posts.Add(new Blog
                {
                    Id = (int)reader["Id"],
                    DatePosted = (DateTime)reader["DatePosted"],
                    Title = (string)reader["Title"],
                    Content = (string)reader["Post"]

                });
            }
            return posts;
        }

        public void AddComment(BlogComment comment)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Comments
            VALUES (@Name, @content, @dateCommented, @postId)";
            cmd.Parameters.AddWithValue("@name", comment.Name);
            cmd.Parameters.AddWithValue("@content", comment.Content);
            cmd.Parameters.AddWithValue("@postId", comment.PostId);
            cmd.Parameters.AddWithValue("@dateCommented", comment.DateCommented);
            connection.Open();

            cmd.ExecuteNonQuery();
        }

        public List<BlogComment> GetCommentsForBlog(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Comments
            WHERE PostId = @id";
            cmd.Parameters.AddWithValue("@id", id);

            connection.Open();

            return CommentReader(cmd.ExecuteReader());
        }

        public List<BlogComment> CommentReader(SqlDataReader reader)
        {
            List<BlogComment> comments = new();
            while (reader.Read())
            {
                comments.Add(new BlogComment
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Content = (string)reader["Content"],
                    DateCommented = (DateTime)reader["DateCommented"],
                    PostId = (int)reader["PostId"]
                });
            }
            return comments;
        }

        public int MostRecent()
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT IDENT_CURRENT('Posts')";

            connection.Open();

            return (int)(decimal)cmd.ExecuteScalar(); ;

        }
    }
}
