using FreezingMan.DataBase;
using FreezingMan.Settings;
using System;
using System.Collections.Generic;
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

namespace FreezingMan.Pages
{
    public partial class CampListPage : Page
    {
        public CampListPage()
        {
            InitializeComponent();
            var campTypes = new List<CampType>() { new CampType() { Name = "All" } };
            campTypes.AddRange(GlobalSettings.DB.CampType.ToList());
            CBCampTypes.ItemsSource = campTypes;
            CBCampTypes.SelectedIndex = 0;
        }

        private void TBSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Refresh();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            var filtred = GlobalSettings.DB.Camp.ToList();
            if (CBCampTypes.SelectedIndex != 0)
                filtred = filtred.Where(f => f.CampTypeId == (CBCampTypes.SelectedItem as CampType).Id).ToList();
            if (!string.IsNullOrWhiteSpace(TBSearch.Text))
                filtred = filtred.Where(f => f.Name.ToLower().Contains(TBSearch.Text.ToLower()) || f.Description.ToLower().Contains(TBSearch.Text.ToLower())).ToList();
            LVCamps.ItemsSource = filtred;
        }

        private void CBCampTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void LVCamps_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedCamp = LVCamps.SelectedItem as Camp;
            if (selectedCamp != null)
                NavigationService.Navigate(new CampCardPage(selectedCamp));
        }
    }
}
