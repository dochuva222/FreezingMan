using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FreezingMan.Models
{
    public static class Tools
    {
        public static BitmapImage BytesToImage(byte[] bytes)
        {
            using(MemoryStream memory = new MemoryStream(bytes))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = memory;
                image.EndInit();
                return image;
            }
        }
    }
}
