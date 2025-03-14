using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YazılımOrg15Eylul_ETicaret.Models;

public class Status
{

    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int StatusID { get; set; }

    [StringLength(100)]
    [Required]
    [DisplayName("Durum")]
    public string? StatusName { get; set; }

    [DisplayName("Aktif/Deaktif")]
    public bool Active { get; set; }














}
