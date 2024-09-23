namespace Data.DTOs
{
    public class ExamRoomDTO
    {
        public Guid? Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Status { get; set; }
        public Guid RoomId { get; set; }
        public Guid ExamId { get; set; }
        public Guid TeacherId1 { get; set; }
        public Guid TeacherId2 { get; set; }
    }
}
