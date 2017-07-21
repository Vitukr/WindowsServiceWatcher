using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceWatcher
{
    public class WatchRecord
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public bool IsFile { get; set; }
        public string Event { get; set; }
        public DateTime Date { get; set; }
    }
}
