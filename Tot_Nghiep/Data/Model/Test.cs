namespace Data.Model
{
    public class Test
    {
        public Guid Id { get; set; }
        //public int Type { get; set; }
        public string Name { get; set; }
        public int Code { get; set; }
        public int? Minute { get; set; }
        //public int? NumberOfTestCode { get; set; }
        public DateTime CreationTime { get; set; }
        public int Status { get; set; }
        public Guid SubjectId { get; set; }
        public Guid PointTypeId { get; set; }
        public virtual Subject? Subject { get; set; }
        public virtual PointType? PointType { get; set; }
        public virtual ICollection<TestQuestion>? testQuestions { get; set; }
        public virtual ICollection<TestCode>? testCodes { get; set; }
    }
}
