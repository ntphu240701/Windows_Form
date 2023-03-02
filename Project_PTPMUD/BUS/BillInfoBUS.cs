using Project_PTPMUD.DAO;
using Project_PTPMUD.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_PTPMUD.BUS
{
    internal class BillInfoBUS
    {
        private static BillInfoBUS instance;

        internal static BillInfoBUS Instance
        {
            get { if (instance == null) instance = new BillInfoBUS(); return BillInfoBUS.instance; }
            private set { instance = value; }
        }

        private BillInfoBUS() { }

        public List<BillInfo> GetListBillInfo(int id)
        {
            return BillInfoDAO.Instance.GetListBillInfo(id);
        }

        public void InsertBillInfo(int idBill, int idFood, int quantity)
        {
            DataProvider.Instance.ExecuteNonQuery("USP_InsertBillInfo @idBill , @idFood , @quantity", new object[] { idBill, idFood, quantity });
        }
    }
}
