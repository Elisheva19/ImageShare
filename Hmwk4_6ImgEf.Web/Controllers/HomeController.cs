using Hmwk4_6ImgEf.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hmwk4_6ImgEf.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Hmwk4_6ImgEf.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public HomeController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;

            _configuration = configuration;
        }
        public IActionResult Index()
        {
            var connectionstring = _configuration.GetConnectionString("ConStr");
            var repo = new ImagesRepository(connectionstring);
          
            return View(new IndexViewModel
            {
                Images = repo.GetAll(),
                
                

            }) ;
        }

        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Upload(IFormFile imageFile, string title)
        {
            string fileName = $"{Guid.NewGuid()}-{imageFile.FileName}";

            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);
            using var fs = new FileStream(filePath, FileMode.CreateNew);
            imageFile.CopyTo(fs);


            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new ImagesRepository(connectionString);
            Image pic = new Image
            {
                Title = title,
                ImagePath = fileName,
                Date = DateTime.Now,
                Likes = 0
            };
            repo.Add(pic);
            return Redirect("/home/index");

        }
        public IActionResult ViewImage(int id)
        {

            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new ImagesRepository(connectionString);
            List<int> Ids = HttpContext.Session.Get<List<int>>("likesids");
            if (Ids == null)
            {
                Ids = new List<int>();
            }
            Image view=   repo.GetById(id);

            return View(new ViewImageViewModel
            {
                ViewImage = view,
                Liked = Ids.Contains(id)

            }) ;
        }

        public void Update(int id)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new ImagesRepository(connectionString);
            repo.Update(id);
            List<int> Ids = HttpContext.Session.Get<List<int>>("likesids");
            if (Ids == null)
            {
                Ids = new List<int>();
            }

          
            Ids.Add(id);
            HttpContext.Session.Set("likesids", Ids);

        }
        public IActionResult GetLikes(int id)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new ImagesRepository(connectionString);
        
            return Json(repo.Likes(id));
        }
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