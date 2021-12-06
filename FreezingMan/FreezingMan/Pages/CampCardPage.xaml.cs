using FreezingMan.DataBase;
using FreezingMan.Models;
using FreezingMan.Settings;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace FreezingMan.Pages
{
    public partial class CampCardPage : Page
    {
        Camp contextCamp;
        DbPropertyValues oldValues;
        public CampCardPage(Camp sentCamp)
        {
            InitializeComponent();
            CBStatuses.ItemsSource = GlobalSettings.DB.Status.ToList();
            oldValues = GlobalSettings.DB.Entry(sentCamp).CurrentValues.Clone();
            contextCamp = sentCamp;
            DataContext = contextCamp;
        }

        private void BChangeImage_Click(object sender, RoutedEventArgs e)
        {

            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog().GetValueOrDefault())
            {
                contextCamp.Image = File.ReadAllBytes(dialog.FileName);
                CampImage.Source = Tools.BytesToImage(contextCamp.Image);
            }
        }

        private void BDeleteImage_Click(object sender, RoutedEventArgs e)
        {
            contextCamp.Image = null;
            CampImage.Source = null;
        }

        private void BSave_Click(object sender, RoutedEventArgs e)
        {
            string errorMessage = "";
            if (contextCamp.X > 2000)
                errorMessage += "Location X is large than 2000\n";
            if (contextCamp.Y > 1000)
                errorMessage += "Location Y is large than 1000\n";
            if (string.IsNullOrWhiteSpace(contextCamp.StartWorkTime.ToString()))
                errorMessage += "Enter start work time\n";
            if (string.IsNullOrWhiteSpace(contextCamp.EndWorkTime.ToString()))
                errorMessage += "Enter end work time\n";
            if (!string.IsNullOrWhiteSpace(contextCamp.StartWorkTime.ToString()) && !string.IsNullOrWhiteSpace(contextCamp.EndWorkTime.ToString()) && contextCamp.StartWorkTime > contextCamp.EndWorkTime)
                errorMessage += "Start work time is large than end work time\n";
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                MessageBox.Show(errorMessage);
                return;
            }
            GlobalSettings.DB.SaveChanges();
            NavigationService.GoBack();
        }

        private void BCancel_Click(object sender, RoutedEventArgs e)
        {
            GlobalSettings.DB.Entry(contextCamp).CurrentValues.SetValues(oldValues);
            NavigationService.GoBack();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, @"[0-9:]"))
                e.Handled = true;
        }
    }
}
