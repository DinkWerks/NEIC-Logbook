using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracker.Core.Events.Payloads
{
    public class ListModificationPayload
    {
        public string Action { get; set; }
        public int ID { get; set; }

        public ListModificationPayload(string action, int id)
        {
            Action = action;
            ID = id;
        }
    }
}
