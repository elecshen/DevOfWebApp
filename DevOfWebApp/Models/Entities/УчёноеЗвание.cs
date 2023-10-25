using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DevOfWebApp.Models.Entities;

[Table("Учёное звание")]
public partial class УчёноеЗвание
{
    [Key]
    [Column("Код учёного звания")]
    public int КодУчёногоЗвания { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Наименование { get; set; } = null!;

    [InverseProperty("КодУчёногоЗванияNavigation")]
    public virtual ICollection<Преподаватели> Преподавателиs { get; set; } = new List<Преподаватели>();
}
