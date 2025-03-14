using Microsoft.AspNetCore.Mvc;
using YazılımOrg15Eylul_ETicaret.Data;
using YazılımOrg15Eylul_ETicaret.Models;
namespace YazılımOrg15Eylul_ETicaret.ViewComponents;

public class Menus : ViewComponent
{
    Yazilima15EylulETicaretContext context = new();

    public IViewComponentResult Invoke()
    {
        List<Category> categories = context.Categories.Where(c => c.Active).ToList();

        return View(categories);
    }




}
