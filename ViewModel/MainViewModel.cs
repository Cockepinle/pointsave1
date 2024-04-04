using System.Windows.Input;
using POINT.ViewModel.Helpers;

namespace POINT.ViewModel
{
    internal class MainViewModel : BindingHelper
    {
        public ICommand Start { get; set; }
        public MainWindow Window { get; set; }
        public MainViewModel(MainWindow Window)
        {
            Start = new BindableCommand(StartMoment);
            this.Window = Window;
        }
        public void StartMoment(object parameter)
        {
            WindowStart windowStart = new WindowStart();
            windowStart.Show();
            Window.Close();
        }
    }
}
