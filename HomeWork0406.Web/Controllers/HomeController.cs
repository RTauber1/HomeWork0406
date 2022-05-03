using HomeWork0406.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using HomeWork0406.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using Newtonsoft.Json;

namespace HomeWork0406.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _connectionString;

        public HomeController(IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            _environment = environment;
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        public IActionResult Index()
        {
            ImageDb imageDb = new ImageDb(_connectionString);
            return View(imageDb.GetImages());
        }
        public IActionResult Uplode()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Uplode(String title, IFormFile tfileName)
        {
            string thefileName = $"{Guid.NewGuid()}{Path.GetExtension(tfileName.FileName)}";
            string fullPath = Path.Combine(_environment.WebRootPath, "uploads", thefileName);
            using var stream = new FileStream(fullPath, FileMode.CreateNew);
            tfileName.CopyTo(stream);

            Image image = new Image();
            image.Title = title;
            image.FileName = thefileName;
            image.DateUploaded = DateTime.Now;
            ImageDb imageDb = new ImageDb(_connectionString);
            imageDb.Add(image);
            return Redirect("/");
        }
        public IActionResult viewimage(int id)
        {
            ImageDb imageDb = new ImageDb(_connectionString);
            VeiwImage veiwImage = new VeiwImage();
            veiwImage.Image = imageDb.GetImage(id);
            bool toSee = HttpContext.Session.Get<List<int>>("AlreadyLiked") == null;
            if (HttpContext.Session.GetString("AlreadyLiked") != null)
            {
                veiwImage.CouldLikeImage = !(HttpContext.Session.Get<List<int>>("AlreadyLiked")).Any(i => i == id);
            }
            else
            {
                veiwImage.CouldLikeImage = true;
            }
            return View(veiwImage);
        }
        public void IncreaseLikes(int id)
        {
            ImageDb imageDb = new ImageDb(_connectionString);
            imageDb.AddLike(id);
            List<int> ImagesLike = new List<int>();

            if (HttpContext.Session.Get<List<int>>("AlreadyLiked") != null)
            {
                ImagesLike = HttpContext.Session.Get<List<int>>("AlreadyLiked");
            }

            ImagesLike.Add(id);

            HttpContext.Session.Set("AlreadyLiked", ImagesLike);
        }
        public  ActionResult GetLikes(int id)
        {
            ImageDb imageDb = new ImageDb(_connectionString);
            return Json(new { imageLikeNum = imageDb.GetLikes(id)});
        }
        
    }
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }
}
