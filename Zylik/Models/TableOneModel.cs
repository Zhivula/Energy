using System.Collections.Generic;

namespace Energy.Models
{
    class TableOneModel
    {
        public List<NodeFirst> Nodes { get; private set; }

        public TableOneModel()
        {
            Nodes = GetLines();
            Nodes.AddRange(GetTransformers());
        }
        private List<NodeFirst> GetLines()
        {
            var lines = new List<NodeFirst>();
            for (byte i = 0; i < HelpClass.kolvo_lines; i++)
            {
                var node = new NodeFirst();
                node.nodeNumber = (i + 1).ToString();
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
                lines.Add(node);
            }
            return lines;
        }
        private List<NodeFirst> GetTransformers()
        {
            var transformrs = new List<NodeFirst>();
            for (byte i = HelpClass.kolvo_lines; i < (HelpClass.kolvo_trans + HelpClass.kolvo_lines); i++)
            {
                var node = new NodeFirst();
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
                transformrs.Add(node);
            }
            return transformrs;
        }
    }
}
