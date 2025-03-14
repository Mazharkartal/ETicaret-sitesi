using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YazılımOrg15Eylul_ETicaret.Models;

public class TopluEmail
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TopluEmailId { get; set; }

    public string? Email { get; set; }

    public bool Active { get; set; }
}
