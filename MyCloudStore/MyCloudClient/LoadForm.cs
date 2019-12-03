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
        private byte[] _algByte;
        private const int CHUNK= 1048576;
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
            {
                _crypto = new KnapSack();
                _algByte = Encoding.ASCII.GetBytes("K");
            }
            else if(algorithm== "SimpleSubstitution")
            {
                _crypto = SimpleSub.Instance;
                _algByte = Encoding.ASCII.GetBytes("S");
            }
            else if(algorithm=="XXTEA")
            {
                _crypto = XXTEA.Instance;
                _algByte = Encoding.ASCII.GetBytes("X");
            }
            UploadFile(fileName, _client, SafeFileName, _redisClient);
            

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
            DownloadFile(fileName, _client, _redisClient);

        }
        private async void DownloadFile(string fileName, Service1Client _client, RedisClient _redisClient)
        {
            var fileinfo = _client.FileInfo(fileName, "WickeD");
            var path = _directoryPath + @"\Download\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var nChunks = fileinfo.Length / CHUNK + 1;
            var down = new byte[fileinfo.Length];
            for (int i = 0; i < nChunks; i++)
            {
                var Downdata = _client.DownloadWithChunks(fileinfo.Name, "WickeD", i);
                Buffer.BlockCopy(Downdata, 0, down, i * CHUNK, Downdata.Length);
            }

            //var down = _client.Download(fileName, "WickeD");
            var dataWithoutAlgorithamByte = new byte[down.Length - 1];
            _algByte = new Byte[] {down[down.Length - 1]};
            if (Encoding.ASCII.GetString(_algByte) == "X")
            {
                _crypto = XXTEA.Instance;
            }
            else if(Encoding.ASCII.GetString(_algByte) == "K")
            {
                _crypto= new KnapSack();
            }
            else if(Encoding.ASCII.GetString(_algByte) == "S")
            {
                _crypto= SimpleSub.Instance;
            }
            Buffer.BlockCopy(down,0,dataWithoutAlgorithamByte,0,down.Length-1);
            var data = _crypto.Decrypt(dataWithoutAlgorithamByte);
            var hash = await Task.Run(() => GetHash(dataWithoutAlgorithamByte));
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
                //var file = new byte[stream.Length];
                var enc = _crypto.Encrypt(file);
                var encWithAlgorithm = Append(enc, _algByte);
                for (int i = 0,j=0; i < encWithAlgorithm.LongLength; i+=CHUNK,j++)
                {
                    var razlika = encWithAlgorithm.Length - i;
                    var chunk = new byte[razlika < CHUNK ? razlika : CHUNK];
                    Buffer.BlockCopy(encWithAlgorithm,i,chunk,0, razlika < CHUNK ? (int)razlika : CHUNK);
                    _client.UploadWithChunks(fileinfo.Name, chunk, "WickeD", j, encWithAlgorithm.LongLength, i + CHUNK >= encWithAlgorithm.Length);
                }
                //_client.Upload(SafeFileName, encWithAlgorithm, "WickeD");
                var hash = await Task.Run(() => GetHash(enc));
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
        public static byte[] Append(byte[] current, byte[] after)
        {
            var bytes = new byte[current.Length + after.Length];
            current.CopyTo(bytes, 0);
            after.CopyTo(bytes, current.Length);
            return bytes;
        }
    }
}
