using ElectroKart.Common.Models;
using ElectroKart.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroKart.Service
{
    public class CategoryService
    {
        private readonly CategoryDataAccess _categoryDataAccess;
        public CategoryService(CategoryDataAccess categoryDataAccess)
        {
            _categoryDataAccess = categoryDataAccess;
        }
        public async Task<List<Category>?> GetAllCategories()
        {
            return await _categoryDataAccess.GetAllCategories();
        }
    }
}
