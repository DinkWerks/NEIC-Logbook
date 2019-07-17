using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tracker.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Tracker.Controls.LockableTextBox"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Tracker.Controls.LockableTextBox;assembly=Tracker.Controls.LockableTextBox"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:LockableTextBox/>
    ///
    /// </summary>
    public class LockableTextBox : Control
    {
        static LockableTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LockableTextBox),
                new FrameworkPropertyMetadata(typeof(LockableTextBox)));
        }

        // Properties

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(LockableTextBox),
                new FrameworkPropertyMetadata(String.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty IsLockedProperty =
            DependencyProperty.Register("IsLocked", typeof(bool), typeof(LockableTextBox),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsLocked
        {
            get { return (bool)GetValue(IsLockedProperty); }
            set { SetValue(IsLockedProperty, value); }
        }

        public static readonly RoutedEvent SwitchLockStateEvent =
            EventManager.RegisterRoutedEvent("SwitchLockState", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(LockableTextBox));

        public event RoutedEventHandler SwitchLockState
        {
            add { AddHandler(SwitchLockStateEvent, value); }
            remove { RemoveHandler(SwitchLockStateEvent, value); }
        }

        public void OnSwitchLockState(object sender, MouseButtonEventArgs e)
        {
            IsLocked = !IsLocked;
        }
    }
}
