using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PagedList.Core;
using System.Collections.Specialized;
using System.Text;
using YazılımOrg15Eylul_ETicaret.Data;
using YazılımOrg15Eylul_ETicaret.Hubs;
using YazılımOrg15Eylul_ETicaret.Models;
using YazılımOrg15Eylul_ETicaret.Services;
using YazılımOrg15Eylul_ETicaret.ViewModels;

namespace YazılımOrg15Eylul_ETicaret.Controllers;

public class HomeController : Controller
{

    /*
     **** 1 = slider ürünler
     **** 2 = özel ürün
     **** 3 = yıldızlı
     **** 4 = fırsat
     **** 5 = dikkat çeken
     **** 6 = günün ürünü
     * 
     *** addDate     = yeni ürünler
     * discount      = indirimli ürünler
     * highlighted   = öne çıkanlar
     * topseller     = en çok satanlar 
     */
    Yazilima15EylulETicaretContext context = new();
    MainPageModel mpm = new();
    ProductService cp = new();
    OrderService o = new();
    CategoryService c = new();
    SupplierService s = new();
    IHubContext<AdminHub> hubContext;

    public HomeController(IHubContext<AdminHub> hubContext)
    {
        this.hubContext = hubContext;
    }

    public string GetClientIp()
    {
        var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
        return remoteIpAddress != null ? remoteIpAddress.ToString() : "IP Adresi Bulunamadı";
    }


    public async Task<IActionResult> Index()
    {
        await hubContext.Clients.All.SendAsync("KullaniciHareketiniYakala", "Ziyaretçi", "Siteye girdi");


        mpm.SliderProducts = cp.GetProducts("SliderProducts", "index");            //slider
        mpm.NewProducts = cp.GetProducts("NewProducts", "index");                   //yeni 
        mpm.ProductsOfDay = cp.GetProductOfDay();                                  //günün ürünü
        mpm.SpecialProducts = cp.GetProducts("SpecialProducts", "index");          //özel ürün
        mpm.StarProducts = cp.GetProducts("StarProducts", "index");                //yıldız
        mpm.FeaturedProducts = cp.GetProducts("FeaturedProducts", "index");        //fırsat
        mpm.DiscountedProducts = cp.GetProducts("DiscountedProducts", "index");    //indirim
        mpm.HighlightedProducts = cp.GetProducts("HighlightedProducts", "index");  //öne çıkan
        mpm.TopsellerProducts = cp.GetProducts("TopsellerProducts", "index");      //çok satanlar
        mpm.NotableProducts = cp.GetProducts("NotableProducts", "index");          // dikkat çeken

        string ipAddress = GetClientIp();

        if (HttpContext.Session.GetString("Email") != null)
        {

            User? user = UserService.GetUserInfo(HttpContext.Session.GetString("Email")!);
            await hubContext
                .Clients.All
                .SendAsync("KullaniciHareketiniYakala", $"{DateTime.UtcNow} {ipAddress} ipli {user.NameSurname}", "ana sayfasına girdi");

        }
        else
        {
            User? user = UserService.GetUserInfo(HttpContext.Session.GetString("Email")!);
            await hubContext
                .Clients.All
                .SendAsync("KullaniciHareketiniYakala", $"{DateTime.UtcNow} {ipAddress} ipli kullanıcı", "ana sayfasına girdi");

        }


        return View(mpm);
    }




    public async Task<IActionResult> NewProducts()
    {
        string ipAddress = GetClientIp();

        if(HttpContext.Session.GetString("Email") != null)
        {

            User? user = UserService.GetUserInfo(HttpContext.Session.GetString("Email")!);
            await hubContext
                .Clients.All
                .SendAsync("KullaniciHareketiniYakala", $"{DateTime.UtcNow} {ipAddress} ipli {user.NameSurname}", "Yeni Ürünler sayfasına girdi");

        }
        else
        {
            User? user = UserService.GetUserInfo(HttpContext.Session.GetString("Email")!);
            await hubContext
                .Clients.All
                .SendAsync("KullaniciHareketiniYakala", $"{DateTime.UtcNow} {ipAddress} ipli kullanıcı", "Yeni Ürünler sayfasına girdi");

        }



        mpm.NewProducts = cp.GetProducts("NewProducts", "topmenu");                //Alt sayfa, menüden yeri ürüne tıklayınca açılacak 
        return View(mpm);
    }



