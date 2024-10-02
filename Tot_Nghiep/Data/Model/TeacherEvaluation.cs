using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class TeacherEvaluation
    {
        //[Key]
        //public Guid Id { get; set; }  // ID đánh giá

        //[Required]
        //public Guid TeacherId { get; set; }  // Mã giáo viên (TeacherId)
        //[ForeignKey("TeacherId")]
        //public Teacher Teacher { get; set; }  // Liên kết đến giáo viên

        //[Required]
        //public Guid StudentId { get; set; }  // Mã học sinh (StudentId)
        //[ForeignKey("StudentId")]
        //public Student Student { get; set; }  // Liên kết đến học sinh

        //public string Notes { get; set; }  // Nội dung đánh giá

        //[Required, MaxLength(255)]
        //public string Title { get; set; }  // Tiêu đề đánh giá

        //public DateTime CreatedAt { get; set; } = DateTime.Now;  // Thời gian tạo

        //public bool Status { get; set; }  // Trạng thái đánh giá
    }
}
