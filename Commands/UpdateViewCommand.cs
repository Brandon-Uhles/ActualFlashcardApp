using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ActualFlashcardApp.ViewModels;

 namespace ActualFlashcardApp.Commands
{
    public class UpdateViewCommand : ICommand
    {
        private MainViewModel viewModel;

        public UpdateViewCommand(MainViewModel viewModel)
        {
            this.viewModel = viewModel;
        }


        public  event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if(parameter.ToString() == "New Deck")
            {
                viewModel.SelectedViewModel = new NewDeckViewModel();
            }
            else if(parameter.ToString() == "Edit Deck")
            {
                viewModel.SelectedViewModel = new EditDeckViewModel();
            }
            else if(parameter.ToString() == "Load Deck")
            {
                viewModel.SelectedViewModel = new LoadDeckViewModel();
            }
            else if(parameter.ToString() == "Statistics")
            {
                viewModel.SelectedViewModel = new StatisticsViewModel();
            }
        }
    }
}
