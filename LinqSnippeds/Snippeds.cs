using System.Xml.Linq;

namespace LinqSnippeds
{
	public class Snippeds
	{
		/// <summary>
		/// Basic Examples whit LinQ
		/// </summary>
		public static void BasicLinQ()
		{
			List<string> cars = new List<string> {
				"VW Golf",
				"VW California",
				"Audi A3",
				"Audi A4",
				"Fiat Punto",
				"Seat Ibiza",
				"Seat León",
			};

			// 1. Select * of cars
			var carList = from car in cars
						  select car;

			Console.WriteLine("------------Cars List------------");
			foreach (var car in carList)
			{
				Console.WriteLine(car);
			}

			// 2. Select Where (Select AUDIs)
			var audiList = from car in cars
						   where car.Contains("Audi")
						   select car;

			Console.WriteLine("------------Audi List------------");
			foreach (var audi in audiList)
			{
				Console.WriteLine(audi);
			}
		}

		/// <summary>
		/// Number examples with LinQ
		/// </summary>
		public static void NumberLinQ()
		{
			List<int> numbers = new List<int>
			{
				1, 2, 3, 4, 5,
				6, 7, 8, 9,
			};

			// Get each number multiplied by 3
			// take all numbers but 9
			// Order numbers by ascending value
			var numberList = numbers
				.Select(n => n * 3) // { 3, 6, 9... }
				.Where(n => n != 9) // { all but 9 }
				.OrderBy(n => n) // at the end, we order ascending
				.ToList();
		}

		public static void SearchExamples()
		{
			List<string> lstText = new List<string>
			{
				"a",
				"bx",
				"c",
				"d",
				"e",
				"cj",
				"f",
				"c",
			};

			// 1. First of all elements
			var first = lstText.First();

			// 2. First element that is "c"
			var cText = lstText.First(t => t.Equals("c"));

			// 3. First element that contains "j"
			var jText = lstText.First(t => t.Contains("c"));

			// 4. First element that contains "z" or default ("")
			var firstOrDefaultText = lstText.FirstOrDefault(t => t.Contains("z"));

			// 5. Last element that contains "z" or default ("")
			var lastOrDefaultText = lstText.LastOrDefault(t => t.Contains("z"));

			// 6. Single values
			var uniqueText = lstText.Single();
			var uniqueOrDefaultText = lstText.SingleOrDefault();

			List<int> evenNumbers = new List<int>() { 0, 2, 4, 6, 8 };
			List<int> otherEvenNumbers = new List<int>() { 0, 2, 6 };

			var myEvenNumbers = evenNumbers.Except(otherEvenNumbers); // { 4, 8 }
		}

		public static void MultipleSelects()
		{
			// SELECT MANY
			List<string> opinions = new List<string>
			{
				"opinion 1, texto 1",
				"opinion 2, texto 2",
				"opinion 3, texto 3",
			};

			var myOpinionSelection = opinions
				.SelectMany(opinion => opinion.Split(","));

			List<Enterprise> enterprises = new List<Enterprise>()
			{
				new Enterprise()
				{
					Id = 1,
					Name = "Enterprise 1",
					Employees = new List<Employee>()
					{
						new Employee()
						{
							Id = 1,
							Name = "Martin",
							Email = "martin@imaginagroup.com",
							Salary = 3000,
						},
						new Employee()
						{
							Id = 2,
							Name = "Tyliz",
							Email = "tyliz@imaginagroup.com",
							Salary = 2500,
						},
						new Employee()
						{
							Id = 3,
							Name = "Pepe",
							Email = "pepe@imaginagroup.com",
							Salary = 1000,
						},
					},
				},
				new Enterprise()
				{
					Id = 2,
					Name = "Enterprise 2",
					Employees = new List<Employee>()
					{
						new Employee()
						{
							Id = 4,
							Name = "Jhenny",
							Email = "jheim@imaginagroup.com",
							Salary = 3000,
						},
						new Employee()
						{
							Id = 5,
							Name = "Luis",
							Email = "luis@imaginagroup.com",
							Salary = 2500,
						},
						new Employee()
						{
							Id = 6,
							Name = "Maria",
							Email = "maria@imaginagroup.com",
							Salary = 6000,
						},
					},
				},
			};

			// Obtain all Employees of all Enterprises
			var employeeList = enterprises
				.SelectMany(enterprise => enterprise.Employees);

			// Know if any list is empty
			bool hasEnterprises = enterprises.Any();

			bool hasEmployees = enterprises.Any(enterprise => enterprise.Employees.Any());

			// All enterprises has al least a employee with more than 1000€ of salary
			bool hasEmployeesSalaryMoreThen1000 = enterprises
				.Any(enterprise => enterprise.Employees.Any(employee => employee.Salary >= 1000));
		}

		public static void LinQCollections()
		{
			var firstList = new List<string>() { "a", "b", "c" };
			var secondList = new List<string>() { "a", "c", "d" };

			// clasic: INNER JOIN
			var commonResult = from element in firstList
							   join secondElement in secondList
							   on element equals secondElement
							   select new { element, secondElement };

			// new: INNER JOIN
			var commonResult2 = firstList.Join(
				secondList,
				element => element, // lo que se compara; podria ser .[key]
				secondElement => secondElement, // lo que se compara; podria ser .[key]
				(element, secondElement) => new { element, secondElement }
			);

			// OUTER JOIN - LEFT 
			var leftOuterJoin = from element in firstList
								join secondElement in secondList
								on element equals secondElement
								into temporalList
								from temporalElement in temporalList.DefaultIfEmpty()
								where element != temporalElement
								select new { Element = element };

			var leftOuterJoin2 = from element in firstList
								 from secondElement in secondList.Where(s => s == element).DefaultIfEmpty()
								 select new { Element = element, SecondElement = secondElement };

			// OUTER JOIN - RIGHT
			var rightOuterJoin = from secondElement in secondList
								 join element in firstList
								 on secondElement equals element
								 into temporalList
								 from temporalElement in temporalList.DefaultIfEmpty()
								 where secondElement != temporalElement
								 select new { Element = secondElement };

			// UNION
			var unionList = leftOuterJoin.Union(rightOuterJoin);
		}

		public static void SkipTakeLinQ()
		{
			List<int> numbers = new List<int>
			{
				1, 2, 3, 4, 5,
				6, 7, 8, 9, 10,
			};

			// Skip first 2 elements
			var skipTwoFirstValues = numbers.Skip(2); // { 3, 4, 5, 6, 7, 8, 9, 10, };
			var skipTwoLastValues = numbers.SkipLast(2); // { 1, 2, 3, 4, 5, 6, 7, 8 };

			var skipWhile = numbers.SkipWhile(n => n < 4);  // { 5, 6, 7, 8, 9, 10 }

			// TAKE
			var takeFirstTwoValues = numbers.Take(2); // { 1, 2 }
			var takeLastTwoValues = numbers.TakeLast(2); // { 9, 10 }

			var takeWhile = numbers.TakeWhile(n => n < 4); // { 1, 2, 3 }
		}
	}
}