    public PartialViewResult _PartialNewProducts(string nextpagenumber)
    {
        //nextpagenumber * 4 = kaçıncı üründen başlayacak skip
        int pagenumber = Convert.ToInt32(nextpagenumber);
        mpm.NewProducts = cp.GetProducts("NewProducts", "topmenuajax", pagenumber);     //alt sayfa            //Alt sayfa, Daha fazla butonuna tıklayınca açılacak 

        return PartialView(mpm);
    }



    public async Task<ActionResult> SpecialProducts()
    {
        string ipAddress = GetClientIp();

        if (HttpContext.Session.GetString("Email") != null)
        {

            User? user = UserService.GetUserInfo(HttpContext.Session.GetString("Email")!);
            await hubContext
                .Clients.All
                .SendAsync("KullaniciHareketiniYakala", $"{DateTime.UtcNow} {ipAddress} ipli {user.NameSurname}", "Özel sayfasına girdi");

        }
        else
        {
            User? user = UserService.GetUserInfo(HttpContext.Session.GetString("Email")!);
            await hubContext
                .Clients.All
                .SendAsync("KullaniciHareketiniYakala", $"{DateTime.UtcNow} {ipAddress} ipli kullanıcı", "Özel sayfasına girdi");

        }
        mpm.SpecialProducts = cp.GetProducts("SpecialProducts", "topmenu");                //Alt sayfa, menüden Özel ürüne tıklayınca açılacak 
        return View(mpm);
    }



    public PartialViewResult _PartialSpecialProducts(string nextpagenumber)
    {
        //nextpagenumber * 4 = kaçıncı üründen başlayacak skip
        int pagenumber = Convert.ToInt32(nextpagenumber);
        mpm.SpecialProducts = cp.GetProducts("SpecialProducts", "topmenuajax", pagenumber);     //alt sayfa            //Alt sayfa, Daha fazla butonuna tıklayınca açılacak 

        return PartialView(mpm);
    }



    public async Task<IActionResult> DiscountedProducts()
    {
        string ipAddress = GetClientIp();

        if (HttpContext.Session.GetString("Email") != null)
        {

            User? user = UserService.GetUserInfo(HttpContext.Session.GetString("Email")!);
            await hubContext
                .Clients.All
                .SendAsync("KullaniciHareketiniYakala", $"{DateTime.UtcNow} {ipAddress} ipli {user.NameSurname}", "ana sayfasına girdi");

        }
        else
        {
            User? user = UserService.GetUserInfo(HttpContext.Session.GetString("Email")!);
            await hubContext
                .Clients.All
                .SendAsync("KullaniciHareketiniYakala", $"{DateTime.UtcNow} {ipAddress} ipli kullanıcı", "ana sayfasına girdi");

        }
        mpm.DiscountedProducts = cp.GetProducts("DiscountedProducts", "topmenu");                //Alt sayfa, menüden indirimli ürüne tıklayınca açılacak 
        return View(mpm);
    }



    public PartialViewResult _PartialDiscountedProducts(string nextpagenumber)
    {
        //nextpagenumber * 4 = kaçıncı üründen başlayacak skip
        int pagenumber = Convert.ToInt32(nextpagenumber);
        mpm.DiscountedProducts = cp.GetProducts("DiscountedProducts", "topmenuajax", pagenumber);     //alt sayfa            //Alt sayfa, Daha fazla butonuna tıklayınca açılacak 

        return PartialView(mpm);
    }



