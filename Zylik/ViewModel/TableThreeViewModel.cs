using Energy.Helpers;
using System.Collections.Generic;
using Zylik.Models;

namespace Energy.ViewModel
{
    class TableThreeViewModel : ObservableObject
    {
        public List<NodeEnergyLoss> GetList { get; set; }

        public TableThreeViewModel()
        {
            GetList = new TableThreeModelcs().Nodes;
        }
    }
}
