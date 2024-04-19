using HomeWork_02._21.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Collections;
using System.Diagnostics;

namespace HomeWork_02._21.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=BlogPost; Integrated Security=true;";

        public IActionResult Index()
        {
            Manager mgr = new(_connectionString);
            return View(new BlogViewModel
            {
                Posts = mgr.GetPosts200()
            });
        }

        public IActionResult MostRecent()
        {
            Manager mgr = new(_connectionString);
            int id = mgr.GetOldest();
            return Redirect($"/Home/ViewBlogPost?id={id}");
        }


        public IActionResult Admin()
        {
            return View();
        }

        public IActionResult ViewBlogPost(int id)
        {
            Manager mgr = new(_connectionString);
            var vm = new PostViewModel
            {
                Post = mgr.GetPost(id),
                Comments = mgr.GetComments(id)
            };
            vm.CommenterName = Request.Cookies["commenter-name"];
            return View(vm);
        }

        [HttpPost]
        public IActionResult AddPost(string content, string title)
        {
            Manager mgr = new(_connectionString);
            decimal id = mgr.AddPost(content, title);
            return Redirect($"/Home/ViewBlogPost?id={id}");
        }

        [HttpPost]
        public IActionResult AddComment(int postId, string commenter, string content)
        {
            Manager mgr = new(_connectionString);
            mgr.AddComment(postId, commenter, content);
            Response.Cookies.Append("commenter-name", commenter);
            return Redirect("/Home/Index");
        }
    }
}
