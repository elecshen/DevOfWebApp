namespace ЛР1.Models
{
	public class FirstModel
	{
		public static List<Student> GetStudents()
		{
			return new()
			{
				new("Анна", 4.5),
				new("Иван", 3.8),
				new("Мария", 4.2),
				new("Петр", 3.5),
				new("Елена", 4.1),
			};
		}

		public struct Student
		{
			public string Name;
			public double Score;

			public Student(string name, double score)
			{
				Name = name;
				Score = score;
			}
		}

		public static List<string> GetVegetablesList()
		{
			List<string> list = new()
			{
				"Томат",
				"Огурец",
				"Картофель",
				"Кабачок",
				"Баклажан",
				"Капуста",
				"Брокколи"
			};
			return list;
		}
	}
}
