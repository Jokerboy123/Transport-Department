using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TransportDepartmentMVVM.Data;
using TransportDepartmentMVVM.Models;
using TransportDepartmentMVVM.Services;
using TransportDepartmentMVVM.Views;


namespace TransportDepartmentMVVM
{
    /// <summary>
    /// Логика взаимодействия для AccountGeorgievsk.xaml
    /// </summary>
    public partial class AccountGeorgievsk : Window
    {
        private Window mainWindow;

        public AccountGeorgievsk(MainWindow main)
        {
            InitializeComponent();

            var vm = new AccountGeorgievskViewModel(main);
            DataContext = vm;

            vm.OnHideRequested += () => this.Hide();
            vm.OnShowRequested += () => this.Show();
            vm.OnCloseRequested += () => this.Close();
        }

        public AccountGeorgievsk(Window mainWindow)
        {
            this.mainWindow = mainWindow;
        }
    }

}
