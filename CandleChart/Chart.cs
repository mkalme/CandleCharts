using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;

namespace CandleChart
{
    public class Chart
    {
        public DateTime StartingDate { get; set; }
        public TimeSpan CandleTimeSpan { get; set; }
        public List<Candle> Sequence { get; set; }

        public Chart() {
            StartingDate = new DateTime();
            CandleTimeSpan = new TimeSpan();
            Sequence = new List<Candle>();
        }
        public Chart(DateTime startingDate, TimeSpan candleTimeSpan) {
            StartingDate = startingDate;
            CandleTimeSpan = candleTimeSpan;
            Sequence = new List<Candle>();
        }
        public Chart(DateTime startingDate, TimeSpan candleTimeSpan, List<Candle> sequence)
        {
            StartingDate = startingDate;
            CandleTimeSpan = candleTimeSpan;
            Sequence = sequence;
        }

        public Bitmap GetImage(int width, int height) {
            Render render = new Render(width, height, this);

            return render.GetImage();
        }

        public float GetHighestWick(){
            if (Sequence.Count == 0) return 0;

            return Sequence.OrderBy(p => p.High).Last().High;
        }
        public float GetLowestWick() {
            if (Sequence.Count == 0) return 0;

            return Sequence.OrderBy(p => p.Low).First().Low;
        }

        public static Chart FromYahooFinance(string filePath) {
            Chart chart = new Chart();
            chart.CandleTimeSpan = TimeSpan.FromDays(1);

            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < 50; i++) {
                lines[i] = lines[i].Replace(",", "~");
                lines[i] = lines[i].Replace(".", ",");

                string[] nodes = lines[i].Split('~');

                Candle candle = new Candle(
                    float.Parse(nodes[1]), float.Parse(nodes[4]), float.Parse(nodes[3]), float.Parse(nodes[2])
                );

                chart.Sequence.Add(candle);
            }

            return chart;
        }
    }
}
