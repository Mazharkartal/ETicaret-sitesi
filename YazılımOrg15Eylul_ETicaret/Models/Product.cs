using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YazılımOrg15Eylul_ETicaret.Models;

public class Product
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [DisplayName("ID")]
    public int ProductID { get; set; }


    [StringLength(100)]
    [Required]
    [DisplayName("Ürün Adı")]
    public string? ProductName { get; set; }

    [DisplayName("Fiyat")]
    public decimal UnitPrice { get; set; }

    [DisplayName("Kategori")]
    public int CategoryID { get; set; }

    [DisplayName("Marka")]
    public int SupplierID { get; set; }

    [DisplayName("Stok")]
    public int Stock { get; set; }

    [DisplayName("İndirim")]
    public int Discount { get; set; }

    [DisplayName("Durum")]
    public int StatusID { get; set; }

    [DisplayName("Eklenme Tarihi")]
    public DateTime AddDate { get; set; }

    [DisplayName("Anahtar Kelimeler")]
    public string? Keywords { get; set; }

    private int _Kdv { get; set; }

    [DisplayName("KDV")]
    public int Kdv
    {
        get { return _Kdv; }
        set { _Kdv = Math.Abs(value); }
    }

    public int HighLighted { get; set; }    //Öne çıkanlar bilgisi burdan alınacak

    public int Topseller { get; set; }  //Çok satanların Satış Sayısı alınıcak kolon

    [DisplayName("İlişkili")]
    public int Related { get; set; }   // ingili ürünler  buna bakanlar bunada baktı

    [DisplayName("Notlar")]
    public string? Notes { get; set; }

    [DisplayName("Fotoğraf")]
    public string? PhotoPath { get; set; }

    [DisplayName("Aktif/Deaktif")]
    public bool Active { get; set; }


    public int ToplamPuan { get; set; }

    public int OySayisi { get; set; }

    public decimal OrtlamaPuan { get; set; }



}
