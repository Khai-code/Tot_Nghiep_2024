namespace Data.DTOs
{
    public class ExamDTO
    {
        public Guid? Id { get; set; }
        public DateTime CreationTime { get; set; }
        public int Status { get; set; }
        public Guid SubjectId { get; set; }
    }
}
