using System;
using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetAllCategories();
        void CreateCategory(Category category);
        void Delete(int id);
        Category GetCategoryById(int id);
        void Update(Category category);
    }
}