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
        private static KnapSack _knapSack;
        private static string _workingDirectory = Environment.CurrentDirectory;
        private string _directoryPath = Directory.GetParent(_workingDirectory).Parent.FullName;
        public LoadForm()
        {
            InitializeComponent();
        }

        public LoadForm(string fileName, Service1Client _client,string SafeFileName,RedisClient _redisClient)
        { 
            //upload constructor
            InitializeComponent();
            this.ControlBox = false;
            lblCaption.Text = $@"File {fileName} is Uploading and encrypting!{Environment.NewLine} Please wait";
            this.Text = @"Uploading...";
            progressBar1.Maximum = 20;
            timer1.Interval = 250;
            progressBar1.Style = ProgressBarStyle.Marquee;
            timer1.Start();
            _knapSack=new KnapSack();
            UploadFile(fileName,_client,SafeFileName,_redisClient);

        }
        public LoadForm(string fileName, Service1Client _client, RedisClient _redisClient)
        {
            //download constructor
            InitializeComponent();
            this.ControlBox = false;
            lblCaption.Text = $@"File {fileName} is Uploading and encrypting!{Environment.NewLine} Please wait";
            this.Text = @"Downloading...";
            progressBar1.Maximum = 20;
            timer1.Interval = 250;
            progressBar1.Style = ProgressBarStyle.Marquee;
            timer1.Start();
            _knapSack = new KnapSack();
            DownloadFile(fileName, _client, _redisClient);

        }
        private async void DownloadFile(string fileName, Service1Client _client, RedisClient _redisClient)
        {
            var path = _directoryPath + @"\Download\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var down = _client.Download(fileName, "WickeD");
            var data = _knapSack.Decrypt(down);
            var hash = await Task.Run(() => GetHash(data));
            //var hash = GetHash(data);
            var redisGet = _redisClient.Get<string>(fileName);
            if (hash == redisGet)
                File.WriteAllBytes($"{path}{fileName}", data);
            else
            {
                MessageBox.Show($@"Error while downloading!{Environment.NewLine}Try again!!!", "Download Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            timer1.Stop();
            DialogResult = DialogResult.OK;
            this.Close();
        }
        private async void UploadFile(string FileName, Service1Client _client,string SafeFileName,RedisClient _redisClient)
        {
            var fileinfo = new FileInfo(FileName);
            var file = File.ReadAllBytes(fileinfo.FullName);
            var enc = _knapSack.Encrypt(file);
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
