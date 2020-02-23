using Energy;
using System.Collections.Generic;

namespace Zylik.Models
{
    class TableThreeModelcs
    {
        public List<NodeEnergyLoss> Nodes { get; private set; }

        public TableThreeModelcs()
        {
            Nodes = GetNodeEnergyLosses();
            Nodes.AddRange(GetNodeEnergyLossesPercent());
        }
        private List<NodeEnergyLoss> GetNodeEnergyLosses()
        {
            var nodes = new List<NodeEnergyLoss>(1);

            var node = new NodeEnergyLoss();
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
            nodes.Add(node);

            return nodes;
        }
        private List<NodeEnergyLoss> GetNodeEnergyLossesPercent()
        {
            var nodes = new List<NodeEnergyLoss>(1);

            NodeEnergyLoss nodeproc = new NodeEnergyLoss();
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
            nodes.Add(nodeproc);

            return nodes;
        }
    }
}
