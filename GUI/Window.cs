using System;
using System.Drawing;
using System.Windows.Forms;
using CandleChart;

namespace GUI {
    public partial class Window : Form {
        private Chart Chart { get; set; }
        private Bitmap Image { get; set; }

        public Window()
        {
            InitializeComponent();
            InitializeChart();
        }
        private void InitializeChart() {
            Chart = Chart.FromYahooFinance(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\candleChart.txt");

            //Chart = new Chart(DateTime.Now, TimeSpan.FromSeconds(30));

            //Chart.Sequence.Add(new Candle(1101, 1097, 1097, 1098));
            //Chart.Sequence.Add(new Candle(1097, 1097.5F, 1097, 1097.5F));
            //Chart.Sequence.Add(new Candle(1097.5F, 1100, 1097.5F, 1100));
            //Chart.Sequence.Add(new Candle(1101, 1100, 1100, 1101));
            //Chart.Sequence.Add(new Candle(1100, 1099, 1099, 1100));
            //Chart.Sequence.Add(new Candle(1099, 1100, 1099, 1100));
            //Chart.Sequence.Add(new Candle(1100, 1098.5F, 1098.5F, 1100));
            //Chart.Sequence.Add(new Candle(1098.5F, 1097, 1097, 1098.5F));
            //Chart.Sequence.Add(new Candle(1097, 1098, 1097, 1098));
        }

        private void Window_Load(object sender, EventArgs e){
            Image = Chart.GetImage(1000, 700);

            DisplayImage();
        }

        private void PictureBox_SizeChanged(object sender, EventArgs e){
            DisplayImage();
        }

        //Scripts
        private void DisplayImage() {
            //PictureBox.Image = ResizeImage();
            PictureBox.Image = Image;
        }
        private Bitmap ResizeImage(){
            int pictureWidth = PictureBox.Width;
            int pictureHeight = PictureBox.Height;

            int width = (int)((Image.Width / (double)Image.Height) * pictureHeight);
            int height = pictureHeight;

            if (width > pictureWidth) {
                width = pictureWidth;
                height = (int)((Image.Height / (double)Image.Width) * width);
            }

            if (width == 0 || height == 0) return Image;

            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result)) {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
                g.DrawImage(Image, 0, 0, width, height);
            }
            return result;
        }
    }
}