    public async Task<IActionResult> HighlightedProducts()
    {
        string ipAddress = GetClientIp();

        if (HttpContext.Session.GetString("Email") != null)
        {

            User? user = UserService.GetUserInfo(HttpContext.Session.GetString("Email")!);
            await hubContext
                .Clients.All
                .SendAsync("KullaniciHareketiniYakala", $"{DateTime.UtcNow} {ipAddress} ipli {user.NameSurname}", "Highlited sayfasına girdi");

        }
        else
        {
            User? user = UserService.GetUserInfo(HttpContext.Session.GetString("Email")!);
            await hubContext
                .Clients.All
                .SendAsync("KullaniciHareketiniYakala", $"{DateTime.UtcNow} {ipAddress} ipli kullanıcı", "Highlited sayfasına girdi");

        }
        mpm.HighlightedProducts = cp.GetProducts("HighlightedProducts", "topmenu");                //Alt sayfa, menüden indirimli ürüne tıklayınca açılacak 
        return View(mpm);
    }



    public PartialViewResult _PartialHighlightedProducts(string nextpagenumber)
    {
        //nextpagenumber * 4 = kaçıncı üründen başlayacak skip
        int pagenumber = Convert.ToInt32(nextpagenumber);
        mpm.HighlightedProducts = cp.GetProducts("HighlightedProducts", "topmenuajax", pagenumber);     //alt sayfa            //Alt sayfa, Daha fazla butonuna tıklayınca açılacak 

        return PartialView(mpm);
    }


    public IActionResult TabsellerProducts(int page = 1, int pageSize = 16)
    {
        PagedList<Product> model = new PagedList<Product>(

            context.Products.Where(p => p.Active)
            .OrderByDescending(p => p.Topseller)
            , page
            , pageSize

            );

        return View(model);

    }




    public IActionResult CategoryPage(int id)
    {
        // mpm.ProductsByCategory =  cp.GetProductsByCategoryId (id);
        List<Product> products = cp.GetProductsByCategoryId(id);
        var category = context.Categories.Find(id);
        if (category != null)
            ViewBag.Category = category.CategoryName;




        return View(products);
    }



    public IActionResult SupplierPage(int id)
    {
        List<Product> products = cp.GetProductsBySupplierId(id);
        return View(products);
    }





    //projenın her hangi bir sayfasında sepete ekle buttonu tıklanınca çalışacak
    public IActionResult CartProcess(int id)
    {

        cp.HighlightedPlus(id);

        o.ProductID = id;
        o.Quantity = 1;


        var cookiOptions = new CookieOptions(); // nesne oluşturduk, instance aldık
        //çerez politikası devreye girecek = cookiede  = tarayıcıda tutulur ( google,chrome,mozilla, forşfox,edge,safari )


        var cookie = Request.Cookies["sepetim"];//  Tarayıcıda sepetim isiminde cookie (çerrez) varmı

        if (cookie == null)
        {
            //kullanıcı sisteme ilk defa ürün sepete eklediğinde çalışacak
            cookiOptions = new CookieOptions();
            cookiOptions.Expires = DateTime.Now.AddDays(1); //bir günlük çerez tanımladık
            cookiOptions.Path = "/";
            o.Sepet = "";
            o.AddToCart(id.ToString());  // sepete ekle metodu yazacaz string bekliyoruz
            Response.Cookies.Append("sepetim", o.Sepet, cookiOptions); //tanımladığımız çerezi tarayıcıya gönderdik
                                                                       //  HttpContext.Session.SetString("Message", "Ürün Sepetinize Eklendi");
            TempData["Message"] = "Ürün Sepete Eklendi";
        }
        else
        {
            //kullanıcı daha önceden sepetine ürün eklediyse çalışacak. Tarayıcıdaki sepetim içeriğini property e gönderiyoruz
            o.Sepet = cookie;
            //aynı ürün daha önce sepete eklenmiş mi kontrolü yapar
            if (o.AddToCart(id.ToString()) == false)
            {
                Response.Cookies.Append("sepetim", o.Sepet, cookiOptions); //eklenmemişse bu satır çalışacak ve ürünü ekler 
                cookiOptions.Expires = DateTime.Now.AddDays(1); //bir günlük çerez tanımladık
                                                                //   HttpContext.Session.SetString("Message", "Ürün Sepetinize Eklendi");
                TempData["Message"] = "Ürün Sepete Eklendi";
            }
            else
            {
                //  HttpContext.Session.SetString("Message", "Ürün Sepetinize Daha önce Eklendi");
                TempData["Message"] = "Ürün Sepete Daha önce Eklendi";
            }
        }

        string url = Request.Headers["Referer"].ToString();



        return Redirect(url); // ürün sepete eklendikten sonra hangi sayfada ise o sayfayaya tekrar yönlendiriyor

    }

