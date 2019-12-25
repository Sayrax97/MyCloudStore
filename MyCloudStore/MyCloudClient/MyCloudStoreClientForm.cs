using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyCloudClient.ServiceReference1;
using CryptoLib.Cryptos;
using ServiceStack.Redis;

namespace MyCloudClient
{
    public partial class MyCloudStoreClientForm : Form
    {
        private List<string> _listFiles=new List<string>();
        private Service1Client _client;
        private string _userName;
        private static Dictionary<string, int> _extensions = new Dictionary<string, int>
        {
            {".png",0},
            {".jpg",1},
            {".txt",2},
            {".doc",3},
            {".pdf",4},
            {".wav",5},
            {".mp3",6},
            {".xml",7},
            {".json",8},
            {".zip",9},
            {".mp4",10},
            {".avi",11},
            {".xls",12}
        };

        private static string _workingDirectory = Environment.CurrentDirectory;
        private string _directoryPath = Directory.GetParent(_workingDirectory).Parent.FullName;
        public MyCloudStoreClientForm()
        {
            InitializeComponent();
        }
        public MyCloudStoreClientForm(string userName)
        {
            InitializeComponent();
            _userName = userName;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            var path = _directoryPath + @"\UIFiles\";
            imageList1.Images.Add(new Bitmap($"{path}png.png"));
            imageList1.Images.Add(new Bitmap($"{path}jpg.png"));
            imageList1.Images.Add(new Bitmap($"{path}txt.png"));
            imageList1.Images.Add(new Bitmap($"{path}doc.png"));
            imageList1.Images.Add(new Bitmap($"{path}pdf.png"));
            imageList1.Images.Add(new Bitmap($"{path}wav.png"));
            imageList1.Images.Add(new Bitmap($"{path}mp3.png"));
            imageList1.Images.Add(new Bitmap($"{path}xml.png"));
            imageList1.Images.Add(new Bitmap($"{path}json.png"));
            imageList1.Images.Add(new Bitmap($"{path}zip.png"));
            imageList1.Images.Add(new Bitmap($"{path}mp4.png"));
            imageList1.Images.Add(new Bitmap($"{path}avi.png"));
            imageList1.Images.Add(new Bitmap($"{path}xls.png"));
            _client= new Service1Client();
            GetAllFiles();
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            lblFileName.Visible = false;
            lblExt.Visible = false;
            lblSize.Visible = false;
            lblTime.Visible = false;
        }
        private void GetAllFiles()
        {
            _listFiles.Clear();
            listView1.Clear();
            _listFiles = _client.AllFiles(_userName).ToList();
            foreach (var item in _listFiles)
            {
                var ext = Path.GetExtension(item);
                listView1.Items.Add(item, _extensions[ext]);
            }

            var left = _client.StorageLeft(_userName);
            left /= 1024;
            left /= 1024;
            lblStorageLeft.Text =  Math.Round(left,2)+ @"MB remaining";
        }
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            var path = _directoryPath + @"\Download\" + listView1.SelectedItems[0].Text;
            if (File.Exists(path))
            {
                Process.Start(path);
            }
            else
            {
                MessageBox.Show($@"File is not in your download folder.{Environment.NewLine}Please try to download it again", "File not found", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listView1.FocusedItem.Bounds.Contains(e.Location))
                {
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }

            if (listView1.SelectedItems.Count>0)
            {
                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                lblFileName.Visible=true;
                lblExt.Visible = true;
                lblSize.Visible = true;
                lblTime.Visible = true;
            }
            var info = _client.FileInfo(listView1.SelectedItems[0].Text, _userName);
            double length = info.Length;
            var unit = "B";
            if (length > 1024)
            {
                length /= 1024;
                unit = "KB";
            }
            if (length > 1024)
            {
                length /= 1024;
                unit = "MB";
            }
            if (length > 1024)
            {
                length /= 1024;
                unit = "GB";
            }

            lblFileName.Text = Path.GetFileNameWithoutExtension(info.Name);
            lblExt.Text = info.Extension;
            lblSize.Text = $@"{Math.Round(length,2)}{unit}";
            lblTime.Text = info.CreationTime.ToLongTimeString();
            
        }
        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (LoadForm lf = new LoadForm( listView1.SelectedItems[0].Text, _client,_userName))
            {
                lf.ShowDialog();
            }
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var info=_client.FileInfo(listView1.SelectedItems[0].Text, _userName);
            var length = info.Length;
            var unit = "B";
            if (length > 1024)
            {
                length /= 1024;
                unit = "KB";
            }
            if (length > 1024)
            {
                length /= 1024;
                unit = "MB";
            }
            if (length > 1024)
            {
                length /= 1024;
                unit = "GB";
            }
            MessageBox.Show(
                $@"File Name: {info.Name} {System.Environment.NewLine} Size: {length}{unit} {Environment.NewLine} Extension: {info.Extension} {Environment.NewLine} Creation time: {info.CreationTime}");
        }

        private void removeStripMenuItem1_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(@"Are you sure you want to delete this item?", @"Warning",
                MessageBoxButtons.OKCancel);
            if(result==DialogResult.OK)
            {
                _client.DeleteFile(listView1.SelectedItems[0].Text, _userName);
                GetAllFiles();
            }
        }

        private void renameStripMenuItem1_Click(object sender, EventArgs e)
        {
            var dilog = new RenameFileForm(listView1.SelectedItems[0].Text, _userName);
            var result = dilog.ShowDialog();
            if (result == DialogResult.OK)
            {
                GetAllFiles();
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show(@"You need to select crypto algorithm before uploading", @"Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = _directoryPath,
                Title = "Select File to Upload",

                CheckFileExists = true,
                CheckPathExists = true,

                Filter = "txt files (*.txt)|*.txt|Image (*.jpg)|*.jpg|Image (*.png)|*.png|Video (*.mp4)|*.mp4|Video (*.avi)|*.avi|Audio (*.wav)|*.wav|Audio (*.mp3)|*.mp3|Document (*.doc)|*.doc|Document (*.pdf)|*.pdf|Document (*.xml)|*.xml|Document (*.json)|*.json|ZIP (*.zip)|*.zip|Excel (*.xls)|*.xls",
                FilterIndex = 1,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    using (LoadForm lf = new LoadForm(ofd.FileName, _client, ofd.SafeFileName, comboBox1.Text,_userName))
                    {
                        try
                        {
                            lf.ShowDialog();
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception);
                        }
                    }
                    GetAllFiles();
                }
                ofd.Reset();
            }

            

            
        }

        private string GetHash(byte[] file)
        {
            return SHA2.Hash(file);
        }

        private void comboBox1_TextUpdate(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox1.Text == "SimpleSubstitution")
            {
                lblAlgDesc.Text = $@"This algorithm is: {Environment.NewLine} fast {Environment.NewLine} weak {Environment.NewLine} file size is same";
            }
            else if (comboBox1.Text == "Knapsack")
            {
                lblAlgDesc.Text = $@"This algorithm is: {Environment.NewLine} fast {Environment.NewLine} strong {Environment.NewLine} file size is 3-5times bigger";
            }
            else if (comboBox1.Text == "XXTEA")
            {
                lblAlgDesc.Text = $@"This algorithm is: {Environment.NewLine} normal {Environment.NewLine} strong {Environment.NewLine} file size is same {Environment.NewLine} shouldn't be used with bigger files";
            }
        }
    }
}
