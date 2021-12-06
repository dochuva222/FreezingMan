using FreezingMan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FreezingMan.DataBase
{
    public partial class Camp
    {
        public static List<LoadedImage> PreLoadImages = new List<LoadedImage>();
        public class LoadedImage
        {
            public int Id { get; set; }
            public ImageSource ImageSouce { get; set; }
        }

        public ImageSource Photo
        {
            get
            {
                var photo = PreLoadImages.FirstOrDefault(p => p.Id == this.Id);
                if (photo != null)
                    return photo.ImageSouce;
                else
                {
                    if (Image != null)
                    {
                        photo = new LoadedImage() { Id = this.Id, ImageSouce = Tools.BytesToImage(Image) };
                        PreLoadImages.Add(photo);
                        return photo.ImageSouce;
                    }
                    return null;
                }
            }
        }
        
        public string Location
        {
            get
            {
                return $"Location: ({X}; {Y})";
            }
        }

    }
}
