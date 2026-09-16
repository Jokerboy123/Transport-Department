using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TransportDepartmentMVVM.ViewModels;

namespace TransportDepartmentMVVM
{
    /// <summary>
    /// Логика взаимодействия для AccountingCard.xaml
    /// </summary>
    public partial class AccountingCard : Window
    {
        public AccountingCard(Models.TransportProperties transport)
        {
            InitializeComponent();

            var viewModel = new ViewModels.AccountingCardViewModel(transport);
            this.DataContext = viewModel;
            viewModel.OnCloseRequested += () => this.Close();
            //vm.OnCloseRequested += () => this.Close();
        }
    }
}
