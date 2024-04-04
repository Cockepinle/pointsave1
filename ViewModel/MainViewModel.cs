using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using POINT.ViewModel.Helpers;

namespace POINT.ViewModel
{
    internal class MainViewModel : BindingHelper
    {
        public ICommand Start { get; set; }
        public MainWindow Window { get; set; }
        public MainViewModel()
        {
            Start = new BindableCommand(StartMoment);
        }
        public void StartMoment(object parameter)
        {
            WindowStart windowStart = new WindowStart();
            windowStart.Show();
            Window.Close();
        }
    }
}
