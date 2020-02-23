using Energy;
using System.Collections.Generic;

namespace Zylik.Models
{
    class TableTwoModel
    {
        public List<NodeSecond> Nodes { get; private set; }

        public TableTwoModel()
        {
            Nodes = GetNodes();
        }
        private List<NodeSecond> GetNodes()
        {
            var nodes = new List<NodeSecond>();

            NodeSecond node = new NodeSecond();
            node.nodeWpgu = HelpClass.Wpgy.ToString("0.0000");
            node.nodeQgu = HelpClass.Qgy.ToString("0.0000");
            node.nodePgu = HelpClass.Pgy.ToString("0.0000");
            nodes.Add(node);
            return nodes;
        }
    }
}