    public async Task<IActionResult> Details(int id)
    {
        //efcore
        //  mpm.ProductDetails = context.Products.FirstOrDefault(p=> p.ProductID == id);

        // select * from Products where ProductID = id /_ BU ado.net

        //LINQ - id`si 4 olan bütün kolon (sütün) bilgilerini getir
        mpm.ProductDetails = (from p
                              in context.Products
                              where p.ProductID == id
                              select p)
                              .FirstOrDefault();

        //LINQ
        mpm.CategoryName = (from p in context.Products
                            join c in context.Categories
                            on p.CategoryID equals c.CategoryID
                            where p.ProductID == id
                            select c.CategoryName).FirstOrDefault();
        //LINQ
        mpm.BrandName = (from p in context.Products
                         join s in context.Suppliers
                         on p.SupplierID equals s.SupplierID
                         where p.ProductID == id
                         select s.BrandName).FirstOrDefault();

        // select* from products where Related = x and ProductID != id
        mpm.RelatedProducts = context
            .Products
            .Where(p => p.Related == mpm.ProductDetails!.Related &&
             p.ProductID != id
            )
            .ToList();

        mpm.Comments = context
            .Comments
            .Where(c => c.ProductID == id)
            .OrderByDescending(c => c.AddDate)
            .ToList();


        ViewBag.Categories = await c.GetCategoriesAsync();
        ViewBag.Suppliers = await s.GetSuppliersAsync();



        cp.HighlightedPlus(id);
        return View(mpm);
    }


    //Projenin sağ üst köşesinden sepet butonuna tıklanınca açılacak
    //sepetten ürün silerken sil buttonuna tıkalyıncadda açılacak
    public IActionResult Cart()
    {   //blabla.com/sil?id=5
        if (HttpContext.Request.Query["scid"].ToString() != "")
        {
            //sayfada ürün silerken sil butonuna tıklanınca scid Göndereceğiz
            //Sepetim isimli cookie(çerez)simnden ürün silerek Cart.cshtml gidecek
            int scid = Convert.ToInt32(HttpContext.Request.Query["scid"].ToString());
            o.Sepet = Request.Cookies["sepetim"];// tarayıcıdan aldık propertiye koyduk
            o.DeleteFromMyCart(scid.ToString());

            var cookiOptions = new CookieOptions();
            //kullanıcı sisteme ilk defa ürün sepete eklediğinde çalışacak
            cookiOptions = new CookieOptions();
            cookiOptions.Expires = DateTime.Now.AddDays(1); //bir günlük çerez tanımladık
            Response.Cookies.Append("sepetim", o.Sepet, cookiOptions); //tanımladığımız çerezi tarayıcıya gönderdik
            TempData["Message"] = "Ürün Sepetten Silindi";

            //Cart.cshtml ürünleri foreach ile dönüp gösterecek viewbag ile o bilgileri hazırlıyoruz

            List<OrderService> sepet = o.GetMyCart();
            ViewBag.Sepet = sepet;
            ViewBag.sepet_tablo_detay = sepet;

            if (sepet.Count == 0)
            {
                ViewBag.bossepet = "boş";
            }


        }
        else
        {
            //Projenin sağ üst köşesinden sepet butonuna tıklanınca açılacak
            //Sepetim isimli cookie(çerez)sinden hiçbir şey değiştirmeden Cart.cshtml gidecek
            var cookie = Request.Cookies["sepetim"];
            List<OrderService> sepet;
            var cookieOptions = new CookieOptions();

            if (cookie == null)
            {

                o.Sepet = "";
                sepet = o.GetMyCart();
                ViewBag.Sepet = sepet;
                ViewBag.sepet_tablo_detay = sepet;
                if (sepet.Count == 0)
                    ViewBag.bossepet = "boş";



            }
            else
            {
                o.Sepet = Request.Cookies["sepetim"];
                sepet = o.GetMyCart();
                ViewBag.Sepet = sepet;
                ViewBag.sepet_tablo_detay = sepet;
                if (sepet.Count == 0)
                    ViewBag.bossepet = "boş";
            }




        }










        return View();
    }

