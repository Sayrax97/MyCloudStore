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
        private static ICryptos _crypto;
        private static string _workingDirectory = Environment.CurrentDirectory;
        private string _directoryPath = Directory.GetParent(_workingDirectory).Parent.FullName;
        public LoadForm()
        {
            InitializeComponent();
        }

        public LoadForm(string fileName, Service1Client _client,string SafeFileName,RedisClient _redisClient,string algorithm)
        { 
            //upload constructor
            InitializeComponent();
            this.ControlBox = false;
            lblCaption.Text = $@"File {fileName}{Environment.NewLine} is uploading and encrypting!{Environment.NewLine} Please wait";
            this.Text = @"Uploading...";
            progressBar1.Maximum = 20;
            timer1.Interval = 250;
            progressBar1.Style = ProgressBarStyle.Marquee;
            timer1.Start();
            if(algorithm=="Knapsack")
                _crypto=new KnapSack();
            else if(algorithm== "SimpleSubstitution")
                _crypto=SimpleSub.Instance;
            else if(algorithm=="XXTEA")
                _crypto=XXTEA.Instance;
            UploadFile(fileName, _client, SafeFileName, _redisClient);
            

        }
        public LoadForm(string fileName, Service1Client _client, RedisClient _redisClient, string algorithm)
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
            if (algorithm == "Knapsack")
                _crypto = new KnapSack();
            else if (algorithm == "SimpleSubstitution")
                _crypto = SimpleSub.Instance;
            else if (algorithm == "XXTEA")
                _crypto = XXTEA.Instance;
            DownloadFile(fileName, _client, _redisClient);

        }
        private async void DownloadFile(string fileName, Service1Client _client, RedisClient _redisClient)
        {
            var path = _directoryPath + @"\Download\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var down = _client.Download(fileName, "WickeD");
            var data = _crypto.Decrypt(down);
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
            try
            {
                var cloudSize = _client.StorageLeft("WickeD");
                var fileinfo = new FileInfo(FileName);
                var file = File.ReadAllBytes(fileinfo.FullName);
                if (file.Length > cloudSize)
                {
                    throw  new IOException("File you want to upload exceeds your storage left!!");
                }

                var enc = _crypto.Encrypt(file);
                _client.Upload(SafeFileName, enc, "WickeD");
                var hash = await Task.Run(() => GetHash(file));
                //var hash = GetHash(file);
                _redisClient.Set(SafeFileName, hash);
                timer1.Stop();
                DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (IOException e)
            {
                MessageBox.Show(e.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                Close();
            }
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
