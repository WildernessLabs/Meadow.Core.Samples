using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AvaloniaSample.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Meadow;
using SkiaSharp;

namespace AvaloniaMeadow.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private SensorService _sensorService;
    
    private ObservableCollection<double> _observableTempValues;
    private ObservableCollection<double> _observableHumValues;
    private int _maxSize = 50;
    
    private static readonly SKColor s_blue = new(25, 118, 210);
    private static readonly SKColor s_red = new(229, 57, 53);

    [ObservableProperty]
    private string _humidity;
    
    [ObservableProperty]
    private string _temperature;
    
    public MainViewModel()
    {
        _observableTempValues = new ObservableCollection<double>();
        _observableHumValues = new ObservableCollection<double>();

        var tempSeries = new LineSeries<double>
        {
            LineSmoothness = 1,
            Name = "Temperature",
            Values = _observableTempValues,
            Stroke = new SolidColorPaint(s_red, 2),
            GeometrySize = 10,
            GeometryStroke = new SolidColorPaint(s_red, 2),
            Fill = null,
            ScalesYAt = 0 // it will be scaled at the Axis[0] instance // mark
        };

        var humSeries = new LineSeries<double>
        {
            Name = "Humidity",
            Values = _observableHumValues,
            Stroke = new SolidColorPaint(s_blue, 2),
            GeometrySize = 10,
            GeometryStroke = new SolidColorPaint(s_blue, 2),
            Fill = null,
            ScalesYAt = 1 // it will be scaled at the YAxes[1] instance // mark
        };

        Series = new ISeries[] { tempSeries, humSeries };
        
        // since Avalonia and Meadow are both starting at the same time, we must wait
        // for MeadowInitialize to complete before the output port is ready
        _ = Task.Run(WaitForSensors);
    }


    private async Task WaitForSensors()
    {
        while (_sensorService == null)
        {
            _sensorService = Resolver.Services.Get<SensorService>();
            await Task.Delay(100);
        }
        
        _sensorService._humiditySensor.HumidityUpdated += (sender, result) =>
        {
            if(_observableHumValues.Count > _maxSize)
                _observableHumValues.RemoveAt(0);
            
            _observableHumValues.Add(result.New.Percent);
            Humidity = $"{result.New.Percent:N1}%";
        };
        
        _sensorService._tempSensor.TemperatureUpdated += (sender, result) =>
        {
            if(_observableTempValues.Count > _maxSize)
                _observableTempValues.RemoveAt(0);
            
            _observableTempValues.Add(result.New.Fahrenheit);
            Temperature = $"{result.New.Fahrenheit:N1}F";
        };
    }
    
    public ISeries[] Series { get; set; }

    public ICartesianAxis[] YAxes { get; set; } =
    {
        new Axis // the "Temperature" series will be scaled on this axis
        {
            Name = "Temperature",
            NameTextSize = 14,
            NamePaint = new SolidColorPaint(s_red),
            NamePadding = new LiveChartsCore.Drawing.Padding(0, 20),
            Padding =  new LiveChartsCore.Drawing.Padding(0, 0, 20, 0),
            TextSize = 12,
            LabelsPaint = new SolidColorPaint(s_red),
            TicksPaint = new SolidColorPaint(s_red),
            SubticksPaint = new SolidColorPaint(s_red),
            DrawTicksPath = true
        },
        new Axis // the "Humidity" series will be scaled on this axis
        {
            Name = "Humidity",
            NameTextSize = 14,
            NamePaint = new SolidColorPaint(s_blue),
            NamePadding = new LiveChartsCore.Drawing.Padding(0, 20),
            Padding =  new LiveChartsCore.Drawing.Padding(20, 0, 0, 0),
            TextSize = 12,
            LabelsPaint = new SolidColorPaint(s_blue),
            TicksPaint = new SolidColorPaint(s_blue),
            SubticksPaint = new SolidColorPaint(s_blue),
            DrawTicksPath = true,
            ShowSeparatorLines = false,
            Position = LiveChartsCore.Measure.AxisPosition.End
        },
    };

    public SolidColorPaint LegendTextPaint { get; set; } =
        new SolidColorPaint
        {
            Color = new SKColor(50, 50, 50),
            SKTypeface = SKTypeface.FromFamilyName("Courier New")
        };

    public SolidColorPaint LedgendBackgroundPaint { get; set; } =
        new SolidColorPaint(new SKColor(240, 240, 240));
}