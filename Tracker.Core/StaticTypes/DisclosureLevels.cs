using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracker.Core.StaticTypes
{
    public static class DisclosureLevels
    {
        public static readonly DisclosureLevel Confidential = new DisclosureLevel();
    }

    public class DisclosureLevel
    {
        public DisclosureLevel()
        {

        }
    }
}
