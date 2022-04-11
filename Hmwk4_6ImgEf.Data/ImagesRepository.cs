using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmwk4_6ImgEf.Data
{
   public class ImagesRepository
    {
        private string _connectionstring;
        public ImagesRepository(string connect)
        {
            _connectionstring = connect;
        }

        public List<Image> GetAll()
        {
            using var context = new ImageDataContext(_connectionstring);
            return context.Images.ToList();
        }

        public void Add(Image image)
        {
            using var context = new ImageDataContext(_connectionstring);
            context.Images.Add(image);
            context.SaveChanges();
        }

        public Image GetById(int id)
        {
            using var context = new ImageDataContext(_connectionstring);
            return context.Images.FirstOrDefault(i => i.Id == id);
        }
        public void Update(int id)
        {
            using var context = new ImageDataContext(_connectionstring);
         Image update=    context.Images.FirstOrDefault(i => i.Id == id);
            update.Likes++;
            context.SaveChanges();
        }
        public int Likes(int id)
        {
            using var context = new ImageDataContext(_connectionstring);
            Image update = context.Images.FirstOrDefault(i => i.Id == id);
            if (update == null)
            {
                return 0;
            }
            return update.Likes;
            
        }
    }

}
