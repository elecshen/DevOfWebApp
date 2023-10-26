using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DevOfWebApp.Models.Entities;

[Table("Преподаватели")]
public partial class Преподаватели
{
    [Key]
    [Column("ID преподавателя")]
    public Guid IdПреподавателя { get; set; }

    [Column("ID института")]
    public Guid IdИнститута { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Фамилия { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Имя { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Отчество { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Должность { get; set; } = null!;

    [Display(Name = "Семейное положение")]
    [Column("Семенйное положение")]
    [StringLength(50)]
    [Unicode(false)]
    public string СеменйноеПоложение { get; set; } = null!;

    [Column("Код учёного звания")]
    public int КодУчёногоЗвания { get; set; }

    [Display(Name = "Институт")]
    [ForeignKey("IdИнститута")]
    [InverseProperty("Преподавателиs")]
    public virtual Институты IdИнститутаNavigation { get; set; } = null!;

    [Display(Name = "Учёное звание")]
    [ForeignKey("КодУчёногоЗвания")]
    [InverseProperty("Преподавателиs")]
    public virtual УчёноеЗвание КодУчёногоЗванияNavigation { get; set; } = null!;
}
