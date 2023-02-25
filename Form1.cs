using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Telgram_Sticker_Converter
{
    public partial class Form1 : Form
    {
        public string selectedFileName;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Video File (*.mp4)|*.mp4";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                selectedFileName = openFileDialog1.FileName;
                textBox1.AppendText(selectedFileName+" Selected."+ Environment.NewLine);
            }
        }

        private async void button2_ClickAsync(object sender, EventArgs e)
        {
            var output = DateTime.Now.ToFileTime();
            button2.Enabled = false;
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "ffmpeg.exe",
                    Arguments = "-i " + selectedFileName + " -ss 00:00:00 -to 00:00:03 -framerate 30 -c:v libvpx-vp9 -an -vf scale=512:512 -pix_fmt yuva420p " + output + ".webm",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = false
                };
                var proc = Process.Start(startInfo);
                textBox1.AppendText("Proccess Has been started."+ Environment.NewLine);
                textBox1.AppendText("Please Wait ..."+ Environment.NewLine);
                await Task.Run(() =>
                proc.WaitForExit());
                textBox1.AppendText(proc.StandardOutput.ReadToEnd().ToString() + "\n");
                textBox1.AppendText("Done ( checkout " + output + ".webm )"+ Environment.NewLine);
                button2.Enabled = true;
            }
            catch (Exception ee)
            {
                textBox1.AppendText(ee.Message + "\n");
            }
           
        }
    }
}
