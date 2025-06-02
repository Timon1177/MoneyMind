using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WPF;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;

namespace MoneyMind
{
    public partial class StockPage : Page
    {
        public ISeries[] Series { get; set; }
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }

        private readonly System.Timers.Timer _updateTimer;
        private readonly string _apiKey;

        public StockPage()
        {
            InitializeComponent();
            DataContext = this;

            _apiKey = "18669672a9f448b2b68d537a9a11286f";

            SetupChart();

            _updateTimer = new System.Timers.Timer(10000); // alle 10 Sekunden
            _updateTimer.Elapsed += async (s, e) => await Dispatcher.InvokeAsync(UpdateChart);
            _updateTimer.Start();
        }

        private void SetupChart()
        {
            Series = new ISeries[]
            {
                new LineSeries<decimal>
                {
                    Values = new List<decimal>(),
                    Fill = null
                }
            };

            XAxes = new Axis[]
            {
                new Axis
                {
                    Labels = new List<string>(),
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
        }

        private async System.Threading.Tasks.Task UpdateChart()
        {
            try
            {
                using var client = new HttpClient();
                string url = $"https://api.twelvedata.com/time_series?symbol=NVDA&interval=1min&outputsize=30&apikey={_apiKey}";
                string response = await client.GetStringAsync(url);

                Console.WriteLine("API Response: " + response);

                var data = JObject.Parse(response);

                if (data["status"]?.ToString() == "error")
                {
                    Console.WriteLine("API Error: " + data["message"]);
                    return;
                }

                var values = new List<decimal>();
                var labels = new List<string>();

                var rawValues = data["values"] as JArray;
                if (rawValues == null || rawValues.Count == 0)
                {
                    Console.WriteLine("No data returned.");
                    return;
                }

                foreach (var entry in rawValues)
                {
                    string closeStr = entry["close"]!.ToString();
                    string timeStr = entry["datetime"]!.ToString().Substring(11); // z. B. "14:52:00"

                    if (decimal.TryParse(closeStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal close))
                    {
                        values.Add(close);
                        labels.Add(timeStr);
                    }
                }

                values.Reverse();
                labels.Reverse();

                Series = new ISeries[]
                {
                    new LineSeries<decimal>
                    {
                        Values = values,
                        Fill = null
                    }
                };

                XAxes = new Axis[]
                {
                    new Axis
                    {
                        Labels = labels,
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

                StockChart.Series = Series;
                StockChart.XAxes = XAxes;
                StockChart.YAxes = YAxes;
                StockChart.InvalidateVisual();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Chart update failed: " + ex.Message);
            }
        }
    }
}
