using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using SharpAvi;
using SharpAvi.Codecs;
using SharpAvi.Output;
using System.Windows.Forms;
using Captura;

namespace ScreenCapture
{
    public partial class Form1 : Form
    {
        private Recorder rec;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            rec = new Recorder(new RecorderParams("c:\\Temp\\out.avi", 10, SharpAvi.KnownFourCCs.Codecs.MotionJpeg, 70));

        }

        private void button2_Click(object sender, EventArgs e)
        {
            rec.Dispose();
        }
    }
}
