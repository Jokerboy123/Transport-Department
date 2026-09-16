using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using TransportDepartmentMVVM.ViewModels;

namespace TransportDepartmentMVVM
{
    public partial class AddCarProperties : Window
    {
        public AddCarProperties()
        {
            InitializeComponent();
            var vm = (AddCarPropertiesViewModel)DataContext;
            vm.OnCloseRequested += () => this.Close();

        }
    }
}
