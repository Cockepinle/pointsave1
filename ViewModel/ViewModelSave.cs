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
        public WindowSave Window { get; set; }
        public ViewModelSave(WindowSave Window) 
        {
            Save = new BindableCommand(SaveEnd);
            this.Window = Window;
        }

        public void SaveEnd(object parameters)
        {
            WindowStart windowStart = new WindowStart();
            windowStart.Show();
            Window.Close();
        }

    }
}
