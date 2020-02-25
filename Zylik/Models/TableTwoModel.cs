using Energy;
using System.Collections.Generic;

namespace Zylik.Models
{
    class TableTwoModel
    {
        private MainModel mainModel;

        public List<NodeSecond> Nodes { get; private set; }

        public TableTwoModel()
        {
            mainModel = new MainModel();

            Nodes = GetNodes();
        }
        private List<NodeSecond> GetNodes()
        {
            var nodes = new List<NodeSecond>();

            NodeSecond node = new NodeSecond();
            node.nodeWpgu = mainModel.Wpgy.ToString("0.0000");
            node.nodeQgu = mainModel.Qgy.ToString("0.0000");
            node.nodePgu = mainModel.Pgy.ToString("0.0000");
            nodes.Add(node);
            return nodes;
        }
    }
}
