using Project_PTPMUD.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_PTPMUD.DAO
{
    internal class MenuDAO
    {
        private static MenuDAO instance;

        internal static MenuDAO Instance
        {
            get { if (instance == null) instance = new MenuDAO(); return MenuDAO.instance; }
            private set { instance = value; }
        }

        private MenuDAO() { }

        public List<Menu> GetListMenuByTable(int id)
        {
            List<Menu> listMenu = new List<Menu>();

            string query = "select f.name, bi.quantity, f.price, f.price*bi.quantity as totalPrice from BillInfo as bi, Bill as b, Food as f where bi.idBill = b.id and bi.idFood = f.id and b.status = 0 and b.idTable = " + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Menu menu = new Menu(item);
                listMenu.Add(menu);
            }

            return listMenu;
        }

        /*public bool UpdateBillInfo(int idBillInfo, int countFood)
        {
            bool data = DataProvider.Instance.ExecuteNonQuery("UPDATE BillInfo SET count= @count where id= @ID ", new object[] { countFood, idBillInfo });
            if (result == true)
                return true;
            return false;
        }*/
    }
}
