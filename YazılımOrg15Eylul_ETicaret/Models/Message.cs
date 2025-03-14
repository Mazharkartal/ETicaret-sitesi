using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YazılımOrg15Eylul_ETicaret.Models;

public class Message
{

    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MessageId { get; set; }

    public int UserID { get; set; }

    public int ProductID { get; set; }

    public string? Content { get; set; }










}
