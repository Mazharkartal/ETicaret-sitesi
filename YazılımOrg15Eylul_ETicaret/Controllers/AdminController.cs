using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using YazılımOrg15Eylul_ETicaret.Data;
using YazılımOrg15Eylul_ETicaret.Models;
using YazılımOrg15Eylul_ETicaret.Services;

namespace YazılımOrg15Eylul_ETicaret.Controllers;

public class AdminController : Controller
{
    UserService u = new();
    CategoryService c = new();
    Yazilima15EylulETicaretContext context = new();

    SupplierService s = new();
    StatusService st = new();
    ProductService p = new();


    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([Bind("Email,Password,NameSurname")] User user)
    {

        if (ModelState.IsValid)
        {
            Models.User? usr = await u.loginControl(user);
            if (usr != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Role,"Admin")
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties();

                await HttpContext.SignInAsync(CookieAuthenticationDefaults
                    .AuthenticationScheme
                    , new ClaimsPrincipal(claimsIdentity)
                    , authProperties
                    );

                return RedirectToAction("Index", "Admin");
            }
        }
        else
        {
            ViewBag.error = "Giriş bilgileri hatalı";
        }


        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Admin");
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Index()
    {
        return View();
    }


    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CategoryIndex()
    {
        List<Category> categories = await c.GetCategoriesAsync();

        return View(categories);
    }


    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult CategoryCreate()
    {
        CategoryFill();
        return View();
    }


    void CategoryFill()
    {
        List<Category> categories = c.GetMainCategories();
        ViewData["categoriList"] = categories
            .Select(c =>
            new SelectListItem
            {
                Text = c.CategoryName,
                Value = c.CategoryID.ToString(),
            });
    }

    async Task SupplierFill()
    {
        List<Supplier> suppliers = await s.GetSuppliersAsync();
        ViewData["suppliersList"] = suppliers
            .Select(s =>
            new SelectListItem
            {
                Text = s.BrandName,
                Value = s.SupplierID.ToString(),
            });
    }

