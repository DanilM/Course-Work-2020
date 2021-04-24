using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;


namespace Kyrsovaya
{
  /// <summary>
  /// Логика взаимодействия для MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {

    private string path1;
    public byte[] pixels;
    //public byte[] pixelsRight;
    public BitmapImage Bmp1;

    public byte[] Answer;

    public MainWindow()
    {
      InitializeComponent();
    }

    private void First_Button(object sender, RoutedEventArgs e)
    {
      OpenFileDialog img = new OpenFileDialog();
      
      img.ShowDialog();
      if (img.FileName != "")
      {
        path1 = img.FileName;
        FirstPath.Text = path1;
        Bmp1 = new BitmapImage(new Uri(img.FileName));
        pixels = new byte[Bmp1.PixelWidth* Bmp1.PixelHeight*4];
        Bmp1.CopyPixels(pixels, Bmp1.PixelWidth * 4, 0);
        
        //вывод картинки на экран
        BitmapImage src = new BitmapImage();
        src.BeginInit();
        src.UriSource = new Uri(path1);
        src.EndInit();
        FirstArea.Source = src;
      }
      else
      {
        MessageBox.Show("Файл не выбран");
      }
    }

    private void Save_Button(object sender, RoutedEventArgs e)
    {
      if (Answer != null)
      {
        SaveFileDialog dlg = new SaveFileDialog();
        dlg.FileName = "Image";
        dlg.DefaultExt = ".jpg";
        dlg.Filter = "Text documents (.jpg)|*.jpg";
        bool? result = dlg.ShowDialog();

        if (result == true)
        {
          string filename = dlg.FileName;
          SecondPath.Text = filename;

          BitmapSource image1 = BitmapSource.Create(Bmp1.PixelWidth, Bmp1.PixelHeight, Bmp1.DpiX, Bmp1.DpiY, Bmp1.Format, Bmp1.Palette, Answer, Bmp1.PixelWidth * 4);

          FileStream stream = new FileStream(filename, FileMode.Create);

          SecondArea.Source = image1;
          JpegBitmapEncoder encoder = new JpegBitmapEncoder();
          encoder.Frames.Add(BitmapFrame.Create(image1));
          encoder.Save(stream);
          stream.Close();
        }
      }
      else
      {
        MessageBox.Show("Вы не запустили программу");
      }
    }

    private void Start_Button(object sender, RoutedEventArgs e)
    {
      if (path1 != null)
      {
        Answer = new byte[Bmp1.PixelWidth * Bmp1.PixelHeight * 4];
        for (int i = 0; i < pixels.Length; i += 4)
        {
          Perceptron core = new Perceptron(Convert.ToDouble(pixels[i]) / 255, Convert.ToDouble(pixels[i + 1]) / 255, Convert.ToDouble(pixels[i + 2]) / 255);
          ref double[] r = ref core.Start();
          for (int j = 0; j < 3; j++)
          {
            {
              Answer[i + j] = Convert.ToByte(Math.Round(r[j] * 255));
            }
            Answer[i + 3] = 255;
          }
        }

        BitmapSource image1 = BitmapSource.Create(Bmp1.PixelWidth, Bmp1.PixelHeight, Bmp1.DpiX, Bmp1.DpiY, Bmp1.Format, Bmp1.Palette, Answer, Bmp1.PixelWidth * 4);
        SecondArea.Source = image1;
      }
      else
      {
        MessageBox.Show("Вы не выбрали путь к файлу");
      }
    }
   }
}
  
