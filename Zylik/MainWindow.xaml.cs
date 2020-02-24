using System;
using System.IO;
using System.Linq;
using System.Windows;
using Energy.ViewModel;

namespace Energy
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //<summary>
        //ЗАПУСКАЕТСЯ ПРИ ЗАПУСКЕ ПРИЛОЖЕНИЯ -> ВНОСИТ НАЧАЛЬНЫЕ ДАННЫЕ, СОХРАНЕННЫЕ В ПАМЯТИ ПОД ПРИЛОЖЕНИЕ
        //</summary>
        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            window.mainGrid.Children.Add(new View.TableOneView());
        }
        public MainWindow()
        {
           
            InitializeComponent();//инициализация компонентов    
            DataContext = new MainWindowViewModel();
        }
        //<summary>
        //ЗАПУСКАЕТСЯ ПРИ ЗАПУСКЕ ПРИЛОЖЕНИЯ -> ВЫХОДЕ ИЗ ПРИЛОЖЕНИЯ
        //</summary>
        private void WindowClosing(object sender, RoutedEventArgs e)
        {
            var response = MessageBox.Show("Вы действительно хотите выйти?", "ВЫХОД", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (response == MessageBoxResult.Yes) Application.Current.Shutdown();
        }
    }
}
