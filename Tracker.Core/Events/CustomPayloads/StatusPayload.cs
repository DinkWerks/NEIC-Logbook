using System.Windows.Media;
using Tracker.Core.Extensions;

namespace Tracker.Core.Events.CustomPayloads
{
    public class StatusPayload
    {
        public Color AlertColor { get; set; }
        public string Message { get; set; }

        //Constructors
        public StatusPayload(string message, string hexCode)
        {
            Message = message;
            AlertColor = hexCode.ToColor();
        }

        public StatusPayload(string message, Color color)
        {
            Message = message;
            AlertColor = color;
        }
    }
}
