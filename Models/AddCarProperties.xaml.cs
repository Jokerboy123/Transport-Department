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
    }
}
