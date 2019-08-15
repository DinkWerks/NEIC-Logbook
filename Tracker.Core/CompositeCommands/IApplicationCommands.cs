using Prism.Commands;

namespace Tracker.Core.CompositeCommands
{
    public interface IApplicationCommands
    {
        CompositeCommand SaveCompCommand { get; }
        CompositeCommand DeleteCompCommand { get; }
    }
}
