using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectricFox.ConfigManagement;
using ElectricFox.Epaper.Data;
using ElectricFox.Epaper.Rendering;
using ElectricFox.Epaper.Shared;
using ReactiveUI;

namespace ElectricFox.Epaper.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly EpaperConfig _epaperConfig;
    private readonly EpaperDataService _epaperDataService;

    public MainWindowViewModel(
        EpaperConfig epaperConfig,
        EpaperDataService epaperDataService)
    {
        _epaperConfig = epaperConfig;
        _epaperDataService = epaperDataService;
        GetDataCommand = ReactiveCommand.CreateFromTask(GetData);
    }
    
    public RenderState? State { get; set; }
    
    public ReactiveCommand<Unit, Unit> GetDataCommand { get; }

    private async Task GetData()
    {
        var (latitude, longitude) = _epaperConfig.GetCoordinates();

        State = await _epaperDataService.GetRenderStateAsync(
            latitude,
            longitude,
            CancellationToken.None
        );
    }
    
}