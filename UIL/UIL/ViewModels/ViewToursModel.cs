using BL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using UIL.Commands;

namespace UIL.ViewModels
{
    public class ViewToursModel : BaseViewModel
    {
        public event EventHandler<string?>? TextChanged;
        public ICommand GetTourInfos { set;  get; }

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
        public ViewToursModel()
        {
            
        }
        public void GetAllTourInfos(tours tour)
        {
            GetTourInfos = new CommandHandler((_) =>
            {
                _Name = tour.tourname;
                _Description = tour.description;
                _From = tour.startpoint;
                _To = tour.endpoint;
                _Type = tour.type;
                TextChanged?.Invoke(this, _Name);
                TextChanged?.Invoke(this, _Description);
                TextChanged?.Invoke(this, _From);
                TextChanged?.Invoke(this, _To);
                TextChanged?.Invoke(this, _Type);

            });
        }
    }
}
