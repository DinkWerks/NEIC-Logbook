using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mReporting.Reporting
{
    public interface IReport
    {
        string Name { get; set; }
        string Description { get; set; }
        ParameterTypes Parameters { get; set; }
        void Execute(object[] parameters);
    }
}
