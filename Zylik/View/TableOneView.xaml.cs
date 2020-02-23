using System.Windows.Controls;
using Energy.ViewModel;

namespace Energy.View
{
    /// <summary>
    /// Логика взаимодействия для BorrowersPageView.xaml
    /// </summary>
    public partial class TableOneView : UserControl
    {
        public TableOneView()
        {
            InitializeComponent();
            DataContext = new TableOneViewModel();
        }

    }
}
