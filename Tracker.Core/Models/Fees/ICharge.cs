using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracker.Core.Models.Fees
{
    public interface ICharge
    {
        int Index { get; set; }
        string Name { get; set; }
        string Type { get; }
        string Description { get; set; }
        decimal Cost { get; set; }
        decimal TotalCost { get; }
    }
}
