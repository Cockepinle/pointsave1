using DevExpress.Data.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POINT.ViewModel.Helpers;
using System.Windows.Interactivity;
using System.Windows.Input;
using System.Windows;

namespace POINT.ViewModel
{
    internal class ViewModelSave : Helpers.BindingHelper
    {
        public ICommand Save { get; set; }
        private bool buttonClicked = false;
        public ViewModelSave() 
        {
            Save = new BindableCommand(SaveEnd);
        }
       
        public void SaveEnd(object parameters)
        {
            buttonClicked = true;

            if (Application.Current.MainWindow is Window saveWindow)
            {
                saveWindow.Close();
            }

            WindowStart windowStart = new WindowStart();
            windowStart.Show();
        }

    }
}
