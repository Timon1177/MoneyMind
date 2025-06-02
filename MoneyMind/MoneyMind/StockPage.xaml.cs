using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WPF;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using System.ComponentModel;

namespace MoneyMind
{
    public partial class StockPage : Page, INotifyPropertyChanged
    {
        private readonly System.Timers.Timer _updateTimer;
        private readonly string _apiKey = "18669672a9f448b2b68d537a9a11286f";

        private readonly List<decimal> _priceValues;
        private readonly List<string> _labels;
        private readonly LineSeries<decimal> _lineSeries;

        private ISeries[] _series;
        public ISeries[] Series
        {
            get => _series;
            set { _series = value; OnPropertyChanged(nameof(Series)); }
        }

        private Axis[] _xAxes;
        public Axis[] XAxes
        {
            get => _xAxes;
            set { _xAxes = value; OnPropertyChanged(nameof(XAxes)); }
        }

        private Axis[] _yAxes;
        public Axis[] YAxes
        {
            get => _yAxes;
            set { _yAxes = value; OnPropertyChanged(nameof(YAxes)); }
        }

        public StockPage()
        {
            InitializeComponent();
            DataContext = this;

            _priceValues = new List<decimal>();
            _labels = new List<string>();

            _lineSeries = new LineSeries<decimal>
            {
                Values = _priceValues,
                Fill = null
            };

            Series = new ISeries[] { _lineSeries };

            XAxes = new Axis[]
            {
                new Axis
                {
                    Labels = _labels,
                    LabelsRotation = 45
                }
            };

            YAxes = new Axis[]
            {
                new Axis
                {
                    Labeler = value => value.ToString("C")
                }
            };

            _updateTimer = new System.Timers.Timer(15000); // alle 15 Sekunden
            _updateTimer.Elapsed += async (s, e) => await Dispatcher.InvokeAsync(UpdateChart);

            _ = LoadInitialData(); // sofort starten
        }

        private async System.Threading.Tasks.Task LoadInitialData()
        {
            await UpdateChart();

            // "Loading..."-Text ausblenden (nur wenn vorhanden im XAML)
            LoadingText.Visibility = Visibility.Collapsed;

            _updateTimer.Start();
        }

        private async System.Threading.Tasks.Task UpdateChart()
        {
            try
            {
                using var client = new HttpClient();
                string url = $"https://api.twelvedata.com/time_series?symbol=MSFT&interval=1min&outputsize=1&apikey={_apiKey}";
                string response = await client.GetStringAsync(url);

                var data = JObject.Parse(response);

                if (data["status"]?.ToString() == "error")
                {
                    Console.WriteLine("API Error: " + data["message"]);
                    return;
                }

                var rawValues = data["values"] as JArray;
                if (rawValues == null || rawValues.Count == 0)
                {
                    Console.WriteLine("No data returned.");
                    return;
                }

                var entry = rawValues.First;
                string closeStr = entry["close"]!.ToString();
                string timeStr = entry["datetime"]!.ToString().Substring(11);

                if (decimal.TryParse(closeStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal close))
                {
                    _priceValues.Add(close);
                    _labels.Add(timeStr);

                    if (_priceValues.Count > 30)
                    {
                        _priceValues.RemoveAt(0);
                        _labels.RemoveAt(0);
                    }

                    StockChart.InvalidateVisual();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Chart update failed: " + ex.Message);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
