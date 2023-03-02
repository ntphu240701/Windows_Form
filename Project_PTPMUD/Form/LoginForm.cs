using Project_PTPMUD.BUS;
using Project_PTPMUD.DAO;
using Project_PTPMUD.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Project_PTPMUD
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }        

        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string passWord = txbPassWord.Text;

            if (Login(userName, passWord))
            {
                Account loginAccount = AccountBUS.Instance.GetAccountByUserName(userName);
                MainFormManager mainFormManager = new MainFormManager(loginAccount);

                this.Hide();                
                mainFormManager.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!");
            }
            Clear();
        }

        bool Login(string userName, string passWord)
        {
            return AccountBUS.Instance.Login(userName, passWord);
        }        

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn thoát chương trình? ", "Thông Báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        void Clear()
        {
            txbUserName.Text = txbPassWord.Text = "";
        }
    }
}
