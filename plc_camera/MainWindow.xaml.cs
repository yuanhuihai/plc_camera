using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AForge;
using AForge.Video;
using System.Drawing;
using System.Threading;

namespace plc_camera
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        #region Private fields

        private IVideoSource _videoSource;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
      
          
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            string ConnectionString = "http://" + ipInfo.Text + "/axis-cgi/jpg/image.cgi";
            _videoSource = new JPEGStream(ConnectionString);
            _videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
            _videoSource.Start();
        }

        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                BitmapImage bi;
                using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
                {
                    bi = bitmap.ToBitmapImage();
                }
                bi.Freeze(); // avoid cross thread operations and prevents leaks
                Dispatcher.BeginInvoke(new ThreadStart(delegate { videoPlayer.Source = bi; }));
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error on _videoSource_NewFrame:\n" + exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                StopCamera();
            }
        }

        private void StopCamera()
        {
            if (_videoSource != null && _videoSource.IsRunning)
            {
                _videoSource.SignalToStop();
                _videoSource.NewFrame -= new NewFrameEventHandler(video_NewFrame);
            }
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            _videoSource.SignalToStop();
        }
    }
}
