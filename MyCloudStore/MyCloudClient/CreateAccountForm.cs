using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServiceStack.Redis;
using CryptoLib.Cryptos;

namespace MyCloudClient
{
    public partial class CreateAccountForm : Form
    {
        private static string _host = "localhost:6379";
        private static RedisClient _redisClient = new RedisClient(_host);
        public CreateAccountForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _redisClient.Hashes[txtUsername.Text]["password"] = SHA2.Hash(txtPassword.Text);
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
