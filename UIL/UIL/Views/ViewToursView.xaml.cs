using BL;
using DAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using UIL.Commands;
using UIL.ViewModels;

namespace UIL.Views
{
    /// <summary>
    /// Interaktionslogik für OptionsView.xaml
    /// </summary>
    public partial class ViewToursView : UserControl
    {
        ObservableCollection<tours> tourlist;
        public ViewToursView()
        {
            InitializeComponent();

            tourlist = new ObservableCollection<tours>();
            FillObservableCollection();
            ViewToursModel model = new ViewToursModel();
            tours thetour = (tours)alltours.SelectedItem;
            model.GetAllTourInfos(thetour);
            

            
            alltours.ItemsSource = tourlist;

        }

        public void FillObservableCollection()
        {
            DALConfiguration configuration = new DALConfiguration();
            ToursContext context = new ToursContext(configuration.configuration);
            ToursSQLRepository repository = new ToursSQLRepository(context);
            var tourrepo = repository.GetAllTours();

            foreach (var tour in tourrepo)
            {
                tourlist.Add(tour);
            }
        }
    }
}
