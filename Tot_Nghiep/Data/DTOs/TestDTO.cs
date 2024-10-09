namespace Data.DTOs
{
    public class TestDTO
    {
        public Guid Id { get; set; }
        public int Code { get; set; }
        public DateTime CreationTime { get; set; }
        public int Status { get; set; }
        public int MaxStatush { get; set; }
        public Guid SubjectId { get; set; }
        public Guid PointTypeId { get; set; }
        public string? Name { get; set; }
        public int? Minute { get; set; }
        //public int? NumberOfTestCode { get; set; }
        public string ClassCode { get; set; }

    }
}
