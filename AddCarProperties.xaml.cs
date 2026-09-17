using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TransportDepartmentMVVM.ViewModels;

namespace TransportDepartmentMVVM
{
    public partial class AddCarProperties : Window
    {
        public AddCarProperties(ViewModels.AddCarPropertiesViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.OnCloseRequested += () => this.Close();

        }

        private void AddCarPropertiesLoaded(object sender, RoutedEventArgs e)
        {
            ClearAllTextBoxes();
        }

        private void ClearAllTextBoxes()
        {
            foreach (var tb in FindVisualChildren<TextBox>(this))
            {
                tb.Text = "";
            }
        }

        // Вспомогательный метод
        private IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T result)
                    yield return result;

                foreach (var descendant in FindVisualChildren<T>(child))
                    yield return descendant;
            }
        }

    }
}
