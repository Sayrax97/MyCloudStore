﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyCloudClient.ServiceReference1;
using CryptoLib.Cryptos;

namespace MyCloudClient
{
    public partial class Form1 : Form
    {
        List<string> _listFiles=new List<string>();
        private Service1Client _client;

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
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var path = @"C:\Users\MICE\Documents\GitHub\MyCloudStore\MyCloudStore\MyCloudClient\UIFiles\";
            
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
        }

        private void GetAllFiles()
        {
            _listFiles.Clear();
            listView1.Clear();
            _listFiles = _client.AllFiles("WickeD").ToList();
            foreach (var item in _listFiles)
            {
                var ext = Path.GetExtension(item);
                listView1.Items.Add(item, _extensions[ext]);
            }
        }
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show(@"You Opened: "+listView1.SelectedItems[0].Text);
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
        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var path = @"C:\Users\MICE\Documents\GitHub\MyCloudStore\MyCloudStore\MyCloudClient\";
            if (!Directory.Exists($"{path}Download"))
                Directory.CreateDirectory($"{path}Download");
            path += @"Download\";

            var down = _client.Download(listView1.SelectedItems[0].Text, "WickeD");
            var data = XXTEA.Decrypt(down);
            File.WriteAllBytes($"{path}{listView1.SelectedItems[0].Text}", data);
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var info=_client.FileInfo(listView1.SelectedItems[0].Text, "WickeD");
            MessageBox.Show(
                $@"File Name: {info.Name} {System.Environment.NewLine} Size: {info.Length}Bytes {Environment.NewLine} Extension: {info.Extension} {Environment.NewLine} Creation time: {info.CreationTime}");
        }

        private void removeStripMenuItem1_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(@"Are you sure you want to delete this item?", @"Warning",
                MessageBoxButtons.OKCancel);
            if(result==DialogResult.OK)
            {
                _client.DeleteFile(listView1.SelectedItems[0].Text, "WickeD");
                GetAllFiles();
            }
        }

        private void renameStripMenuItem1_Click(object sender, EventArgs e)
        {
            var dilog = new RenameFileForm(listView1.SelectedItems[0].Text,"WickeD");
            var result = dilog.ShowDialog();
            if (result == DialogResult.OK)
            {
                GetAllFiles();
            }
        }
    }
}
