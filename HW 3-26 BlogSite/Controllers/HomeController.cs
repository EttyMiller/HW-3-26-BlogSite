using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HW_3_26_BlogSite.Models;
using Microsoft.Data.SqlClient;

namespace HW_3_26_BlogSite.Controllers;

public class HomeController : Controller
{
    private readonly string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=BlogSite;Integrated Security=True;TrustServerCertificate=true;";

    public IActionResult Index(int page = 1)
    {
        var db = new BlogSiteDb(_connectionString);
        return View(new IndexViewModel()
        {
            Blogs = db.GetBlogs(page),
            Page = page,
            TotalPages = db.GetTotalPages()
        });
    }


    public IActionResult ViewBlog(int id)
    {
        var db = new BlogSiteDb(_connectionString);

        var vm = new ViewBlogViewModel()
        {
            Blog = db.GetBlogById(id),
            BlogComments = db.GetCommentsForBlog(id)
        };

        var lastCommenter = Request.Cookies["commenter"];
        if (lastCommenter != null)
        {
            vm.LastCommenter = lastCommenter;
        }

        return View(vm);
    }

    [HttpPost]
    public IActionResult AddComment(BlogComment comment)
    {
        var db = new BlogSiteDb(_connectionString);
        db.AddComment(comment);

        Response.Cookies.Append("commenter", comment.Name, new CookieOptions
        {
            Expires = new DateTimeOffset(DateTime.Today.AddDays(7))
        });
        return Redirect($"/home/viewblog?id={comment.PostId}");
    }

    public IActionResult MostRecent()
    {
        var db = new BlogSiteDb(_connectionString);
        var mostRecent = db.MostRecent();
        return Redirect($"/home/ViewBlog?id={mostRecent}");
    }
}
