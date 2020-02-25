using System.Collections.Generic;
using Zylik.Models;

namespace Energy.Models
{
    class TableOneModel
    {
        private MainModel mainModel;

        public List<NodeFirst> Nodes { get; private set; }

        public TableOneModel()
        {
            mainModel = new MainModel();

            Nodes = GetLines();
            Nodes.AddRange(GetTransformers());
        }
        private List<NodeFirst> GetLines()
        {
            var lines = new List<NodeFirst>();
            for (byte i = 0; i < Lines.Count; i++)
            {
                var node = new NodeFirst();
                node.nodeNumber = (i + 1).ToString();
                node.nodeStart = mainModel.n[i].ToString();
                node.nodeFinish = mainModel.k[i].ToString();
                node.nodeAo = mainModel.ao[i].ToString();
                node.nodeP = mainModel.p[i].ToString("0.000");
                node.nodeQ = mainModel.q[i].ToString("0.000");
                node.nodeWpi = mainModel.Wpi[i].ToString("0.000");
                node.nodeWqi = mainModel.Wqi[i].ToString("0.000");
                node.nodeU = mainModel.U[i].ToString("0.000");
                node.nodedQli = mainModel.dQli[i].ToString("0.000");
                node.nodedPli = mainModel.dPli[i].ToString("0.000");
                node.nodedTmai = mainModel.Tmai[i].ToString("0.000");
                node.nodedWi = mainModel.dWi[i].ToString("0.000");
                node.nodedU = mainModel.dU[i].ToString("0.000000");
                lines.Add(node);
            }
            return lines;
        }
        private List<NodeFirst> GetTransformers()
        {
            var transformrs = new List<NodeFirst>();
            for (byte i = Lines.Count; i < Lines.Count+Transformers.Count; i++)
            {
                var node = new NodeFirst();
                node.nodeNumber = (i + 1).ToString();
                node.nodeStart = mainModel.n[i].ToString();
                node.nodeFinish = mainModel.k[i].ToString();
                node.nodeAo = mainModel.ao[i].ToString();
                node.nodeP = mainModel.p[i].ToString("0.000");
                node.nodeQ = mainModel.q[i].ToString("0.000");
                node.nodeWpi = mainModel.Wpi[i].ToString("0.000");
                node.nodeWqi = mainModel.Wqi[i].ToString("0.000");
                node.nodeU = mainModel.U[i].ToString("0.000");
                node.nodedQli = mainModel.dQj[i].ToString("0.000");
                node.nodedPli = mainModel.dPj[i].ToString("0.000");
                node.nodedTmai = mainModel.Tmaj[i].ToString("0.000");
                node.nodedWi = mainModel.dWj[i].ToString("0.000");
                node.nodedU = mainModel.dU[i].ToString("0.000000");
                transformrs.Add(node);
            }
            return transformrs;
        }
    }
}
