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
    internal class TableBUS
    {
        private static TableBUS instance;

        internal static TableBUS Instance
        {
            get { if (instance == null) instance = new TableBUS(); return TableBUS.instance; }
            private set { TableBUS.instance = value; }
        }

        private TableBUS() { }

        public List<Table> LoadTableList()
        {
            return TableDAO.Instance.LoadTableList();
        }

        public List<Table> GetTableStatusByTableID(string status)
        {
            return TableDAO.Instance.GetTableStatusByTableID(status);
        }

        public bool InsertTable(string name)
        {
            return TableDAO.Instance.InsertTable(name);
        }

        public bool DeleteTable(int id)
        {
            return TableDAO.Instance.DeleteTable(id);
        }

        public bool UpdateTable(int id, string name)
        {
            return TableDAO.Instance.UpdateTable(id, name);
        }
    }
}
