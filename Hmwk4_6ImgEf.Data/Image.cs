using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmwk4_6ImgEf.Data
{
    public class Image
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public int Likes { get; set; }
       
    }
}
