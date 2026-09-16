using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TransportDepartmentMVVM;
using TransportDepartmentMVVM.Data;
using TransportDepartmentMVVM.Models;
using TransportDepartmentMVVM.Services;
using TransportDepartmentMVVM.Views;

public class AccountGeorgievskViewModel : INotifyPropertyChanged
{
    private readonly TransportRepository _repo;

    public ObservableCollection<TransportProperties> Transports { get; set; }

    public ICommand OpenAccountingCardCommand { get; }
    public RelayCommand GoToMainCommand { get; }

    public event Action OnCloseRequested;
    public event Action OnHideRequested;
    public event Action OnShowRequested;
    public MainWindow _mainWindow;
    public AccountGeorgievskViewModel(MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
        _repo = new TransportRepository();

        // Команда БЕЗ <T> — просто передаём метод
        OpenAccountingCardCommand = new RelayCommand(OpenAccountingCard);
        GoToMainCommand = new RelayCommand(GoToMain);

        LoadTransports();
    }

    private void LoadTransports()
    {
        try
        {
            DataBaseInitializer.EnsureDataBaseStructure();
            var list = _repo.GetTransportsByRegion("Георгиевск");
            Transports = new ObservableCollection<TransportProperties>(list);
            OnPropertyChanged(nameof(Transports));
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Не удалось загрузить данные: {ex.Message}",
                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    // Метод принимает object — RelayCommand передаст параметр из XAML
    private void OpenAccountingCard(object parameter)
    {
        if (parameter is TransportProperties transport)
        {
            OnHideRequested?.Invoke();

            var card = new AccountingCard(transport);
            card.Closed += (s, e) => OnShowRequested?.Invoke();
            card.Show();
        }
    }

    private void GoToMain()
    {
        var main = new MainWindow();
        main.Show();
        OnCloseRequested?.Invoke();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
