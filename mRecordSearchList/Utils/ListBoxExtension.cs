using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace mRecordSearchList.Utils
{
    public static class ListBoxExtension
    {
        // Using a DependencyProperty as the backing store for SearchValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemListProperty =
            DependencyProperty.RegisterAttached("SelectedItemList", typeof(IList), typeof(ListBoxExtension),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnSelectedItemListChanged)));

        public static IList GetSelectedItemList(DependencyObject obj)
        {
            return (IList)obj.GetValue(SelectedItemListProperty);
        }

        public static void SetSelectedItemList(DependencyObject obj, IList value)
        {
            obj.SetValue(SelectedItemListProperty, value);
        }

        private static void OnSelectedItemListChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var listbox = d as ListBox;
            if (listbox != null)
            {
                listbox.SelectedItems.Clear();
                var selectedItems = e.NewValue as IList;
                if (selectedItems != null)
                {
                    foreach (var item in selectedItems)
                    {
                        listbox.SelectedItems.Add(item);
                    }
                }
            }
        }
    }
}
