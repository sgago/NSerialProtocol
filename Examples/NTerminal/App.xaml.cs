using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using NTerminal.ViewModels;

namespace NTerminal
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ComposeObjects();
        }

        private void ComposeObjects()
        {
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();

            Current.MainWindow = new MainWindowView();
            Current.MainWindow.DataContext = mainWindowViewModel;
            Current.MainWindow.Show();
        }
    }
}
