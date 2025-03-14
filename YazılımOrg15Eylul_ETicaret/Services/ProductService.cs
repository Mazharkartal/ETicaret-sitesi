using Microsoft.EntityFrameworkCore;
using YazılımOrg15Eylul_ETicaret.Data;
using YazılımOrg15Eylul_ETicaret.Models;
using YazılımOrg15Eylul_ETicaret.ViewModels;

namespace YazılımOrg15Eylul_ETicaret.Services;

public class ProductService
{

    Yazilima15EylulETicaretContext context = new();




    public async Task<List<Product>> GetProductsAsync()
    {
        List<Product> products = await context.Products.ToListAsync();
        return products;
    }



    public static bool ProductInsert(Product product)
    {
        //metod static olduğu için context direk gelmez
        using (Yazilima15EylulETicaretContext context = new())
        {
            try
            {
                // product.ProductName = product.ProductName!.ToLower().Trim();


                if (product.Notes == null)
                    product.Notes = "";


                product.AddDate = DateTime.Now;

                bool exists =
                    context
                    .Products
                    .Any(p => p.ProductName!.ToLower().Trim()
                    .Equals(product.ProductName!
                    .ToLower().Trim())
                    );

                if (!exists)
                {
                    context.Add(product);
                    context.SaveChanges();
                    return true;
                }

                return false;


            }
            catch (Exception)
            {

                return false;
            }
        }

    }


    public async Task<Product?> GetProductDetailsAsync(int? id)
    {
        Product? product = await context
            .Products
            .FirstOrDefaultAsync(p => p.ProductID == id);
        return product;

    }

