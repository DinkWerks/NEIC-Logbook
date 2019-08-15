using Prism.Commands;

namespace Tracker.Core.CompositeCommands
{
    public class ApplicationCommands : IApplicationCommands
    {
        private CompositeCommand _saveCompCommand = new CompositeCommand(true);
        private CompositeCommand _removeCompCommand = new CompositeCommand(true);

        public CompositeCommand SaveCompCommand
        {
            get { return _saveCompCommand; }
        }

        public CompositeCommand DeleteCompCommand
        {
            get { return _removeCompCommand; }
        }
    }
}
