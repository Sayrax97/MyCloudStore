using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyCloudClient.ServiceReference1;
using CryptoLib.Cryptos;

namespace MyCloudClient
{
    public partial class LoginForm : Form
    {
        Service1Client _client;
        public LoginForm()
        {
            InitializeComponent();
            _client= new Service1Client();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var password = SHA2.Hash(textBox2.Text);
            if(_client.Login(textBox1.Text, password))
            {
                var formMain=new MyCloudStoreClientForm(textBox1.Text);
                formMain.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Wrong username or password!!! Try again!!!","Wrong credentials",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void bntCreateAccount_Click(object sender, EventArgs e)
        {
            var CEform=new CreateAccountForm();
            if (CEform.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("You can login now", "Account Created", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
    }
}