    [HttpGet]
    public IActionResult Order()
    {

        //kullanıcı giriş yapmışmı
        if (HttpContext.Session.GetString("Email") != null)
        {
            //Bu kullanıcı giriş yapmiş ve benden Email isminden bir oturum almış
            Models.User? user = UserService.GetUserInfo(HttpContext.Session.GetString("Email"));

            return View(user);


        }
        else
        {

            return RedirectToAction("Login");
        }

    }

    [HttpPost]
    public IActionResult Order(Order order, IFormCollection frm)
    {
        if (frm["txt_tckimlikno"] != "")
        {
            string? txt_tckimlikno = Request.Form["txt_tckimlikno"];
        }
        if (frm["txt_vergino"] != "")
        {
            string? txt_tckimlikno = Request.Form["txt_vergino"];
        }

        string? kredikartno = Request.Form["kredikartno"];
        string? kredikartay = Request.Form["kredikartay"];
        string? kredikartyil = Request.Form["kredikartyil"];
        string? kredikartcvv = Request.Form["kredikartcvv"];
        /*
        //payu, paytr, paratika, iyzico

        //api
        //gizlilik sözleşmesi iade koşulları, mesafeli satış sözleşmesi, iletişim adres telefon mail

        NameValueCollection data = new NameValueCollection();
        string payu_url = "https://www.eticaret.com/backref";
        data.Add("BACK_REF", payu_url);
        data.Add("CC_CVV",kredikartcvv);
        data.Add("CC_NUMBER",kredikartno);
        data.Add("CC_MONTH",kredikartay);
        data.Add("CC_YEAR",kredikartyil);


        var deger = "";
        foreach (var item in data)
        {
            var value = item as string;
            var byteCount = Encoding.UTF8.GetByteCount(data.Get(value));
            deger += byteCount + data.Get(value);

        }


        var signatureKey = "size verilen SECRET_KEY buraya yazılacak";
        var hash = HashWithSignature(deger, signatureKey);
        data.Add("ORDER_HASH", hash);
        var x = POSTFormPayu("https://secure.payu.com.tr/order/...0",data);


        if (x.Contains("<STATUS>SUCCES</STATUS>") && x.Contains("<RETURN_CODE>3DS_ENROLLED</RETURN_CODE>"))
        {
            //SANAL KART İLE ALIŞVERİŞİNİ YAPTI ÖDEMEDE SIKINTI YOK
        }
        else 
        {
            //GERÇEK KART

            if (x.Contains("<STATUS>SUCCES</STATUS>") && x.Contains("<RETURN_CODE>AUTOHORIZED</RETURN_CODE>"))
            {
                //gerçek kart ile alışveriş yaptı
            }


        }
        */


        return RedirectToAction("backref");
    }

    public string POSTFormPayu(string url, NameValueCollection data)
    {
        return "";
    }

    public string HashWithSignature(string deger, string signatureKey)
    {
        return "";
    }

    public IActionResult backref()
    {
        OrderConfirm();
        return RedirectToAction("Confirm");
    }

    public static string OrderGroupGUID = "";//20250227223230

    public static string cevap = "";
    public IActionResult OrderConfirm()
    {

        //cookie spstindeki siparişi Orders tablosuna işleyecez


        var cookiOptions = new CookieOptions();
        var cookie = Request.Cookies["sepetim"];//  Tarayıcıda sepetim isiminde cookie (çerrez) varmı

        if (cookie != null)
        {
            o.Sepet = cookie;
            OrderGroupGUID = o.WriteFromCookieToTable(HttpContext.Session.GetString("Email").ToString());
            cookiOptions.Expires = DateTime.Now.AddDays(1); //bir günlük çerez tanımladık
            Response.Cookies.Delete("sepetim");


            cevap = UserService.Send_Sms(OrderGroupGUID);
            UserService.Send_Email(OrderGroupGUID);


        }


        return RedirectToAction("Confirm");
    }

