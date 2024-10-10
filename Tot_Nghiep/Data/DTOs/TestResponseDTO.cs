namespace Data.DTOs
{
    public class TestResponseDTO
    {
        public IEnumerable<TestGridDTO> ListTest { get; set; }
    }
    public class TestGridDTO
    {
        public Guid Id { get; set; }
        //public int Type { get; set; }
        public int? Minute { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? NumberOfTestCode { get; set; }
        public int Status { get; set; }
        public Guid SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string Creator { get; set; }
      public string namepoint { get; set; }
        
    }
}
