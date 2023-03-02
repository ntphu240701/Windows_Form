using Project_PTPMUD.BUS;
using Project_PTPMUD.DAO;
using Project_PTPMUD.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_PTPMUD
{
    public partial class AccountProfileForm : Form
    {
        private Account loginAccount;

        internal Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount); }
        }

        public AccountProfileForm(Account acc)
        {
            InitializeComponent();

            LoginAccount = acc;
        }

        void ChangeAccount(Account acc)
        {
            txbUserName.Text = LoginAccount.UserName;
            txbDisplayName.Text = LoginAccount.DisplayName;
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccountInfo();
        }

        void UpdateAccountInfo()
        {
            string displayName = txbDisplayName.Text;
            string password = txbPassWord.Text;
            string newpass = txbNewPassWord.Text;
            string reenterPass = txbReEnterPassWord.Text;
            string userName = txbUserName.Text;

            byte[] temp = ASCIIEncoding.ASCII.GetBytes(password);
            byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);

            string hasPass = "";

            foreach (byte item in hasData)
            {
                hasPass += item;
            }

            byte[] temp1 = ASCIIEncoding.ASCII.GetBytes(newpass);
            byte[] hasData1 = new MD5CryptoServiceProvider().ComputeHash(temp1);

            string hasNewPass = "";

            foreach (byte item in hasData1)
            {
                hasNewPass += item;
            }

            byte[] temp2 = ASCIIEncoding.ASCII.GetBytes(reenterPass);
            byte[] hasData2 = new MD5CryptoServiceProvider().ComputeHash(temp2);

            string hasreenterPass = "";

            foreach (byte item in hasData2)
            {
                hasreenterPass += item;
            }

            if (!hasNewPass.Equals(hasreenterPass))
            {
                MessageBox.Show("Vui lòng nhập lại mật khẩu đúng với mật khẩu mới!");
            }
            else
            {
                if (AccountBUS.Instance.UpdateAccount(userName, displayName, hasPass, hasNewPass))
                {
                    MessageBox.Show("Cập nhật thành công");
                    if (updateAccount != null)
                        updateAccount(this, new AccountEvent(AccountBUS.Instance.GetAccountByUserName(userName)));
                }
                else
                {
                    MessageBox.Show("Vui lòng điền đúng mật khấu");
                }
            }
        }

        private event EventHandler<AccountEvent> updateAccount;
        public event EventHandler<AccountEvent> UpdateAccount
        {
            add { updateAccount += value; }
            remove { updateAccount -= value; }
        }

        public class AccountEvent : EventArgs
        {
            private Account acc;

            public Account Acc
            {
                get { return acc; }
                set { acc = value; }
            }

            public AccountEvent(Account acc)
            {
                this.Acc = acc;
            }
        }
    }
}
