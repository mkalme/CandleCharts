using System;
using System.Drawing;

namespace CandleChart {
    class Render {
        private Bitmap Image { get; set; }
        private Chart Chart { get; set; }

        private Graphics Graphics { get; set; }
        private ChartGraphics ChartSettings { get; set; }

        public Render(int width, int height, Chart chart) {
            Image = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Chart = chart;

            Graphics = Graphics.FromImage(Image);
            ChartSettings = ChartGraphics.Default;
        }

        public Bitmap GetImage() {
            Graphics.Clear(ColorTranslator.FromHtml("#EAEAEA"));

            if (Chart.Sequence.Count == 0) return Image;

            DrawChartBody();
            DrawAmounts();
            DrawDates();

            return Image;
        }

        //Body
        private void DrawChartBody() {
            BodyGraphics body = ChartSettings.Body;

            int width = (int)(body.Width * Image.Width);
            int height = (int)(body.Height * Image.Height);

            Graphics.DrawRectangle(new Pen(Color.Black, 1), new Rectangle(0, 0, width, height));

            int areaHeight = (int)(body.CandleArea * height);

            CandleAreaProfile areaProfile = new CandleAreaProfile();

            areaProfile.Y = (height - areaHeight) / 2;
            areaProfile.Min = Chart.GetLowestWick();
            areaProfile.Max = Chart.GetHighestWick();
            areaProfile.PixelRatio = (areaProfile.Max - areaProfile.Min) / areaHeight;

            int candleWidth = (width - Chart.Sequence.Count * body.Candle.Margin) / Chart.Sequence.Count;
            candleWidth = candleWidth < 2 ? 2 : candleWidth;
            for (int i = 0; i < Chart.Sequence.Count; i++) {
                Candle candle = Chart.Sequence[i];

                int x = i * (body.Candle.Margin + candleWidth) + 13;

                DrawCandle1(candle, x, candleWidth, areaProfile);
            }
        }
        private void DrawCandle(Candle candle, int x, int y, int width, int height) {
            CandleType type = candle.GetCandleType();
            
            Color color = new Color();
            if (type == CandleType.Up) {
                y -= height;

                color = ChartSettings.Body.Candle.UpColor;
            }else if (type == CandleType.Down) {
                color = ChartSettings.Body.Candle.DownColor;
            }

            double candleAmount = Math.Abs(candle.Close - candle.Open);

            

            Graphics.FillRectangle(new SolidBrush(color), new RectangleF(x, y, width, height));
        }
        private void DrawCandle1(Candle candle, int x, int width, CandleAreaProfile profile) {
            CandleType type = candle.GetCandleType();

            float candleTop = type == CandleType.Up ? candle.Close : candle.Open;
            float candleBottom = type == CandleType.Up ? candle.Open : candle.Close;

            int y = profile.GetYFromAmount(candleTop);
            int height = profile.GetYFromAmount(candleBottom) - y;

            Color color = new Color();
            if (type == CandleType.Up) {
                color = ChartSettings.Body.Candle.UpColor;
            } else if (type == CandleType.Down) {
                color = ChartSettings.Body.Candle.DownColor;
            }

            int wickY1 = profile.GetYFromAmount(candle.High);
            int wickY2 = profile.GetYFromAmount(candle.Low);

            Graphics.DrawLine(new Pen(ChartSettings.Body.Candle.WickColor, 2), x + width / 2, wickY1, x + width / 2, wickY2);
            Graphics.FillRectangle(new SolidBrush(color), new RectangleF(x, y, width, height));
        }

        //Amounts
        private void DrawAmounts() { 

        }

        //Dates
        private void DrawDates() { 

        }
    }

    class ChartGraphics {
        public static ChartGraphics Default = new ChartGraphics(BodyGraphics.Default, 20, 10);

        public BodyGraphics Body { get; set; }
        public int AmountPoints { get; set; }
        public int TimePoints { get; set; }

        public ChartGraphics(BodyGraphics body, int amountPoints, int timePoints) {
            Body = body;
            AmountPoints = amountPoints;
            TimePoints = timePoints;
        }
    }
    class BodyGraphics {
        public static BodyGraphics Default = new BodyGraphics(0.92F, 0.92F, 0.8F, CandleGraphics.Default);

        public float Width { get; set; }
        public float Height { get; set; }
        public float CandleArea { get; set; }
        public CandleGraphics Candle { get; set; }

        public BodyGraphics(float width, float height, float candleArea, CandleGraphics candle){
            Width = width;
            Height = height;
            CandleArea = candleArea;
            Candle = candle;
        }
    }
    class CandleGraphics {
        public static CandleGraphics Default = new CandleGraphics(Color.Green, Color.Red, Color.Gray, 4);

        public Color UpColor { get; set; }
        public Color DownColor { get; set; }
        public Color WickColor { get; set; }
        public int Margin { get; set; }

        public CandleGraphics(Color upColor, Color downColor, Color wickColor, int margin) {
            UpColor = upColor;
            DownColor = downColor;
            WickColor = wickColor;
            Margin = margin;
        }
    }

    class CandleAreaProfile {
        public int Y { get; set; }
        public float Min { get; set; }
        public float Max { get; set; }
        public float PixelRatio { get; set; }
        public float Amount {
            get { return Max - Min; }
        }

        public int GetYFromAmount(float amount){
            return (int)((Max - amount) / PixelRatio + Y);
        }
    }
}
