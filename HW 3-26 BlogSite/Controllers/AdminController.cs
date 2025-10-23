using HW_3_26_BlogSite.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace HW_3_26_BlogSite.Controllers
{
    public class AdminController : Controller
    {
        private readonly string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=BlogSite;Integrated Security=True;TrustServerCertificate=true;";

        public IActionResult Admin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitPost(Blog blog)
        {
            var db = new BlogSiteDb(_connectionString);
            int recentId = db.AddBlog(blog);
            return Redirect($"/home/viewblog?id={recentId}");
        }
    }
}
