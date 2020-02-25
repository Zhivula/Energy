using System.Linq;
using System.Windows;
using Energy.Helpers;
using Zylik.Models;

namespace Energy.ViewModel
{
    public class MainWindowViewModel
    {
        MainWindow window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        MainModel model;
        public MainWindowViewModel()
        {
            OpenTableOneCommand = new DelegateCommand(o => OpenTableOne());
            OpenTableTwoCommand = new DelegateCommand(o => OpenTableTwo());
            OpenTableThreeCommand = new DelegateCommand(o => OpenTableThree());
            OpenGraphCommand = new DelegateCommand(o => OpenGraph()); 
        }
        #region Command
        public DelegateCommand OpenTableOneCommand { get; set; }
        public DelegateCommand OpenTableTwoCommand { get; set; }
        public DelegateCommand OpenTableThreeCommand { get; set; }
        public DelegateCommand OpenGraphCommand { get; set; }
        #endregion
        #region Command implementation
        private void OpenTableOne()
        {
            window.mainGrid.Children.Clear();
            window.mainGrid.Children.Add(new View.TableOneView());
        }
        private void OpenGraph( )
        {
            window.mainGrid.Children.Clear();
            window.mainGrid.Children.Add(new View.GraphPageView());
        }
        private void OpenTableTwo()
        {
            window.mainGrid.Children.Clear();
            window.mainGrid.Children.Add(new View.TableTwoView());
        }
        private void OpenTableThree()
        {
            window.mainGrid.Children.Clear();
            window.mainGrid.Children.Add(new View.TableThreeView());
        }
        #endregion
    }
}
