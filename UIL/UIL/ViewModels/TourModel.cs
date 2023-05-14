using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DAL;
using UIL.Commands;
using UIL.Views;
using BL;

namespace UIL.ViewModels
{
    public class TourModel : BaseViewModel
    {

        public event EventHandler<string?>? TextChanged;
        public ICommand GetTourInfos { get; }
        private string? _name;
        private string? _description;
        private string? _from;
        private string? _to;
        private string? _type;
        public string? _Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public string? _Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }
        public string? _From
        {
            get => _from;
            set
            {
                _from = value;
                OnPropertyChanged();
            }
        }
        public string? _To
        {
            get => _to; 
            set
            {
                _to = value;
                OnPropertyChanged();
            }   
        }
        public string? _Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged();
            }
        }


        public TourModel()
        {
            GetTourInfos = new CommandHandler((_) =>
            {
                TextChanged?.Invoke(this, _Name);
                TextChanged?.Invoke(this, _Description);
                TextChanged?.Invoke(this, _From);
                TextChanged?.Invoke(this, _To);
                TextChanged?.Invoke(this, _Type);

                CreateTourForDB createTourForDB = new BL.CreateTourForDB(_Name, _Description, _From, _To, _Type);
            });
        }

    }
}
