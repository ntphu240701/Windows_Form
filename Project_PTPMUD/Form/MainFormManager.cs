using Project_PTPMUD.BUS;
using Project_PTPMUD.DAO;
using Project_PTPMUD.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Menu = Project_PTPMUD.DTO.Menu;

namespace Project_PTPMUD
{
    public partial class MainFormManager : Form
    {
        private Account loginAccount;

        internal Account LoginAccount 
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount.Type); }
        }

        public MainFormManager(Account acc)
        {
            InitializeComponent();

            this.LoginAccount = acc;
            
            LoadTable();
            LoadCategory();
        }      

        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinTàiKhoảnToolStripMenuItem.Text += " (" + LoginAccount.DisplayName + ")";
        }

        void LoadCategory()
        {
            List<Category> listCategory = CategoryBUS.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }
        
        void LoadFoodListCategoryID(int id)
        {
            List<Food> listFood = FoodBUS.Instance.GetFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }

        void LoadTable()
        {
            flpTable.Controls.Clear(); 

            List<Table> tableList = TableBUS.Instance.LoadTableList();

            foreach (Table item in tableList)
            {
                Button btn = new Button() {  Width = TableDAO.TableWidth, Height = TableDAO.TableHeight};
                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Click += Btn_Click; 
                btn.Tag = item;

                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.Green; break;
                        default: btn.BackColor = Color.Red; break;
                }

                flpTable.Controls.Add(btn);                
            }
        }

        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;

            foreach(Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Quantity.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
            }
            CultureInfo culture = new CultureInfo("vi-VN");
            txb_TongTien.Text = totalPrice.ToString("c", culture);
        }        

        private void Btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
            
            string tableName = ((sender as Button).Tag as Table).Name;
            this.txb_CurrentTable.Text = tableName.ToString();
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AccountProfileForm accountProfileForm = new AccountProfileForm(LoginAccount);
            accountProfileForm.UpdateAccount += accountProfileForm_UpdateAccount;
            accountProfileForm.ShowDialog();
        }

        private void accountProfileForm_UpdateAccount(object sender, AccountProfileForm.AccountEvent e)
        {
            thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản (" + e.Acc.DisplayName + ")";
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdminForm adminForm = new AdminForm();
            adminForm.loginAccount = LoginAccount;
            adminForm.InsertFood += adminForm_InsertFood;
            adminForm.DeleteFood += adminForm_DeleteFood;
            adminForm.UpdateFood += adminForm_UpdateFood;
            adminForm.ShowDialog();
        }

        private void adminForm_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadTable();
        }

        private void adminForm_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadTable();
        }

        private void adminForm_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            Category selected = cb.SelectedItem as Category;

            id = selected.ID;

            LoadFoodListCategoryID(id);
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            if (table == null)
            {
                MessageBox.Show("Hãy chọn bàn");
                return;
            }

            int idBill = BillBUS.Instance.GetUncheckedBillByTableID(table.ID);
            int foodID = (cbFood.SelectedItem as Food).ID;
            int quantity = (int)nmFoodCount.Value;

            if(idBill == -1)
            {
                BillBUS.Instance.InsertBill(table.ID);
                BillInfoBUS.Instance.InsertBillInfo(BillBUS.Instance.GetMaxIDBill(), foodID, quantity);
            }
            else
            {
                BillInfoBUS.Instance.InsertBillInfo(idBill, foodID, quantity);
            }

            ShowBill(table.ID);

            LoadTable();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            int idBill = BillBUS.Instance.GetUncheckedBillByTableID(table.ID);
            int discount = (int)nmDiscount.Value;

            double totalPrice = Convert.ToDouble(txb_TongTien.Text.Split(',')[0].Replace(".",""));
            double tienKhach = Convert.ToDouble(txb_TienKhach.Text);
            double finalPrice = totalPrice - (totalPrice / 100) * discount;
            double tienThoi = tienKhach - finalPrice;

            if (idBill != -1)
            {
                if ( MessageBox.Show(string.Format("Thanh toán hóa đơn cho bàn {0} \n\n Tiền khách đưa: {1} \n\n Giảm giá: {3} % \n\n Tổng tiền: {4} \n\n Tiền thối: {5}", table.Name, tienKhach, totalPrice, discount, finalPrice, tienThoi), "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    BillBUS.Instance.CheckOut(idBill, discount, (float)finalPrice);

                    ShowBill(table.ID);

                    LoadTable();
                }
            }                
        }
    }
}
