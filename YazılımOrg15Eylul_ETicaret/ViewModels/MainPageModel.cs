using YazılımOrg15Eylul_ETicaret.Models;

namespace YazılımOrg15Eylul_ETicaret.ViewModels;

public class MainPageModel
{

    public List<Product>? SliderProducts { get; set; } //slider 

    public List<Product>? NewProducts { get; set; } //yeni 

    public Product? ProductsOfDay { get; set; }  //günün ürünü

    public List<Product>? SpecialProducts { get; set; } //özel

    public List<Product>? StarProducts { get; set; } //yıldızz
    public List<Product>? FeaturedProducts { get; set; } //fırsat
    public List<Product>? DiscountedProducts { get; set; } //İndirim
    public List<Product>? HighlightedProducts { get; set; } //Öne çıkanlar
    public List<Product>? TopsellerProducts { get; set; } //Çok satan
    public List<Product>? NotableProducts { get; set; } //Dikkat çeken
    //public List<Product>? ProductsByCategory { get; set; } //Kategori getiren CategoryPage

    public Product? ProductDetails { get; set; }

    public string? CategoryName { get; set; }

    public string? BrandName { get; set; }

    public List<Product>? RelatedProducts { get; set; }

    public List<Comment>? Comments { get; set; }





}
