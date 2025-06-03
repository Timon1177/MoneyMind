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

        private List<decimal> _priceValues;
        private List<string> _labels;

        private bool _isRunning = false;
        private bool _isUpdating = false;
        private string? _lastTimestamp;
        private decimal? _lastPrice;

        public ISeries[] Series { get; set; }
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }

        public StockPage()
        {
            InitializeComponent();
            DataContext = this;

            _priceValues = new List<decimal>();
            _labels = new List<string>();

            Series = new ISeries[]
            {
                new LineSeries<decimal>
                {
                    Values = _priceValues,
                    Fill = null
                }
            };

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

            _updateTimer = new System.Timers.Timer(12000);
            _updateTimer.Elapsed += async (s, e) => await Dispatcher.InvokeAsync(UpdateChart);
        }

        public async void StartChartUpdates()
        {
            if (_isRunning) return;
            _isRunning = true;

            await UpdateChart();
            LoadingText.Visibility = Visibility.Collapsed;
            _updateTimer.Start();
        }

        public async System.Threading.Tasks.Task UpdateChart()
        {
            if (_isUpdating) return;
            _isUpdating = true;

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
                    if (_lastPrice != null && _lastTimestamp != null)
                    {
                        _priceValues.Add((decimal)_lastPrice);
                        _labels.Add(DateTime.Now.ToString("HH:mm:ss"));

                        if (_priceValues.Count > 30)
                        {
                            _priceValues.RemoveAt(0);
                            _labels.RemoveAt(0);
                        }

                        Series = new ISeries[]
                        {
                            new LineSeries<decimal>
                            {
                                Values = _priceValues,
                                Fill = null
                            }
                        };

                        XAxes = new Axis[]
                        {
                            new Axis
                            {
                                Labels = _labels,
                                LabelsRotation = 45
                            }
                        };

                        MarketStatusText.Text = "Market is closed – showing last known price.";
                        MarketStatusText.Visibility = Visibility.Visible;
                        OnPropertyChanged(nameof(Series));
                        OnPropertyChanged(nameof(XAxes));
                    }
                    return;
                }

                var entry = rawValues.First;
                string closeStr = entry["close"]!.ToString();
                string timeStr = entry["datetime"]!.ToString();

                if (decimal.TryParse(closeStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal close))
                {
                    if (_lastTimestamp == timeStr && _lastPrice == close)
                    {
                        Console.WriteLine("No change, skipping update.");
                        return;
                    }

                    _lastTimestamp = timeStr;
                    _lastPrice = close;

                    string timeOnly = DateTime.Parse(timeStr).ToString("HH:mm:ss");

                    var newPrices = new List<decimal>(_priceValues) { close };
                    var newLabels = new List<string>(_labels) { timeOnly };

                    if (newPrices.Count > 30)
                    {
                        newPrices.RemoveAt(0);
                        newLabels.RemoveAt(0);
                    }

                    _priceValues = newPrices;
                    _labels = newLabels;

                    Series = new ISeries[]
                    {
                        new LineSeries<decimal>
                        {
                            Values = _priceValues,
                            Fill = null
                        }
                    };

                    XAxes = new Axis[]
                    {
                        new Axis
                        {
                            Labels = _labels,
                            LabelsRotation = 45
                        }
                    };

                    if (DateTime.TryParse(timeStr, out var serverTime))
                    {
                        if ((DateTime.UtcNow - serverTime.ToUniversalTime()) > TimeSpan.FromMinutes(10))
                        {
                            MarketStatusText.Text = "Market is closed – showing last known price.";
                            MarketStatusText.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            MarketStatusText.Visibility = Visibility.Collapsed;
                        }
                    }

                    OnPropertyChanged(nameof(Series));
                    OnPropertyChanged(nameof(XAxes));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Chart update failed: " + ex.Message);
            }
            finally
            {
                _isUpdating = false;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
