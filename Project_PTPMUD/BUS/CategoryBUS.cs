using Project_PTPMUD.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_PTPMUD.BUS
{
    internal class CategoryBUS
    {
        private static CategoryBUS instance;

        public static CategoryBUS Instance
        {
            get { if (instance == null) instance = new CategoryBUS(); return CategoryBUS.instance; }
            private set { CategoryBUS.instance = value; }
        }

        private CategoryBUS() { }

        public List<Category> GetListCategory()
        {
            return CategoryDAO.Instance.GetListCategory();
        }

        public Category GetCategoryByID(int id)
        {
            return CategoryDAO.Instance.GetCategoryByID(id);
        }

        public bool InsertCategory(string name)
        {
            return CategoryDAO.Instance.InsertCategory(name);
        }

        public bool UpdateCategory(int id, string name)
        {
            return CategoryDAO.Instance.UpdateCategory(id, name);
        }

        public bool DeleteCategory(int id)
        {
            return CategoryDAO.Instance.DeleteCategory(id);
        }        
    }
}
