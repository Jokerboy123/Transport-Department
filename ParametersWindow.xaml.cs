using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TransportDepartment
{
    /// <summary>
    /// Логика взаимодействия для ParametersWindow.xaml
    /// </summary>
    public partial class ParametersWindow : Window, INotifyPropertyChanged
    {
        public ParametersWindow()
        {
            InitializeComponent();
            DataContext = this;
            SelectedMonth = Parameters.SelectedMonth;
            SelectedYear = Parameters.SelectedYear;
        }

        public bool IsConfirmed { get; private set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)=>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        private static readonly List<string> list = new List<string> {
    "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь",
    "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"
};

        public string? _selectedMonth;
        public string SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                if (_selectedMonth != value)
                {
                    _selectedMonth = value;
                    OnPropertyChanged();
                }
            }
        }
        public List<string> Months { get; } = list;

        public string? _selectedYear;
        public string SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    OnPropertyChanged();
                }
            }
        }
        public List<string> Years { get; } = list;


        private void onMainWindow_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedMonth != null)
            {
                Parameters.SelectedMonth = SelectedMonth;
                Parameters.SelectedYear = SelectedYear;

            }
            this.Close();



        }

        private void onMainWindow_Click222(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Parameters.SelectedYear);
            MessageBox.Show(Parameters.SelectedMonth);

        }
    }
}
