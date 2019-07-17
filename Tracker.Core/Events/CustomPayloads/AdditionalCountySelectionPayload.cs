using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Core.StaticTypes;

namespace Tracker.Core.Events.CustomPayloads
{
    public class AdditionalCountySelectionPayload
    {
        public County CountyPayload { get; set; } 
        public bool IsAdded { get; set; }
    }
}
