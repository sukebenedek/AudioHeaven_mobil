using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioHeaven.Models
{
    public class HistoryResponse
    {
        public int CurrentPage { get; set; }
        public List<HistoryItem> Data { get; set; }
    }

    public class HistoryItem
    {
        public int Id { get; set; }

        public Song Song { get; set; }
    }


}
