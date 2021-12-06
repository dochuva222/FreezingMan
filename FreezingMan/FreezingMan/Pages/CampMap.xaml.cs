using FreezingMan.DataBase;
using FreezingMan.Models;
using FreezingMan.Settings;
using FreezingMan.UserControls;
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
    public partial class CampMap : Page
    {
        List<Camp> _filtredCamps = new List<Camp>();
        public CampMap()
        {
            InitializeComponent();
            var types = new List<CampType>() { new CampType() { Name = "All" } };
            var statuses = new List<Status>() { new Status() { Name = "All" } };
            types.AddRange(GlobalSettings.DB.CampType.ToList());
            statuses.AddRange(GlobalSettings.DB.Status.ToList());
            CBTypes.ItemsSource = types;
            CBStatuses.ItemsSource = statuses;
            CBTypes.SelectedIndex = 0;
            CBStatuses.SelectedIndex = 0;
            Refresh();
        }

        private void Refresh()
        {
            _filtredCamps = GlobalSettings.DB.Camp.ToList();
            if (CBTypes.SelectedIndex != 0)
                _filtredCamps = _filtredCamps.Where(f => f.CampTypeId == CBTypes.SelectedIndex).ToList();
            if (CBStatuses.SelectedIndex != 0)
                _filtredCamps = _filtredCamps.Where(f => f.StatusId == CBStatuses.SelectedIndex).ToList();
            CreateCampIcons(_filtredCamps);
        }

        private void CreateCampIcons(List<Camp> filtredCamps)
        {
            MapCanvas.Children.Clear();
            foreach (var camp in GlobalSettings.DB.Camp.ToList())
            {
                var campIcon = new CampItemControl(camp);
                if (_filtredCamps.FirstOrDefault(c => c.Id == camp.Id) == null)
                    campIcon.Ellipse.Opacity = 0.2;
                campIcon.MouseEnter += CampIcon_MouseEnter;
                campIcon.MouseLeave += CampIcon_MouseLeave;
                campIcon.MouseDown += CampIcon_MouseDown;
                Canvas.SetLeft(campIcon, camp.X.Value);
                Canvas.SetTop(campIcon, camp.Y.Value);
                MapCanvas.Children.Add(campIcon);
            }
        }

        private void CampIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var selectedCamp = (sender as CampItemControl).DataContext as Camp;
            if (selectedCamp != null)
                NavigationService.Navigate(new CampCardPage(selectedCamp));
        }

        private void CampIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            var campIcon = sender as CampItemControl;
            var selectedCamp = (sender as CampItemControl).DataContext as Camp;
            campIcon.TBName.Visibility = Visibility.Visible;
            campIcon.Ellipse.Opacity = 1;
        }

        private void CampIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            var campIcon = sender as CampItemControl;
            var selectedCamp = (sender as CampItemControl).DataContext as Camp;
            campIcon.TBName.Visibility = Visibility.Hidden;
            if (_filtredCamps.FirstOrDefault(f => f.Id == selectedCamp.Id) == null)
                campIcon.Ellipse.Opacity = 0.2;
        }


        private void CBTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void CBStatuses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void MapCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(MapCanvas);
            double x = position.X - 50;
            double y = position.Y - 50;
            if (x < 0)
                x = 0;
            if (y < 0)
                y = 0;
            TBLocation.Text = $"X={Math.Round(x, 0)}; Y={Math.Round(y, 0)}";
        }
    }
}
