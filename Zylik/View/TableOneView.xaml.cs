using System.Windows.Controls;
using Zylik.ViewModel;

namespace Zylik.View
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
            for (byte i = 0; i < HelpClass.kolvo_lines; i++)
            {
                Node node = new Node();
                node.nodeNumber = (i+1).ToString();
                node.nodeStart = HelpClass.n[i].ToString();
                node.nodeFinish = HelpClass.k[i].ToString();
                node.nodeAo = HelpClass.ao[i].ToString();
                node.nodeP = HelpClass.p[i].ToString("0.000");
                node.nodeQ = HelpClass.q[i].ToString("0.000");
                node.nodeWpi = HelpClass.Wpi[i].ToString("0.000");
                node.nodeWqi = HelpClass.Wqi[i].ToString("0.000");
                node.nodeU = HelpClass.U[i].ToString("0.000");
                node.nodedQli = HelpClass.dQli[i].ToString("0.000");
                node.nodedPli = HelpClass.dPli[i].ToString("0.000");
                node.nodedTmai = HelpClass.Tmai[i].ToString("0.000");
                node.nodedWi = HelpClass.dWi[i].ToString("0.000");
                node.nodedU = HelpClass.dU[i].ToString("0.000000");
                dataGridOne.Items.Add(node);
            }
            for (byte i = HelpClass.kolvo_lines; i < (HelpClass.kolvo_trans + HelpClass.kolvo_lines); i++)
            {
                Node node = new Node();
                node.nodeNumber = (i + 1).ToString();
                node.nodeStart = HelpClass.n[i].ToString();
                node.nodeFinish = HelpClass.k[i].ToString();
                node.nodeAo = HelpClass.ao[i].ToString();
                node.nodeP = HelpClass.p[i].ToString("0.000");
                node.nodeQ = HelpClass.q[i].ToString("0.000");
                node.nodeWpi = HelpClass.Wpi[i].ToString("0.000");
                node.nodeWqi = HelpClass.Wqi[i].ToString("0.000");
                node.nodeU = HelpClass.U[i].ToString("0.000");
                node.nodedQli = HelpClass.dQj[i].ToString("0.000");
                node.nodedPli = HelpClass.dPj[i].ToString("0.000");
                node.nodedTmai = HelpClass.Tmaj[i].ToString("0.000");
                node.nodedWi = HelpClass.dWj[i].ToString("0.000");
                node.nodedU = HelpClass.dU[i].ToString("0.000000");
                dataGridOne.Items.Add(node);
            }
        }
public class Node
{
    public string nodeNumber { get; set; }
    public string nodeStart { get; set; }
    public string nodeFinish { get; set; }
    public string nodeAo { get; set; }
    public string nodeP { get; set; }
    public string nodeQ { get; set; }
    public string nodeWpi { get; set; }
    public string nodeWqi { get; set; }
    public string nodeU { get; set; }
    public string nodedQli { get; set; }
    public string nodedPli { get; set; }
    public string nodedTmai { get; set; }
    public string nodedWi { get; set; }
    public string nodedU { get; set; }
}

    }
}