    public IActionResult Confirm()
    {

        ViewBag.OrderGroupGUID = OrderGroupGUID;
        ViewBag.cevap = cevap; /// netgsmden dönen cevap 
        return View();
    }


    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(User user)
    {
        string answer = UserService.AddUser(user);

        if (answer == "Başarılı")
        {
            TempData["Message"] = "Bilgileriniz Başarıyla Kaydedilde";
            return RedirectToAction("Login");
        }
        else if (answer == "Email zaten var")
        {
            TempData["Message"] = "Email daha önce kayıtlı. Tekrar deneyin";
            return View();
        }

        TempData["Message"] = "Giriş Hatalı ";
        return View();
    }



    public IActionResult Login()
    {
        string url = Request.Headers["Referer"].ToString();
        HttpContext.Session.SetString("url", url);
        return View();
    }


    [HttpPost]
    public IActionResult Login(User user)
    {
        string answer = UserService.UserControl(user);

        if (answer == "yanlış")
        {
            //email ve şifre kayıtlı değil
            TempData["Message"] = "Hata.. Email veya Şifre yanlış. Tekrar deneyin";
            return View();
        }
        else if (answer == "admin")
        {
            HttpContext.Session.SetString("Admin", answer);
            return RedirectToAction("Login", "Admin");

        }
        else if (answer == "HATA")
        {
            TempData["Message"] = "Bir HATA Oluştu. Tekrar deneyin";
            return View();

        }
        else
        {
            HttpContext.Session.SetString("Email", answer);

            if (HttpContext.Session.GetString("url") != null)
            {
                string url = HttpContext.Session.GetString("url")!;
                HttpContext.Session.Remove("url");
                return Redirect(url);

            }


            return RedirectToAction("Index", "Home");
        }

    }


    public IActionResult Logout()
    {

        HttpContext.Session.Remove("Email");
        HttpContext.Session.Remove("Admin");
        HttpContext.Session.Remove("url");
        return RedirectToAction("Login");

    }


    [HttpPost]
    public IActionResult AddToCart(int id)
    {
        string message = "";

        cp.HighlightedPlus(id);

        o.ProductID = id;
        o.Quantity = 1;


        var cookiOptions = new CookieOptions(); // nesne oluşturduk, instance aldık
        //çerez politikası devreye girecek = cookiede  = tarayıcıda tutulur ( google,chrome,mozilla, forşfox,edge,safari )


        var cookie = Request.Cookies["sepetim"];//  Tarayıcıda sepetim isiminde cookie (çerrez) varmı

        if (cookie == null)
        {
            //kullanıcı sisteme ilk defa ürün sepete eklediğinde çalışacak
            cookiOptions = new CookieOptions();
            cookiOptions.Expires = DateTime.Now.AddDays(1); //bir günlük çerez tanımladık
            cookiOptions.Path = "/";
            o.Sepet = "";
            o.AddToCart(id.ToString());  // sepete ekle metodu yazacaz string bekliyoruz
            Response.Cookies.Append("sepetim", o.Sepet, cookiOptions); //tanımladığımız çerezi tarayıcıya gönderdik
            message = "Ürün Sepete Eklendi";                                                         //  HttpContext.Session.SetString("Message", "Ürün Sepetinize Eklendi");
            //TempData["Message"] = "Ürün Sepete Eklendi";
        }
        else
        {
            //kullanıcı daha önceden sepetine ürün eklediyse çalışacak. Tarayıcıdaki sepetim içeriğini property e gönderiyoruz
            o.Sepet = cookie;
            //aynı ürün daha önce sepete eklenmiş mi kontrolü yapar
            if (o.AddToCart(id.ToString()) == false)
            {
                Response.Cookies.Append("sepetim", o.Sepet, cookiOptions); //eklenmemişse bu satır çalışacak ve ürünü ekler 
                cookiOptions.Expires = DateTime.Now.AddDays(1); //bir günlük çerez tanımladık
                message = "Ürün Sepete Eklendi";                                                  //   HttpContext.Session.SetString("Message", "Ürün Sepetinize Eklendi");
                //TempData["Message"] = "Ürün Sepete Eklendi";
            }
            else
            {
                //  HttpContext.Session.SetString("Message", "Ürün Sepetinize Daha önce Eklendi");
                // TempData["Message"] = "Ürün Sepete Daha önce Eklendi";
                message = "Ürün Sepetinizde Zaten Var";
            }
        }


        // cookie = Request.Cookies["sepetim"];
        //int itemCount = cookie != null ? cookie.Split('&').Length : 0;



        return Json(new { success = true, message = message });

    }



