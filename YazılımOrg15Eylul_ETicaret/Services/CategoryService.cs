using Microsoft.EntityFrameworkCore;
using YazılımOrg15Eylul_ETicaret.Data;
using YazılımOrg15Eylul_ETicaret.Models;

namespace YazılımOrg15Eylul_ETicaret.Services;

public class CategoryService
{
    Yazilima15EylulETicaretContext context = new();





    //bütün kategorileri liste olarak döner (asenkron)
    public async Task<List<Category>> GetCategoriesAsync()
    {
        List<Category> categories = await context.Categories.ToListAsync();
        return categories;
    }


    //Ana kategorileri liste olarak döner
    public List<Category> GetMainCategories()
    {
        List<Category> categories = context.Categories.Where(c => c.ParentID == 0).ToList();
        return categories;
    }




    public static bool CategoryInsert(Category category)
    {
        //metod static olduğu için context direk gelmez
        using (Yazilima15EylulETicaretContext context = new())
        {
            try
            {
                context.Add(category);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

    }



    public async Task<Category?> GetCategoryDetailsAsync(int? id)
    {
        Category? category = await context
            .Categories
            .FirstOrDefaultAsync(c => c.CategoryID == id);
        return category;

    }


    public static bool CategoryUpdate(Category category)
    {
        //metod static olduğu için context direk gelmez
        using (Yazilima15EylulETicaretContext context = new())
        {
            try
            {
                context.Update(category);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

    }


    public static bool CategoryDelete(int id)
    {
        //metod static olduğu için context direk gelmez
        using (Yazilima15EylulETicaretContext context = new())
        {
            try
            {
                Category? category = context
                    .Categories
                    .FirstOrDefault(c => c.CategoryID == id);


                category!.Active = false;
                List<Category> categories = context // Alt kategorileride deaktif yapmaya yarayan kod
                    .Categories
                    .Where(c => c.ParentID == id)
                    .ToList();
                foreach (var item in categories)
                {
                    item.Active = false;
                }


                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

    }




}