    async Task StatusFill()
    {
        List<Status> statuses = await st.GetStatusesAsync();
        ViewData["statusesList"] = statuses
            .Select(st =>
            new SelectListItem
            {
                Text = st.StatusName,
                Value = st.StatusID.ToString(),
            });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult CategoryCreate(Category category)
    {
        bool asnwer = CategoryService.CategoryInsert(category);

        if (asnwer)
        {
            TempData["Message"] = "Kategori Eklendi";
            // return RedirectToAction(nameof(CategoryCreate));
        }
        else
        {
            TempData["Message"] = "HATA!!!  Kategori Eklenemedi";
            // return View();
        }

        //CategoryFill(); 
        //return View();
        return RedirectToAction(nameof(CategoryCreate));
    }


    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CategoryEdit(int? id)
    {
        CategoryFill();

        if (id == null || context.Categories == null)
        {
            return NotFound();
        }

        var category = await c.GetCategoryDetailsAsync(id);

        return View(category);
    }



    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult CategoryEdit(Category category)
    {
        bool asnwer = CategoryService.CategoryUpdate(category);

        if (asnwer)
        {
            TempData["Message"] = "Kategori Güncellendi";
            return RedirectToAction(nameof(CategoryIndex));
        }
        else
        {
            TempData["Message"] = "HATA!!!  Kategori Güncellendi";

        }


        return RedirectToAction(nameof(CategoryEdit));
    }


    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CategoryDetails(int? id)
    {
        var category = await c.GetCategoryDetailsAsync(id);
        ViewBag.category = category?.CategoryName;
        return View(category);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> CategoryDelete(int? id)
    {
        if (id == null || context.Categories == null)
            return NotFound();

        var category = await context.Categories.FirstOrDefaultAsync(c => c.CategoryID == id);


        if (category == null)
            return NotFound();

        return View(category);
    }


    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("CategoryDelete")]
    public async Task<IActionResult> CategoryDeleteConfirmed(int id)
    {
        bool asnwer = CategoryService.CategoryDelete(id);

        if (asnwer)
        {
            TempData["Message"] = "Kategori Silindi";
            return RedirectToAction(nameof(CategoryIndex));
        }
        else
        {
            TempData["Message"] = "HATA!!!  Kategori Silinemedi";

        }


        return RedirectToAction(nameof(CategoryDelete));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SupplierIndex()
    {
        List<Supplier> suppliers = await s.GetSuppliersAsync();
        return View(suppliers);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult SupplierCreate()
    {

        return View();
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult SupplierCreate(Supplier supplier)
    {
        bool asnwer = SupplierService.SupplierInsert(supplier);

        if (asnwer)
        {
            TempData["Message"] = "Marka Eklendi";
        }
        else
        {
            TempData["Message"] = "HATA!!!  Marka Eklenemedi";

        }

        return RedirectToAction(nameof(SupplierCreate));
    }



    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SupplierEdit(int? id)
    {


        if (id == null || context.Suppliers == null)
        {
            return NotFound();
        }

        var supplier = await s.GetSupplierDetailsAsync(id);

        return View(supplier);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult SupplierEdit(Supplier supplier)
    {
        if (supplier.PhotoPath == null)
        {  //fotoğraf değiştirmez ise eski haliyle kaydetsin 
            string? photoPath = context
                .Suppliers
                .FirstOrDefault(s => s.SupplierID == supplier.SupplierID)?
                .PhotoPath;

            supplier.PhotoPath = photoPath;
        }

        bool asnwer = SupplierService.SupplierUpdate(supplier);

        if (asnwer)
            TempData["Message"] = supplier.BrandName?.ToUpper() + "Markası Güncellendi";


        else
            TempData["Message"] = "HATA!!!  Marka Güncellenemedi";

        return RedirectToAction(nameof(SupplierIndex));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SupplierDetails(int? id)
    {

        if (id == null)
        {
            return NotFound();
        }

        var supplier = await s.GetSupplierDetailsAsync(id);
        ViewBag.supplier = supplier?.BrandName;
        return View(supplier);
    }



    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> SupplierDelete(int? id)
    {
        if (id == null || context.Suppliers == null)
            return NotFound();

        var supplier = await context.Suppliers.FirstOrDefaultAsync(s => s.SupplierID == id);


        if (supplier == null)
            return NotFound();

        return View(supplier);
    }


    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("SupplierDelete")]
    public async Task<IActionResult> SupplierDeleteConfirmed(int id)
    {
        bool asnwer = SupplierService.SupplierDelete(id);

        if (asnwer)
        {
            TempData["Message"] = "Marka Silindi";
            return RedirectToAction(nameof(SupplierIndex));
        }
        else
        {
            TempData["Message"] = "HATA!!!  Marka Silinemedi";

        }


        return RedirectToAction(nameof(CategoryDelete));
    }


    //Durum formları
    //listeleme (get)
    //ekleme (get, post)
    //detay (get)
    //sil(get , post)

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> StatusIndex()
    {
        List<Status> statuses = await st.GetStatusesAsync();

        return View(statuses);
    }



    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult StatusCreate()
    {

        return View();
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult StatusCreate(Status status)
    {
        bool asnwer = StatusService.StatusInsert(status);

        if (asnwer)
        {
            TempData["Message"] = "Durum Eklendi";
        }
        else
        {
            TempData["Message"] = "HATA!!!  Durum Eklenemedi";

        }

        return RedirectToAction(nameof(StatusCreate));
    }


    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> StatusEdit(int? id)
    {


        if (id == null || context.Statuses == null)
            return NotFound();


        var status = await st.GetStatusDetailsAsync(id);

        return View(status);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult StatusEdit(Status status)
    {


        bool asnwer = StatusService.StatusUpdate(status);

        if (asnwer)
            TempData["Message"] = status.StatusName?.ToUpper() + "Durumu Güncellendi";


        else
            TempData["Message"] = "HATA!!!  Durumu Güncellenemedi";

        return RedirectToAction(nameof(StatusIndex));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> StatusDetails(int? id)
    {

        if (id == null)
        {
            return NotFound();
        }

        var status = await st.GetStatusDetailsAsync(id);
        ViewBag.status = status?.StatusName;
        return View(status);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> StatusDelete(int? id)
    {
        if (id == null || context.Statuses == null)
            return NotFound();

        var status = await context.Statuses.FirstOrDefaultAsync(s => s.StatusID == id);


        if (status == null)
            return NotFound();

        return View(status);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("StatusDelete")]
    public async Task<IActionResult> StatusDeleteDeleteConfirmed(int id)
    {
        bool asnwer = StatusService.StatusDelete(id);

        if (asnwer)
        {
            TempData["Message"] = "Durum  Silindi";
            return RedirectToAction(nameof(StatusIndex));
        }
        else
        {
            TempData["Message"] = "HATA!!!  Durum Silinemedi";

        }


        return RedirectToAction(nameof(StatusIndex));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ProductIndex()
    {
        List<Product> products = await p.GetProductsAsync();

        return View(products);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> ProductCreate()
    {
        CategoryFill();
        await SupplierFill();
        await StatusFill();

        return View();
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult ProductCreate(Product product)
    {
        if (ModelState.IsValid)
        {
            bool asnwer = ProductService.ProductInsert(product);

            if (asnwer)
                TempData["Message"] = "Ürün Eklendi";


            else
                TempData["Message"] = "HATA!!!  Ürün Eklenemedi";
        }
        else
        {
            TempData["Message"] = "HATA!!!  Zorunlu alanları doldurunuz";
        }


        return RedirectToAction(nameof(ProductCreate));//[HttpGet] e yönlendirir
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> ProductEdit(int? id)
    {
        CategoryFill();
        await SupplierFill();
        await StatusFill();

        if (id == null || context.Products == null)
        {
            return NotFound();
        }

        var product = await p.GetProductDetailsAsync(id);

        return View(product);
    }



    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> ProductEdit(Product product)
    {
        var existProduct = await p.GetProductDetailsAsync(product.ProductID);

        if (product.PhotoPath == null)
            product.PhotoPath = existProduct?.PhotoPath;


        //bana gelmeyen değerler : highlighted, topseller, addDate

        product.HighLighted = existProduct!.HighLighted;
        product.Topseller = existProduct!.Topseller;
        product.AddDate = existProduct!.AddDate;

        bool asnwer = ProductService.ProductUpdate(product);

        if (asnwer)
        {
            TempData["Message"] = "Ürün Güncellendi";
            return RedirectToAction(nameof(ProductIndex));
        }
        else
        {
            TempData["Message"] = "HATA!!!  Ürün Güncellenemedi";

        }


        return RedirectToAction(nameof(ProductEdit)); //[HttpGet ] e geri gönderir
    }




    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> ProductDelete(int? id)
    {
        if (id == null || context.Products == null)
            return NotFound();

        var products = await context.Products.FirstOrDefaultAsync(p => p.ProductID == id);


        if (products == null)
            return NotFound();

        return View(products);
    }


    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("ProductDelete")]
    public async Task<IActionResult> ProductDeleteConfirmed(int id)
    {
        bool asnwer = ProductService.ProductDelete(id);

        if (asnwer)
        {
            TempData["Message"] = "Ürün Silindi";
            return RedirectToAction(nameof(ProductIndex));
        }
        else
        {
            TempData["Message"] = "HATA!!!  Ürün Silinemedi";

        }


        return RedirectToAction(nameof(ProductDelete));
    }

    //ProductDetails

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ProductDetails(int? id)
    {
        if (id == null)
            return NotFound();

        var product = await p.GetProductDetailsAsync(id);
        ViewBag.product = product?.ProductName;
        return View(product);
    }






}
