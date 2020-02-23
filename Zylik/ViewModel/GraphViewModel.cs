using Energy.Helpers;
using System.Drawing;
using System.Windows.Forms;
using Zylik.Models;

namespace Energy.ViewModel
{
    public class GraphViewModel : ObservableObject
    {
        public PictureBox Host { get; set; }

        public GraphViewModel()
        {
            var graph = new GraphModel();

            Host = graph.Picture;
        }
    }
}
