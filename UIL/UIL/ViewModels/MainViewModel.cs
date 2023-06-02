using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UIL.Commands;
using DAL;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace UIL.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ResultViewModel _ResultViewModel { get; }
        public SearchViewModel _SearchViewModel { get; }
        public event EventHandler<string?>? SearchTextChanged;
        public event EventHandler<tours?>? SelectedItemChanged;

        public ICommand SearchCommand { get; }

        public ICommand ClearCommand { get; }

        private string? searchText;
        public string? SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged();
            }
        }
        private ImageSource mapImage;
        public ImageSource MapImage
        {
            get { return mapImage; }
            set
            {
                mapImage = value;
                OnPropertyChanged(nameof(MapImage));
            }
        }
        private tours? selectedItem;
        public tours? SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                OnPropertyChanged();
                OnSelectedItemChanged();
            }
        }
        private BaseViewModel _selectedViewModel;
        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }
       
        
        public ICommand UpdateViewCommand { get; set; }
        TourLogHandler tourLog = new TourLogHandler();
        TourHandler tour = new TourHandler();

        public MainViewModel(SearchViewModel searchViewModel, ResultViewModel resultViewModel)
        {
             _ResultViewModel = resultViewModel;
             _SearchViewModel = searchViewModel;

            //UpdateViewCommand = new UpdateViewCommand(this);

            searchViewModel.SearchTextChanged += (_, searchText) =>
            {
                Search(searchText);
                
            };
            resultViewModel.SelectedItemChanged += (_, selectedItem) =>
            {
                if (selectedItem != null)
                {
                    SearchLogs(selectedItem);
                    GetImage(selectedItem);
                }
                
            };
            
            
            
            
            
        }
        private void Search(string? searchText)
        {
            
            List<tours> tourlist = new List<tours>();
            tourlist = tour.SearchForTours(searchText);
            _ResultViewModel.SetItems(tourlist);
            

            
            List<tourlogs> tourloglist = new List<tourlogs>();
            tourloglist = tourLog.SearchForLogs(searchText);
            _ResultViewModel.SetTourLogs(tourloglist);

        }
        private void SearchLogs(tours tour)
        {
            if (tour != null)
            {
                
                List<tourlogs> tourlogs = new List<tourlogs>();
                tourlogs = tourLog.Search(tour.id);
                _ResultViewModel.SetTourLogs(tourlogs);
            }
        }
        public async void GetImage(tours tour)
        {
            if(tour != null)
            {
                string picturename = AppDomain.CurrentDomain.BaseDirectory + "\\Images\\" +  tour.information;
                Uri uri = new Uri(picturename, UriKind.RelativeOrAbsolute);
                _ResultViewModel.MapImage = new BitmapImage(uri);
            }
        }
        private void OnSelectedItemChanged()
        {
            SelectedItemChanged?.Invoke(this, SelectedItem);
        }
    }
}
