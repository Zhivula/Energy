using System.Windows.Controls;
using Zylik.ViewModel;

namespace Zylik.View
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
            Node node = new Node();
            node.nodeWpgu = HelpClass.Wpgy.ToString("0.0000");
            node.nodeQgu = HelpClass.Qgy.ToString("0.0000");
            node.nodePgu = HelpClass.Pgy.ToString("0.0000");
            dataGridTwo.Items.Add(node);
        }
        public class Node
        {
            public string nodeWpgu { get; set; }
            public string nodeQgu { get; set; }
            public string nodePgu { get; set; }
        }
    }
}
