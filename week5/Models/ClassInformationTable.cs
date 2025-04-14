namespace week5.Models
{
    public class ClassInformationTable
    {
        public int Id { get; set; }
        public required string ClassName { get; set; }
        public required string Description { get; set; }

        public int StudentCount { get; set; }
    }
}