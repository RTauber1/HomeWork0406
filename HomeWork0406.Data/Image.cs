using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;


namespace HomeWork0406.Data
{
    public class Image
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FileName { get; set; }
        public DateTime DateUploaded { get; set; }
        public int Likes { get; set; }
    }
    public class VeiwImage
    {
        public Image Image { get; set; }
        public bool CouldLikeImage { get; set; }
    }
}
