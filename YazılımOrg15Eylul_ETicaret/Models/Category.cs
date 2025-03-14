using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YazılımOrg15Eylul_ETicaret.Models;

public class Category
{

    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CategoryID { get; set; }

    public int ParentID { get; set; }

    [StringLength(50, ErrorMessage = "En fazla 50 karakter girilebilir")]
    [Required(ErrorMessage = "Kategori Adı Zorunlu Alan")]
    [DisplayName("Kategori Adı")]
    public string? CategoryName { get; set; }

    [DisplayName("Aktif/Deaktif")]
    public bool Active { get; set; }













}
