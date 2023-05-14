using UIL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UIL.Commands
{
    public class UpdateViewCommand : ICommand
    {
        private MainViewModel viewModel;

        public UpdateViewCommand(MainViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter.ToString() == "Help")
            {
                viewModel.SelectedViewModel = new HelpViewModel();
            }
            else if (parameter.ToString() == "ViewTours")
            {
                viewModel.SelectedViewModel = new ViewToursModel();
            }
            else if(parameter.ToString() == "File")
            {
                viewModel.SelectedViewModel = new FileViewModel();
            }
            else if(parameter.ToString() == "Edit")
            {
                viewModel.SelectedViewModel = new EditViewModel();
            }
            else if(parameter.ToString() == "Tours")
            {
                viewModel.SelectedViewModel = new TourModel();
            }
        }
    }
}