    public IActionResult CartSummary()
    {
        return ViewComponent("CartSummary");
    }



    public IActionResult MyOrders()
    {
        if (HttpContext.Session.GetString("Email") != null)
        {
            List<MyOrdersViewModel> orders = o.SelectMyOrders(HttpContext.Session.GetString("Email").ToString());
            return View(orders);
        }


        return RedirectToAction("Login");
    }





    public async Task<IActionResult> DetailedSearch()
    {
        ViewBag.Categories = await c.GetCategoriesAsync();
        ViewBag.Suppliers = await s.GetSuppliersAsync();

        return View();

    }




    public IActionResult DpProducts(int CategoryID, string[] SupplierID, string price, string isInStock)
    {
        price = price.Replace(" ", "").Replace("TL", "");
        string[] priceArray = price.Split("-");
        string startmoney = priceArray[0];
        string endmoney = priceArray[1];



        string SupplierValue = "";

        for (int i = 0; i < SupplierID.Length; i++)
        {
            if (i != 0)
                SupplierValue += " or ";
            SupplierValue += "SupplierID = " + SupplierID[i];

        }

        if (SupplierValue != "")
            SupplierValue = $"({SupplierValue}) and";



        string query = $"Select * from Products where (CategoryID ={CategoryID}) and {SupplierValue} (UnitPrice >= {startmoney} and UnitPrice <= {endmoney}) and Stock >= {isInStock} Order By AddDate Desc";

        ViewBag.Products = o.Select_Products_DetailsSearch(query);


        return View();
    }


    public PartialViewResult gettingProducts(string id)
    {
        id = id.ToUpper(new System.Globalization.CultureInfo("tr-TR"));
        List<QuickSearchViewModel> ulist = ProductService.gettingSearchProducts(id);
        //string json = JsonConvert.SerializeObject(ulist);
        //var response = JsonConvert.DeserializeObject<List<Search>>(json);



        return PartialView(ulist);

    }

    public IActionResult Abone(string email)
    {
        TopluEmail topluEmail = new TopluEmail();

        topluEmail.Email = email;
        topluEmail.Active = true;

        context.TopluEmails.Add(topluEmail);

        context.SaveChanges();

        return RedirectToAction("Index");
    }


    public IActionResult TopluMailGonder()
    {
        List<TopluEmail> mailler = context.TopluEmails.Where(x => x.Active).ToList();

        foreach (var mail in mailler)
        {
            UserService.TopluMailGonder(mail.Email);
        }

        return RedirectToAction("Index");
    }


    public IActionResult YorumYap(string userMail, int productId, string review, int point)
    {
        User? user = context.Users.FirstOrDefault(x => x.Email == userMail);

        Comment comment = new()
        {
            AddDate = DateTime.Now,
            ProductID = productId,
            Review = review,
            UserID = user.UserID,
            Puan = point
        };

        context.Comments.Add(comment);

        Product? product = context.Products.FirstOrDefault(x => x.ProductID == productId);

        product.OySayisi++;
        product.ToplamPuan += point;
        product.OrtlamaPuan = Convert.ToDecimal(product.ToplamPuan) / Convert.ToDecimal(product.OySayisi);
        context.SaveChanges();

        TempData["Message"] = "Yorumunuz eklendi";
        string url = Request.Headers["Referer"].ToString();
        return Redirect(url);

    }











}
