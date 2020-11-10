using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Video;

namespace PLC_CAMEAR_WinForm
{
    public partial class Form1 : Form
    {

        private IVideoSource videoSource;
        public Form1()
        {
            InitializeComponent();

        }


        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = bitmap;
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            string ConnectionString = "http://" + cameraIp.Text + "/axis-cgi/jpg/image.cgi";
            videoSource = new JPEGStream(ConnectionString);
            videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
            videoSource.Start();
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            videoSource.SignalToStop();
            if (videoSource != null && videoSource.IsRunning && pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
            }
        }
    }
}
