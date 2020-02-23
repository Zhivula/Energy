using System.Windows.Controls;
using Energy.ViewModel;

namespace Energy.View
{
    /// <summary>
    /// Логика взаимодействия для CreditorsPageView.xaml
    /// </summary>
    public partial class TableThreeView : UserControl
    {
        public TableThreeView()
        {
            InitializeComponent();
            DataContext = new TableThreeViewModel();
        }
    }
}
