using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YazılımOrg15Eylul_ETicaret.Models;

public class User
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserID { get; set; }

    [Required]
    [StringLength(100)]
    public string? NameSurname { get; set; }

    [Required]
    [StringLength(100)]
    public string? Email { get; set; }

    [Required]
    [StringLength(100)]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    public string? Telephone { get; set; }

    public string? InvoicesAddress { get; set; }

    public bool IsAdmin { get; set; }

    public bool Active { get; set; }





}
