using Microsoft.AspNetCore.Mvc;

namespace YazılımOrg15Eylul_ETicaret.ViewComponents;

public class CartSummary : ViewComponent
{
    public IViewComponentResult Invoke()
    {

        var cookie = Request.Cookies["sepetim"];
        int itemCount = cookie != null ? cookie.Split('&').Length : 0;


        return View(itemCount);
    }



}
