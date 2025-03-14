﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YazılımOrg15Eylul_ETicaret.Models;

public class Supplier
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SupplierID { get; set; }

    [Required]
    [StringLength(100)]
    [DisplayName("Marka Adı")]
    public string? BrandName { get; set; }

    [DisplayName("Logo")]
    public string? PhotoPath { get; set; }
    [DisplayName("Aktif/Deaktif")]
    public bool Active { get; set; }










}
