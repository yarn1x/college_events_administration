using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace college_events_desktop.ViewModels
{
    public abstract class BaseViewModel
    {
        public void ShowLoadingInterface(StackPanel container, UserControl loadingInterface)
        {
            container.Children.Add(loadingInterface);
        }
        public void ShowLoadingInterface(Grid container, UserControl loadingInterface, int row = 0, int column = 0)
        {
            container.Children.Add(loadingInterface);
            Grid.SetRow(loadingInterface, row);
            Grid.SetColumn(loadingInterface, column);
        }
    }
}
