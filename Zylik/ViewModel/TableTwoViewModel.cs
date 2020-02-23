using Energy.Helpers;
using System.Collections.Generic;
using Zylik.Models;

namespace Energy.ViewModel
{
    class TableTwoViewModel : ObservableObject
    {
        public List<NodeSecond> GetList { get; set; }

        public TableTwoViewModel()
        {
            GetList = new TableTwoModel().Nodes;
            
        }
    }
}
