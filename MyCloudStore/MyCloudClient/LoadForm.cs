using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CryptoLib.Cryptos;
using MyCloudClient.ServiceReference1;
using ServiceStack;
using ServiceStack.Redis;

namespace MyCloudClient
{
    public partial class LoadForm : Form
    {
        public LoadForm()
        {
            InitializeComponent();
        }

        public LoadForm(string type,string fileName, Service1Client _client,string SafeFileName,RedisClient _redisClient)
        {
            InitializeComponent();
            this.ControlBox = false;
            lblCaption.Text = $@"File {fileName} is {type}ing and encrypting!{Environment.NewLine} Please wait";
            progressBar1.Maximum = 20;
            timer1.Interval = 250;
            progressBar1.Style = ProgressBarStyle.Marquee;
            timer1.Start();
            UploadFile(fileName,_client,SafeFileName,_redisClient);

        }

        private async void UploadFile(string FileName, Service1Client _client,string SafeFileName,RedisClient _redisClient)
        {
            var fileinfo = new FileInfo(FileName);
            var file = File.ReadAllBytes(fileinfo.FullName);
            var enc = XXTEA.Encrypt(file);
            _client.Upload(SafeFileName, enc, "WickeD");
            var hash= await Task.Run(()=>GetHash(file));
            //var hash = GetHash(file);
            _redisClient.Set(SafeFileName, hash);
            timer1.Stop();
            DialogResult = DialogResult.OK;
            this.Close();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value == 20)
                progressBar1.Value = 0;
            else
                progressBar1.Value++;
        }
        private string GetHash(byte[] file)
        {
            return SHA2.Hash(file);
        }
    }
}
