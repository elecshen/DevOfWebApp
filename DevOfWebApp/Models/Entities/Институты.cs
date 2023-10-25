using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DevOfWebApp.Models.Entities;

[Table("Институты")]
public partial class Институты
{
    [Key]
    [Column("ID института")]
    public Guid IdИнститута { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Наименование { get; set; } = null!;

    [InverseProperty("IdИнститутаNavigation")]
    public virtual ICollection<Преподаватели> Преподавателиs { get; set; } = new List<Преподаватели>();
}