    public static bool ProductUpdate(Product product)
    {
        //metod static olduğu için context direk gelmez
        using (Yazilima15EylulETicaretContext context = new())
        {
            try
            {
                context.Update(product);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

    }


    public static bool ProductDelete(int id)
    {
        //metod static olduğu için context direk gelmez
        using (Yazilima15EylulETicaretContext context = new())
        {
            try
            {
                Product? product = context
                    .Products
                    .FirstOrDefault(p => p.ProductID == id);


                product!.Active = false;


                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

    }


    public List<Product> GetProducts(string mainPageName, string subPageName, int pagenumber = 0)
    {
        List<Product> products;

        int? MainPageCount = context.Settings.FirstOrDefault(s => s.SettingID ==1 )?.MainpageCount;
        int? SubPageCount = context.Settings.FirstOrDefault(s => s.SettingID ==1 )?.SubpageCount;

        

        if (mainPageName == "SliderProducts")
        {

            //slider ürünleri getiren sorgu burada yer alacak 
            products = context.Products.Where(p => p.StatusID == 1 && p.Active).Take(Convert.ToInt32(MainPageCount)).ToList();
        }
        else if (mainPageName == "NewProducts")
        {
            // Yeni
            if (subPageName == "index")
            {
                //home/index
                products = context.Products.Where(p => p.Active).OrderByDescending(p => p.AddDate).Take(Convert.ToInt32(MainPageCount)).ToList();

            }
            else
            {
                //Home/NewProducts
                if (subPageName == "topmenu")
                {
                    //menüden new tıklanınca 
                    products = context.Products.Where(p => p.Active).OrderByDescending(p => p.AddDate).Take(Convert.ToInt32(SubPageCount)).ToList();

                }
                else
                {    //ajax daha fazla butonuna tıklanınca çalışır
                    products = context.Products.Where(p => p.Active).OrderByDescending(p => p.AddDate).Skip(pagenumber * Convert.ToInt32(SubPageCount)).Take(Convert.ToInt32(SubPageCount)).ToList();

                }

            }

        }
        else if (mainPageName == "SpecialProducts")
        {
            // özel
            if (subPageName == "index")
            {
                //home/index
                products = context.Products.Where(p => p.StatusID == 2 && p.Active).OrderByDescending(o => o.AddDate).Take(Convert.ToInt32(MainPageCount)).ToList();

            }
            else
            {
                //Home/SpecialProducts
                if (subPageName == "topmenu")
                {
                    //menüden özel tıklanınca 
                    products = context.Products.Where(p => p.StatusID == 2 && p.Active).OrderByDescending(o => o.AddDate).Take(Convert.ToInt32(SubPageCount)).ToList();

                }
                else  
                {    //ajax daha fazla butonuna tıklanınca çalışır
                    products = context.Products.Where(p => p.StatusID == 2 && p.Active).OrderByDescending(o => o.AddDate).Skip(pagenumber * Convert.ToInt32(SubPageCount)).Take(Convert.ToInt32(SubPageCount)).ToList();

                }

            }

        }
        else if (mainPageName == "DiscountedProducts")
        {
            // İndirimli
            if (subPageName == "index")
            {
                //home/index
                products = context.Products.Where(p => p.Active)
                        .OrderByDescending(p => p.Discount)
                        .Take(Convert.ToInt32(MainPageCount)).ToList();
            }
            else
            {
                //Home/DiscountedProducts
                if (subPageName == "topmenu")
                {
                    //menüden indirimli tıklanınca 
                    products = context.Products.Where(p => p.Active)
                        .OrderByDescending(p => p.Discount)
                        .Take(Convert.ToInt32(SubPageCount)).ToList();

                }
                else
                {    //ajax daha fazla butonuna tıklanınca çalışır
                    products = context.Products.Where(p => p.Active)
                        .OrderByDescending(p => p.Discount)
                        .Skip(pagenumber * Convert.ToInt32(SubPageCount))
                        .Take(Convert.ToInt32(SubPageCount)).ToList();
                }

            }

        }




        else if (mainPageName == "HighlightedProducts")
        {
            // öne çıkanlar 
            if (subPageName == "index")
            {
                //home/index
                products = context.Products.Where(p => p.Active)
                    .OrderByDescending(p => p.HighLighted)
                    .Take(Convert.ToInt32(MainPageCount)).ToList();

            }
            else
            {
                //Home/HighlightedProducts
                if (subPageName == "topmenu")
                {
                    //menüden öne çıkanlar  tıklanınca 
                    products = context.Products.Where(p => p.Active)
                        .OrderByDescending(p => p.HighLighted)
                        .Take(Convert.ToInt32(SubPageCount)).ToList();

                }
                else
                {    //ajax daha fazla butonuna tıklanınca çalışır
                    products = context.Products.Where(p => p.Active)
                        .OrderByDescending(p => p.HighLighted)
                        .Skip(pagenumber * (Convert.ToInt32(SubPageCount)))
                        .Take(Convert.ToInt32(SubPageCount)).ToList();
                }

            }

        }





        else if (mainPageName == "StarProducts")
        {
            // Yıldız 
            products = context.Products.Where(p => p.StatusID == 3 && p.Active).OrderByDescending(o => o.AddDate).Take(Convert.ToInt32(MainPageCount)).ToList();

        }
        else if (mainPageName == "FeaturedProducts")
        {
            // Fırsat 
            products = context.Products.Where(p => p.StatusID == 4 && p.Active).OrderByDescending(o => o.AddDate).Take(Convert.ToInt32(MainPageCount)).ToList();

        }


        else if (mainPageName == "TopsellerProducts")
        {
            // Çok satanlar
            products = context.Products.Where(p => p.Active).OrderByDescending(p => p.Topseller).Take(Convert.ToInt32(MainPageCount)).ToList();

        }
        else if (mainPageName == "NotableProducts")
        {
            // Çok satanlar
            products = context.Products.Where(p => p.StatusID == 5 && p.Active).OrderByDescending(o => o.AddDate).Take(Convert.ToInt32(MainPageCount)).ToList();

        }
        else
        {
            return null!;
        }
        return products;
    }



    public Product? GetProductOfDay()
    {


        //Günün ürününü getiren sorgu burada yer alacak 
        Product? product = context.Products.FirstOrDefault(p => p.StatusID == 6);


        return product;
    }


    public List<Product> GetProductsByCategoryId(int id)
    {
        List<Product> products = context.Products.Where(p => p.CategoryID == id && p.Active)
            .OrderByDescending(o => o.AddDate)
            .ToList();
        return products;
    }

    public List<Product> GetProductsBySupplierId(int id)
    {
        List<Product> products = context.Products.Where(p => p.SupplierID == id && p.Active)
            .OrderByDescending(o => o.AddDate)
            .ToList();
        return products;
    }


    public void HighlightedPlus(int id)
    {
        //1.Product Tablosundan ürünü bul
        //2.Highlighted kolonundaki sayıyı 1 artır

        using (Yazilima15EylulETicaretContext context = new())
        {

            Product? product = context
                .Products
                .FirstOrDefault(p => p.ProductID == id);


            product!.HighLighted = product.HighLighted + 1;


            context.Update(product);
            context.SaveChanges();


        }
    }


    public static List<QuickSearchViewModel> gettingSearchProducts(string id)
    {
        using (Yazilima15EylulETicaretContext context = new())
        {

            var products = context.sp_Aramas.FromSql($"sp_arama {id}").ToList();



            return products;
        }



    }










}
