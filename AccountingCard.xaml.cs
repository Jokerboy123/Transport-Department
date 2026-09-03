using System.Data.SQLite;
//using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static TransportDepartment.DataBaseInitializer;

namespace TransportDepartment
{
    /// <summary>
    /// Логика взаимодействия для AccountingCard.xaml
    /// </summary>
    public partial class AccountingCard : Window
    {
        private readonly TransportProperties _transport; //

      //  private ObservableCollection<TransportProperties> _transportList = new ObservableCollection<TransportProperties>();

        public AccountingCard(TransportProperties transport)
        {
            InitializeComponent();

            HideCloseButton();
            _transport = transport;
            this.DataContext =  _transport;

            // 2. Загружаем данные из БД
        }

        // Обработчик события Loaded
        private void AccountingCard_Loaded(object sender, RoutedEventArgs e)
        {
            HideCloseButton();

            // Пример выборки по номеру из БД, используя уже переданный номер:
            string stateNumber = _transport.StateNumber;

            //  string  

            // Тут делаешь запрос к БД, используя stateNumber
            var datagridStateNumber = DataBaseInitializer.GetTransportBrandByStateNumber(stateNumber);
            var d = DataBaseInitializer.GetTransportsByRegion(stateNumber);
            // Дальше используешь details для заполнения полей карточки...
        //    MessageBox.Show(datagridStateNumber);


            // Загружаем данные
        }



        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

      

        private void HideCloseButton()
        {
            // Теперь Handle гарантированно существует
            var hwnd = new WindowInteropHelper(this).Handle;

            const int GWL_STYLE = -16;
            const int WS_SYSMENU = 0x80000;

            int currentStyle = GetWindowLong(hwnd, GWL_STYLE);

            // Убираем флаг WS_SYSMENU (системное меню), который отвечает за крестик
            SetWindowLong(hwnd, GWL_STYLE, currentStyle & ~WS_SYSMENU);
        }

        private void onMainWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var newWindow = new MainWindow();
            newWindow.Show();
        }
    }
}
