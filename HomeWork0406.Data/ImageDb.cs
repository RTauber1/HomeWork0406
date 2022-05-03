using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork0406.Data
{
    public class ImageDb
    {
        private string _connectionString;

        public ImageDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Image> GetImages()
        {
            using var context = new ImageLikesContext(_connectionString);
            return context.Images.OrderByDescending(i => i.DateUploaded).ToList();
        }

        public void Add(Image image)
        {
            using var context = new ImageLikesContext(_connectionString);
            context.Images.Add(image);
            context.SaveChanges();
        }

        public Image GetImage(int id)
        {
            using var context = new ImageLikesContext(_connectionString);
            return context.Images.FirstOrDefault(i => i.Id == id);
        }

        public void AddLike(int id)
        {
            using var context = new ImageLikesContext(_connectionString);
            Image image = context.Images.FirstOrDefault(i => i.Id == id);
            image.Likes++;
            context.SaveChanges();
        }

        public int GetLikes(int id)
        {
            using var context = new ImageLikesContext(_connectionString);
            Image image = context.Images.FirstOrDefault(i => i.Id == id);
            return image != null ? image.Likes : 0;
        }
    }

}
