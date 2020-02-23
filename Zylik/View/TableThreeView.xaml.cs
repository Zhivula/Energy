using System.Windows.Controls;
using Zylik.ViewModel;

namespace Zylik.View
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
            NodePotery node = new NodePotery();
            node.node = "Все потери";
            node.nodedPt = HelpClass.dPt.ToString("0.000");
            node.nodedQt = HelpClass.dQt.ToString("0.000");
            node.nodedPxx = HelpClass.dPxx.ToString("0.000");
            node.nodedQxx = HelpClass.dQxx.ToString("0.000");
            node.nodedWt = HelpClass.dWt.ToString("0.000");
            node.nodedWxx = HelpClass.dWxx.ToString("0.000");
            node.nodedQl = HelpClass.dQl.ToString("0.000");
            node.nodedPl = HelpClass.dPl.ToString("0.000");
            node.nodedWl = HelpClass.dWl.ToString("0.000");
            node.nodedW = HelpClass.dW.ToString("0.000");
            node.nodedQ = HelpClass.dQ.ToString("0.000");
            node.nodedP = HelpClass.dP.ToString("0.000");
            node.nodedQlt = HelpClass.dQlt.ToString("0.000");
            node.nodedPlt = HelpClass.dPlt.ToString("0.000");
            dataGridThree.Items.Add(node);
            NodeProc nodeproc = new NodeProc();
            nodeproc.node = "В процентах";
            nodeproc.nodedPt = HelpClass.dPtproc.ToString("0.000");
            nodeproc.nodedQt = HelpClass.dQtproc.ToString("0.000");
            nodeproc.nodedPxx = HelpClass.dPxxproc.ToString("0.000");
            nodeproc.nodedQxx = HelpClass.dQxxproc.ToString("0.000");
            nodeproc.nodedWt = HelpClass.dWtproc.ToString("0.000");
            nodeproc.nodedWxx = HelpClass.dWxxproc.ToString("0.000");
            nodeproc.nodedQl = HelpClass.dQlproc.ToString("0.000");
            nodeproc.nodedPl = HelpClass.dPlproc.ToString("0.000");
            nodeproc.nodedWl = HelpClass.dWlproc.ToString("0.000");
            nodeproc.nodedW = HelpClass.dWproc.ToString("0.000");
            nodeproc.nodedQ = HelpClass.dQproc.ToString("0.000");
            nodeproc.nodedP = HelpClass.dPproc.ToString("0.000");
            nodeproc.nodedQlt = HelpClass.dQltproc.ToString("0.000");
            nodeproc.nodedPlt = HelpClass.dPltproc.ToString("0.000");
            dataGridThree.Items.Add(nodeproc);
        }
        public class NodePotery
        {
            public string node { get; set; }
            public string nodedPt { get; set; }
            public string nodedQt { get; set; }
            public string nodedPxx { get; set; }
            public string nodedQxx { get; set; }
            public string nodedWt { get; set; }
            public string nodedWxx { get; set; }
            public string nodedQl { get; set; }
            public string nodedPl { get; set; }
            public string nodedWl { get; set; }
            public string nodedW { get; set; }
            public string nodedQ { get; set; }
            public string nodedP { get; set; }
            public string nodedQlt { get; set; }
            public string nodedPlt { get; set; }
        }
        public class NodeProc
        {
            public string node { get; set; }
            public string nodedPt { get; set; }
            public string nodedQt { get; set; }
            public string nodedPxx { get; set; }
            public string nodedQxx { get; set; }
            public string nodedWt { get; set; }
            public string nodedWxx { get; set; }
            public string nodedQl { get; set; }
            public string nodedPl { get; set; }
            public string nodedWl { get; set; }
            public string nodedW { get; set; }
            public string nodedQ { get; set; }
            public string nodedP { get; set; }
            public string nodedQlt { get; set; }
            public string nodedPlt { get; set; }
        }
    }
}
