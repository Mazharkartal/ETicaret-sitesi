using Microsoft.Data.SqlClient;
using YazılımOrg15Eylul_ETicaret.Data;
using YazılımOrg15Eylul_ETicaret.Models;
using YazılımOrg15Eylul_ETicaret.ViewModels;

namespace YazılımOrg15Eylul_ETicaret.Services;

public class OrderService
{

    public int ProductID { get; set; }
    public string? ProductName { get; set; }

    public int Quantity { get; set; }

    public string? Sepet { get; set; } //  5=1&10=1&6=3 şeklinde eklenir

    public decimal UnitPrice { get; set; }

    public int Kdv { get; set; }

    public string? PhotoPath { get; set; }

    Yazilima15EylulETicaretContext context = new();

    //sepete ekle işini yapan bir metod
    public bool AddToCart(string id)
    {
        bool varmi = false;

        if (Sepet == "")
        {
            //sepette ilk defa ürün ekleniyor 
            Sepet = id + "=1";

        }
        else
        {
            //daha önceden sepete birşey(ler) eklenmiş ama şuan eklemek istediği ürün sepetinde zaten varmı bilmiyoruz onu kontrol etmeliyiz
            string[] sepetdizi = Sepet!.Split('&');
            for (int i = 0; i < sepetdizi.Length; i++)
            {
                //10=1  -> sepetdizi[0]
                //20=1  -> sepetdizi[1]
                string[] sepetdizi2 = sepetdizi[i].Split('=');

                if (sepetdizi2[0] == id)
                {
                    //buraya düşerse ürün sepette var manasına gelir
                    varmi = true;
                }

            }
            if (varmi == false)
            {
                //ürün sepete daha önce eklenmemiş
                Sepet = Sepet + "&" + id + "=1";
            }
        }




        return varmi;

    }



    //Projede sağ üst köşedeki sepet sayfası ve  sil buttonu tıklanınca yüklenecek olan sayfa bu metodu çağıracak
    //list<cls_Order> = propertyleri dönecekkk
    //siparişi onaylama metodudda bunu çağırıyor ve kullanıyor
    public List<OrderService> GetMyCart()
    {
        List<OrderService> list = new List<OrderService>();
        string[] sepetdizi = Sepet!.Split('&');
        // sepeteki  ürünleri ayırdık 5=1 


        if (sepetdizi[0] != "")
        {
            for (int i = 0; i < sepetdizi.Length; i++)
            {
                string[] sepetdizi2 = sepetdizi[i].Split('='); // 5 1
                int sepetid = Convert.ToInt32(sepetdizi2[0]);  // bu productID
                int adet = Convert.ToInt32(sepetdizi2[1]);     // Quantity

                Product? product = context
                    .Products.FirstOrDefault
                    (p => p.ProductID == sepetid);

                OrderService p = new OrderService();
                p.ProductID = product!.ProductID;
                p.ProductName = product.ProductName;
                p.Quantity = adet;
                p.UnitPrice = product.UnitPrice;
                p.Kdv = product.Kdv;
                p.PhotoPath = product.PhotoPath;
                list.Add(p);


            }



        }

        return list;




    }



    public void DeleteFromMyCart(string id)
    {
        string[] sepetdizi = Sepet!.Split('&'); //ürünleri ayırıyoruzz

        string yenisepet = "";
        for (int i = 0; i < sepetdizi.Length; i++)
        {
            //
            string[] sepetdizi2 = sepetdizi[i].Split('=');
            int adet = Convert.ToInt32(sepetdizi2[1]);
            if (sepetdizi2[0] != id)
            {
                //silinmeyecek ürünleri burda yakalıyoruz ve yeni bir sepet (dizin) oluşturuyoruz
                if (yenisepet == "")
                {
                    yenisepet = sepetdizi2[0] + "=" + adet;  //sepetdizi[i]

                }
                else
                {
                    yenisepet = yenisepet + "&" + sepetdizi2[0] + "=" + adet;
                }



            }


        }

        Sepet = yenisepet;











    }




    public string WriteFromCookieToTable(string Email)
    {
        //ürünleri ayrı ayrı dönerken (siparişler tablosuna işlerken)
        //satın alınan ürünlerin stock değeri quantity kadar azalatıcaz 
        //ve yine o ürünlerin topseller kolonunu qunatity kadar arttır


        string OrderGroupGUID = DateTime.Now.ToString().Replace(".", "").Replace(" ", "").Replace(":", "");
        DateTime orderdate = DateTime.Now;

        List<OrderService> orders = GetMyCart();


        foreach (var item in orders)
        {
            Order order = new Order();
            order.OrderDate = orderdate;
            order.OrderGroupGUID = OrderGroupGUID;
            order.UserID = context.Users.FirstOrDefault(u => u.Email == Email)!.UserID;
            order.ProductID = item.ProductID;
            order.Quantity = item.Quantity;
            context.Orders.Add(order);//siparişler tablosuna bu sipariş ekleniyor

            Product product = context.Products.FirstOrDefault(p => p.ProductID == order.ProductID)!;

            product.Stock = product.Stock - order.Quantity;

            if (product.Stock == 0)
                product.Active = false;


            product.Topseller = product.Topseller + order.Quantity;



            context.SaveChanges();


        }




        return OrderGroupGUID;
    }



    public List<MyOrdersViewModel> SelectMyOrders(string Email)
    {
        int? UserID = context.Users.FirstOrDefault(u => u.Email == Email)!.UserID;

        List<MyOrdersViewModel> orders = context.vw_MyOrders.Where(o => o.UserID == UserID).ToList();

        return orders;
    }


    public List<OrderService> Select_Products_DetailsSearch(string query)
    {
        List<OrderService> products = new();

        SqlConnection sqlcon = connection.baglanti;
        SqlCommand sqlcmd = new SqlCommand(query, sqlcon);
        sqlcon.Open();
        SqlDataReader sqlDataReader = sqlcmd.ExecuteReader();
        while (sqlDataReader.Read())
        {
            OrderService p = new OrderService();
            p.ProductID = Convert.ToInt32(sqlDataReader["ProductID"]);
            p.ProductName = sqlDataReader["ProductName"].ToString();
            p.UnitPrice = Convert.ToDecimal(sqlDataReader["UnitPrice"]);
            p.PhotoPath = sqlDataReader["PhotoPath"].ToString();
            products.Add(p);

        }
        sqlcon.Close();
        return products;

    }





}
