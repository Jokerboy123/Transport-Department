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
    /// Логика взаимодействия для DemonstrationCard.xaml
    /// </summary>
    public partial class DemonstrationCard : Window
    {
        private readonly DemonstrationCardViewModel _vm;
        public DemonstrationCard(DemonstrationCardViewModel vm)
        {
            InitializeComponent();

            _vm = vm;                         // сохраняем ссылку, если нужно
            DataContext = _vm;                 // сразу ставим DataContext

            // И ТОЛЬКО теперь подписываемся — DataContext точно не null
            _vm.OnCloseRequested += () => this.Close();

        }

     
    }
}
