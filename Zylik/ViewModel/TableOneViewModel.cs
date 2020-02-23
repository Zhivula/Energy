using Energy.Helpers;
using Energy.Models;
using System.Collections.Generic;

namespace Energy.ViewModel
{
    public class TableOneViewModel : ObservableObject
    {
        public List<NodeFirst> GetList { get; set; }

        public TableOneViewModel()
        {
            GetList = new TableOneModel().Nodes;
        }
    }
}
