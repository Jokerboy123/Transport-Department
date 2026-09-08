using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace TransportDepartment
{
    public partial class AddCarProperties : Window
    {
        public AddCarProperties()
        {
            InitializeComponent();
        }

        private void AddCarProperties_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                InitializePlaceholders();
            }), System.Windows.Threading.DispatcherPriority.DataBind);
        }

        private void InitializePlaceholders()
        {
            foreach (var tb in FindTextBoxes(this))
            {
                if (tb.Tag == null) continue;

                // ПРОПУСКАЕМ поля с Binding — ими управляет ViewModel
                var binding = BindingOperations.GetBindingExpression(tb, TextBox.TextProperty);
                if (binding != null) continue;

                string placeholder = tb.Tag.ToString();

                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    tb.Text = placeholder;
                    tb.Foreground = new SolidColorBrush(Color.FromRgb(153, 153, 153));
                }
            }
        }

        private static IEnumerable<TextBox> FindTextBoxes(DependencyObject parent)
        {
            if (parent == null) yield break;

            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is TextBox tb)
                    yield return tb;

                foreach (var descendant in FindTextBoxes(child))
                    yield return descendant;
            }
        }

        private void Placeholder_GotFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb == null || tb.Tag == null) return;

            // Если у поля есть Binding — не очищаем, пусть ViewModel решает
            var binding = BindingOperations.GetBindingExpression(tb, TextBox.TextProperty);
            if (binding != null) return;

            if (tb.Text == tb.Tag.ToString())
            {
                tb.Text = "";
                tb.Foreground = Brushes.Black;
            }
        }

        private void Placeholder_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb == null || tb.Tag == null) return;

            // Если у поля есть Binding — не возвращаем подсказку
            var binding = BindingOperations.GetBindingExpression(tb, TextBox.TextProperty);
            if (binding != null) return;

            if (string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.Text = tb.Tag.ToString();
                tb.Foreground = new SolidColorBrush(Color.FromRgb(153, 153, 153));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Привязать эти поля таблицы к полям класса и звписать в БД, затем вывести в датагриде");
        }
    }
}
