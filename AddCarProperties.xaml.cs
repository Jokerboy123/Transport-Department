using System.Windows;

namespace TransportDepartmentMVVM
{
    public partial class AddCarProperties : Window
    {
        public AddCarProperties(ViewModels.AddCarPropertiesViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.CloseRequested += (s, e) => this.Close();
        }
    }
}
