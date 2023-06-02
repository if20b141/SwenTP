using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using UIL.Commands;

namespace UIL.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        public event EventHandler<string?>? SearchTextChanged;
        public event EventHandler<ImageSource?>? ImageChanged;

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

        public SearchViewModel()
        {
            SearchCommand = new CommandHandler((_) =>
            {
                SearchTextChanged?.Invoke(this, SearchText);
            });

            ClearCommand = new CommandHandler((_) =>
            {
                SearchText = "";
                SearchTextChanged?.Invoke(this, SearchText);
                MapImage = null;
                ImageChanged?.Invoke(this, MapImage);
            });

        }
    }
}
