using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System.Windows.Media.Imaging;
using System.IO;
using System.Text;

namespace VisualBitmap_Demo
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SeriesCollection SeriesCollection { get; set; }
        private List<ChartModel> myList = new List<ChartModel>();

        public Func<double, string> Formatter { get; set; }
        public DateTime InitialDateTime { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            myList.Add(new ChartModel(new DateTime(2019, 9, 23), 2));
            myList.Add(new ChartModel(new DateTime(2019, 12, 14), 0.56));
            myList.Add(new ChartModel(new DateTime(2020, 2, 5), 13.5));
            myList.Add(new ChartModel(new DateTime(2020, 2, 21), 50));
            myList.Add(new ChartModel(new DateTime(2020, 3, 23), 27.9));
            myList.Add(new ChartModel(new DateTime(2020, 3, 29), 42.10));
            myList.Add(new ChartModel(new DateTime(2020, 4, 10), 14));
            myList.Add(new ChartModel(new DateTime(2020, 4, 18), 16.5));
            myList.Add(new ChartModel(new DateTime(2020, 5, 7), 20));

            SetValues_LineChart();

            TextBlock text = new TextBlock();
            text.Text = "hello world";
            
            
            

            this.DataContext = this;
        }



        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //forPrint.PageHeight = 1123.2; // paperSizeA4 Height: 11.7 inches x 96px = 1123.2
                //forPrint.PageWidth = forPrint.ColumnWidth = 796.8; // paperSizeA4 Width: 8.3 inches x 96px = 796.8
                //forPrint.PagePadding = new Thickness(40);
                //DocumentPaginator docPaginator = ((IDocumentPaginatorSource)forPrint).DocumentPaginator;
                //docPaginator.ComputePageCount();

                //this.IsEnabled = false;
                //PrintDialog My_printDialog = new PrintDialog();
                //My_printDialog.PrintDocument(docPaginator, "My PDF print");
                //Image_StackPanel.Children.Add( SaveToPng(Header, @"D:\imag.png"));


                StackPanel stack = new StackPanel();
                stack.Children.Add(SaveToPng(ChartVisual, @"D:\imag.png"));
                stack.Width = 500;
                stack.Height = 500;
                Image_StackPanel.Children.Add(stack);
            }
            finally
            {
                this.IsEnabled = true;
            }
        }

        private void SetValues_LineChart()
        {
            var dayConfig = Mappers.Xy<ChartModel>()
                          .X(dayModel => dayModel.DateTime.Ticks)
                          .Y(dayModel => dayModel.Value);

            SeriesCollection = new SeriesCollection(dayConfig)
            {
                new LineSeries()
                {
                    Values = new ChartValues<ChartModel>(myList)

                }
            };

            InitialDateTime = new DateTime(2019, 8, 1);
            Formatter = value => new DateTime((long)value).ToString("dd/MM/yyyy");
        }




        public Image SaveToPng(FrameworkElement visual, string fileName)
        {
            var encoder = new PngBitmapEncoder();
            return SaveUsingEncoder(visual, fileName, encoder);
        }

        private Image SaveUsingEncoder(FrameworkElement visual, string fileName, BitmapEncoder encoder)
        {
            RenderTargetBitmap bitmap = new RenderTargetBitmap((int)visual.ActualWidth, (int)visual.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            Size visualSize = new Size(visual.ActualWidth, visual.ActualHeight);
            visual.Measure(visualSize);
            visual.Arrange(new Rect(visualSize));
            bitmap.Render(visual);
            BitmapFrame frame = BitmapFrame.Create(bitmap);
            encoder.Frames.Add(frame);

            Image image = new Image();           
            image.Source = encoder.Frames[0];

            return image;
        }






    }





    public class ChartModel
    {
        public DateTime DateTime { get; set; }
        public double Value { get; set; }

        public ChartModel(DateTime dateTime, double value)
        {
            this.DateTime = dateTime;
            this.Value = value;
        }
    }
}
