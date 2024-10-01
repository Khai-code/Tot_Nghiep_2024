using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
	public class TeacherSubjectDTO
	{
		public Guid Id { get; set; }
		public Guid TeacherId { get; set; }
		public List<Guid> SubjectIds { get; set; }


	}
}
