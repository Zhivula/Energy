using System.Windows.Controls;
using Energy.ViewModel;

namespace Energy.View
{
    /// <summary>
    /// Логика взаимодействия для CollectionPageView.xaml
    /// </summary>
    public partial class TableTwoView : UserControl
    {
        public TableTwoView()
        {
            InitializeComponent();
            DataContext = new TableTwoViewModel();
        }
    }
}
