using Energy.ViewModel;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Energy.View
{
    /// <summary>
    /// Логика взаимодействия для GraphPageView.xaml
    /// </summary>
    public partial class GraphPageView : System.Windows.Controls.UserControl
    {
        public GraphPageView()
        {                           
            InitializeComponent();
            WinHost.Child = new GraphViewModel().Host;
        }
    }
}
