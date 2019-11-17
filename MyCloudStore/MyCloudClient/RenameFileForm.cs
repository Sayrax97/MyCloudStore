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
using MyCloudClient.ServiceReference1;

namespace MyCloudClient
{
    public partial class RenameFileForm : Form
    {
        private Service1Client _client;
        private string _fileName;
        private string _userName;
        public RenameFileForm()
        {
            InitializeComponent();
        }

        public RenameFileForm(string fileName,string userName)
        {
            InitializeComponent();
            _fileName = fileName;
            _userName = userName;
            _client= new Service1Client();
            lblTitle.Text = $@"You are about to rename {fileName}";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var fileinfo=new FileInfo(_fileName);
            _client.RenameFile(_fileName,_userName,textBox1.Text+fileinfo.Extension);
            DialogResult=DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
