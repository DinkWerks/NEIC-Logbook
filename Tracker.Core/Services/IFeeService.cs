using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Core.Models.Fees;

namespace Tracker.Core.Services
{
    public interface IFeeService
    {
        string ConnectionString { get; set; }
        void SetConnectionString();
        Fee GetFeeData(Fee returnValue, bool loadAsCurrentSearch = true);
    }
}
