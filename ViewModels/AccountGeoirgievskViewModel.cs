using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TransportDepartmentMVVM;
using TransportDepartmentMVVM.Data;
using TransportDepartmentMVVM.Models;
using TransportDepartmentMVVM.Services;
using TransportDepartmentMVVM.ViewModels;
using TransportDepartmentMVVM.Views;

public class AccountGeorgievskViewModel : INotifyPropertyChanged
{
    private readonly TransportRepository _repo;
    private readonly string _regionIndex;
    public Window _mainWindow;

    public string RegionIndex => _regionIndex;     
    public Window MainWindow => _mainWindow;        


    public ObservableCollection<TransportProperties> Transports { get; set; }

   // public ICommand OpenDemonstrationCardCommand { get; }
    public RelayCommand GoToMainCommand { get; }

    public event Action OnCloseRequested;
    public event Action OnHideRequested;
    public event Action OnShowRequested;
    public AccountGeorgievskViewModel(Window mainWindow, string regionIndex)
    {
        _mainWindow = mainWindow;
        _repo = new TransportRepository();
        _regionIndex = regionIndex; // теперь регион доступен в этой VM
        // Команда БЕЗ <T> — просто передаём метод
      //  OpenDemonstrationCardCommand = new RelayCommand(OpenDemonstrationCard);
        GoToMainCommand = new RelayCommand(GoToMain);

        LoadTransports();
    }

    private void LoadTransports()
    {
        try
        {
            DataBaseInitializer.EnsureDataBaseStructure();
            string targetRegion = "Георгиевск";

            var list = _repo.GetTransportsByRegion(targetRegion);
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
