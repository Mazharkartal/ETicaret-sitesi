﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YazılımOrg15Eylul_ETicaret.Models;

public class Comment
{

    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CommentID { get; set; }

    public int UserID { get; set; }

    public int ProductID { get; set; }

    [StringLength(150)]
    public string? Review { get; set; }

    public DateTime AddDate { get; set; }

    public int Puan { get; set; }







}
