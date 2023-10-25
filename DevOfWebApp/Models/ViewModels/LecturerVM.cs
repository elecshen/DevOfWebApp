using DevOfWebApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace DevOfWebApp.Models.ViewModels
{
	public class LecturerVM
	{
		public LecturerVM() { }
		public LecturerVM(Преподаватели преподаватели)
		{
			IdПреподавателя = преподаватели.IdПреподавателя;
			IdИнститута = преподаватели.IdИнститута;
			Фамилия = преподаватели.Фамилия;
			Имя = преподаватели.Имя;
			Отчество = преподаватели.Отчество;
			Должность = преподаватели.Должность;
			СеменйноеПоложение = преподаватели.СеменйноеПоложение;
			КодУчёногоЗвания = преподаватели.КодУчёногоЗвания;
			Birthday = new DateTime(2034, 2, 15, 16, 53, 22);
			InsertedDateTime = new DateTime(2034, 2, 15, 16, 53, 22);
			WakeUpTime = new DateTime(2034, 2, 15, 16, 53, 22);
		}

		public Преподаватели GetПреподаватели()
		{
			return new Преподаватели()
			{
				IdПреподавателя = this.IdПреподавателя,
				IdИнститута = this.IdИнститута,
				Фамилия = this.Фамилия,
				Имя = this.Имя,
				Отчество = this.Отчество,
				Должность = this.Должность,
				СеменйноеПоложение = this.СеменйноеПоложение,
				КодУчёногоЗвания = this.КодУчёногоЗвания,
			};
		}

		public Guid IdПреподавателя { get; set; }

		[Required]
		[Display(Name = "Институт")]
		public Guid IdИнститута { get; set; }

		[Required]
		[StringLength(50, MinimumLength = 2)]
		public string Фамилия { get; set; } = null!;

		[Required]
		[StringLength(50, MinimumLength = 2)]
		public string Имя { get; set; } = null!;

		[StringLength(50, MinimumLength = 2)]
		public string Отчество { get; set; } = null!;

		[Required]
		[StringLength(50, MinimumLength = 2)]
		public string Должность { get; set; } = null!;

		[Required]
		[Display(Name = "Семейное положение")]
		[StringLength(50, MinimumLength = 2)]
		public string СеменйноеПоложение { get; set; } = null!;

		[Required]
		[Display(Name = "Учёное звание")]
		public int КодУчёногоЗвания { get; set; }

        [Display(Name = "Дата рождения")]
		public DateTime Birthday { get; set; }

		[Display(Name = "Дата и время добавления")]
		public DateTime InsertedDateTime { get; set; }

		[Display(Name = "Время подъема")]
		public DateTime WakeUpTime { get; set; }
    }
}