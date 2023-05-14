using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BL;
using DAL;
using UIL.Commands;

namespace UIL.ViewModels
{
    public class HelpViewModel : BaseViewModel
    {
        //DataHandler handler = new DataHandler();

        public event EventHandler<string?>? TextChanged;
        public ICommand MapQuestCommand { get; }
        private string? text;
        public string? Text
        {
            get => text;
            set
            {
                text = value;
                OnPropertyChanged();
            }
        }


        public HelpViewModel()
        {
            MapQuestCommand = new CommandHandler((_) =>
            {
                MapQuestTest();
                TextChanged?.Invoke(this, Text);
            });
        }
        public async void MapQuestTest()
        {
            MapServer server = new BL.MapServer();
        }

    }
}
