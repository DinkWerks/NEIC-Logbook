using Prism.Commands;

namespace Tracker.Core.CompositeCommands
{
    public class ApplicationCommands : IApplicationCommands
    {
        private CompositeCommand _saveCompCommand = new CompositeCommand(true);
        private CompositeCommand _removeCompCommand = new CompositeCommand(true);
        private CompositeCommand _backCompCommand = new CompositeCommand(true);
        private CompositeCommand _forwardCompCommand = new CompositeCommand(true);

        public CompositeCommand SaveCompCommand
        {
            get { return _saveCompCommand; }
        }

        public CompositeCommand DeleteCompCommand
        {
            get { return _removeCompCommand; }
        }

        public CompositeCommand BackCompCommand
        {
            get { return _backCompCommand; }
        }

        public CompositeCommand ForwardCompCommand
        {
            get { return _forwardCompCommand; }
        }
    }
}
