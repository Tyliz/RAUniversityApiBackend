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

		// Paging with Skip & Take
		public static IEnumerable<T> GetPage<T>(IEnumerable<T> collection, int page, int resultsPerPage)
		{
			int startIndex = (page - 1) * resultsPerPage;

			return collection.Skip(startIndex).Take(resultsPerPage);
		}

		/// Variables
		public static void LinqVariables()
		{
			List<int> numbers = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

			var aboveAverage = from number in numbers
							   let average = numbers.Average()
							   let nSquare = Math.Pow(number, 2)
							   where nSquare > average
							   select number;

			Console.WriteLine("Average: {0}", numbers.Average());
			foreach (int number in aboveAverage)
			{
				Console.WriteLine("Query: Number: {0} Square: {1}", number, Math.Pow(number, 2));
			}
		}

		// Zip
		public static void ZipLinQ()
		{
			List<int> numbers = new() { 1, 2, 3, 4, 5 };
			List<string> stringNumbers = new() { "One", "Two", "Three", "Four", "Five" };

			IEnumerable<string> zipNumbers = numbers.Zip(
				stringNumbers,
				(number, word) => string.Format("{0}={1}", numbers, word)
			); // { "1=One", "2=Two", ...}
		}

		// Repeat & Range
		public static void RepeatRangeLinQ()
		{
			// Generate collection from 1 - 1000
			IEnumerable<int> first1000 = Enumerable.Range(1, 1000);


			// Repeat a Value N times
			IEnumerable<string> fiveXs = Enumerable.Repeat("X", 5);
		}

		public static void StudentsLinQ()
		{
			List<Student> classRom = new()
			{
				new Student()
				{
					Id = 1,
					Name = "Martin",
					Grade = 90,
					Certified = true,
				},
				new Student()
				{
					Id = 2,
					Name = "Tyliz",
					Grade = 90,
					Certified = false,
				},
				new Student()
				{
					Id = 3,
					Name = "Luis",
					Grade = 50,
					Certified = false,
				},
				new Student()
				{
					Id = 4,
					Name = "Jhen",
					Grade = 75,
					Certified = true,
				},
			};

			var certifiedStudents = from student in classRom
									where student.Certified
									select student;

			var notCertifiedStudents = from student in classRom
									   where !student.Certified
									   select student;

			var approvedStudents = from student in classRom
								   where student.Grade >= 50 && student.Certified
								   select student.Name;
		}

		// ALL
		public static void AllLinQ()
		{
			List<int> numbers = new() { 1, 2, 3, 4, 5 };

			bool allAreSmallerThan10 = numbers.All(number => number < 10); // true
			bool allAreBiggerOrEqualThan2 = numbers.All(number => number >= 2); // false

			List<int> emptyList = new();

			bool allNumberAreGreaterThan0 = emptyList.All(number => number > 0); // true
		}

		public static void AgregaterQueries()
		{
			List<int> numbers = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

			int sum = numbers.Aggregate((prevNum, current) => prevNum + current);

			List<string> words = new() { "hello,", "my", "name", "is", "Tyliz" };

			string greeting = words.Aggregate((previous, current) => previous + " " + current).Trim();
		}

		public static void DistinctValues()
		{
			List<int> numbers = new() { 1, 2, 3, 4, 5, 1, 2, 3, 4, 5 };

			var disctintValues = numbers.Distinct();
		}

		public static void GroupByExample()
		{
			List<int> numbers = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

			var grouped = numbers.GroupBy(x => x % 2 == 0);

			// We will have two groups:
			// 1. The group that doesn't fit the condition (odd numbers)
			// 2. The group that fits the condition (even numbers)

			foreach (var group in grouped)
			{
				foreach (var value in group)
				{
					Console.WriteLine(value); // 1,3,5,7,9 ... 2,4,6,8
				}
			}

			List<Student> classRom = new()
			{
				new Student()
				{
					Id = 1,
					Name = "Martin",
					Grade = 90,
					Certified = true,
				},
				new Student()
				{
					Id = 2,
					Name = "Tyliz",
					Grade = 90,
					Certified = false,
				},
				new Student()
				{
					Id = 3,
					Name = "Luis",
					Grade = 50,
					Certified = false,
				},
				new Student()
				{
					Id = 4,
					Name = "Jhen",
					Grade = 75,
					Certified = true,
				},
			};

			var approvedQuery = classRom
				.GroupBy(student => student.Certified && student.Grade > 50);

			var certifiedQuery = classRom
				.GroupBy(student => student.Certified);

			// We obtain 2 groups
			// 1. Not certified students
			// 2. Certified students

			foreach (var group in certifiedQuery)
			{
				Console.WriteLine("-------------{0}-------------", group.Key);
				foreach (var student in group)
				{
					Console.WriteLine("{0}: {1}", student.Name, student.Certified);
				}
			}
		}

		public static void RelationsLinQ()
		{
			List<Post> post = new()
			{
				new Post()
				{
					Id = 1,
					Title = "First Post",
					Content = "First Post, content",
					Created = DateTime.Now,
					Comments = new()
					{
						new Comment()
						{
							Id = 1,
							Title = "First comment",
							Content = "First comment content 1",
							Created = DateTime.Now,
						},
						new Comment()
						{
							Id = 2,
							Title = "2 comment",
							Content = "2 comment content 2",
							Created = DateTime.Now,
						},
					},
				},
				new Post()
				{
					Id = 2,
					Title = "First Post",
					Content = "First Post, content",
					Created = DateTime.Now,
					Comments = new()
					{
						new Comment()
						{
							Id = 3,
							Title = "First fasdft comment",
							Content = "First comment fasd content 1",
							Created = DateTime.Now,
						},
						new Comment()
						{
							Id = 4,
							Title = "2 comment fasdfsa ",
							Content = "2 comment content 2 adsfsa dfad",
							Created = DateTime.Now,
						},
					},
				},
			};

			var commentsWithContent = post
				.SelectMany(
					post => post.Comments,
					(post, comment) => new {
						IdPost = post.Id,
						CommentContent = comment.Content
					}
				);
		}
	}
}