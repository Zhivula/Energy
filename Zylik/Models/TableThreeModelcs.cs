using Energy;
using System.Collections.Generic;

namespace Zylik.Models
{
    class TableThreeModelcs
    {
        private MainModel mainModel;

        public List<NodeEnergyLoss> Nodes { get; private set; }

        public TableThreeModelcs()
        {
            mainModel = new MainModel();

            Nodes = GetNodeEnergyLosses();
            Nodes.AddRange(GetNodeEnergyLossesPercent());
        }
        private List<NodeEnergyLoss> GetNodeEnergyLosses()
        {
            var nodes = new List<NodeEnergyLoss>(1);

            var node = new NodeEnergyLoss();
            node.node = "Все потери";
            node.nodedPt = mainModel.dPt.ToString("0.000");
            node.nodedQt = mainModel.dQt.ToString("0.000");
            node.nodedPxx = mainModel.dPxx.ToString("0.000");
            node.nodedQxx = mainModel.dQxx.ToString("0.000");
            node.nodedWt = mainModel.dWt.ToString("0.000");
            node.nodedWxx = mainModel.dWxx.ToString("0.000");
            node.nodedQl = mainModel.dQl.ToString("0.000");
            node.nodedPl = mainModel.dPl.ToString("0.000");
            node.nodedWl = mainModel.dWl.ToString("0.000");
            node.nodedW = mainModel.dW.ToString("0.000");
            node.nodedQ = mainModel.dQ.ToString("0.000");
            node.nodedP = mainModel.dP.ToString("0.000");
            node.nodedQlt = mainModel.dQlt.ToString("0.000");
            node.nodedPlt = mainModel.dPlt.ToString("0.000");
            nodes.Add(node);

            return nodes;
        }
        private List<NodeEnergyLoss> GetNodeEnergyLossesPercent()
        {
            var nodes = new List<NodeEnergyLoss>(1);

            NodeEnergyLoss nodeproc = new NodeEnergyLoss();
            nodeproc.node = "В процентах";
            nodeproc.nodedPt = mainModel.dPtproc.ToString("0.000");
            nodeproc.nodedQt = mainModel.dQtproc.ToString("0.000");
            nodeproc.nodedPxx = mainModel.dPxxproc.ToString("0.000");
            nodeproc.nodedQxx = mainModel.dQxxproc.ToString("0.000");
            nodeproc.nodedWt = mainModel.dWtproc.ToString("0.000");
            nodeproc.nodedWxx = mainModel.dWxxproc.ToString("0.000");
            nodeproc.nodedQl = mainModel.dQlproc.ToString("0.000");
            nodeproc.nodedPl = mainModel.dPlproc.ToString("0.000");
            nodeproc.nodedWl = mainModel.dWlproc.ToString("0.000");
            nodeproc.nodedW = mainModel.dWproc.ToString("0.000");
            nodeproc.nodedQ = mainModel.dQproc.ToString("0.000");
            nodeproc.nodedP = mainModel.dPproc.ToString("0.000");
            nodeproc.nodedQlt = mainModel.dQltproc.ToString("0.000");
            nodeproc.nodedPlt = mainModel.dPltproc.ToString("0.000");
            nodes.Add(nodeproc);

            return nodes;
        }
    }
}
