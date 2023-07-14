using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSnippeds
{
	public class Post
	{
		public int Id { get; set; }

		public string Title { get; set; } = string.Empty;

		public string Content { get; set; } = string.Empty;

		public List<Comment> Comments { get; set; } = new List<Comment>();

		public DateTime Created { get; set; }
	}
}
