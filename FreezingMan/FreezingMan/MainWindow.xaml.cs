using FreezingMan.Pages;
using FreezingMan.Settings;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FreezingMan
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new CampListPage());
        }

        private void BCampList_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CampListPage());
        }

        private void BMap_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CampMap());
        }

        private void BDownload_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog() { Multiselect = true };
            int id = 2;
            int imageId = 0;
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                var files = dialog.FileNames;
                for (int i = 100; i < GlobalSettings.DB.Camp.Where(c => c.Image == null).Count() - 1; i++)
                {
                    if (imageId > files.Length - 1)
                        imageId = 0;
                    var camp = GlobalSettings.DB.Camp.FirstOrDefault(c => c.Id == i);
                    if (camp != null)
                    {
                        camp.Image = null;
                        //camp.Image = File.ReadAllBytes(files[imageId]);
                        imageId++;
                    }
                }
                //foreach (var file in dialog.FileNames)
                //{
                //    var camp = GlobalSettings.DB.Camp.FirstOrDefault(c => c.Id == id);
                //    if (camp != null)
                //    {
                //        camp.Image = 
                //    }
                //    id++;
                //}
                GlobalSettings.DB.SaveChanges();
            }
        }
    }
}
