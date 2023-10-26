using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DevOfWebApp.Models.Entities;

[Table("User")]
public partial class User
{
    [Key]
    [Column("ID User")]
    public Guid IdUser { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Login { get; set; } = null!;

    [StringLength(200)]
    [Unicode(false)]
    public string PasswordHash { get; set; } = null!;

    public Guid Salt { get; set; }

    [Column("Role ID")]
    public int RoleId { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role Role { get; set; } = null!;
}
