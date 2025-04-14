namespace week5.Models
{
    public class ClassInformationModel
    {
        public int Id { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int StudentCount { get; set; }
    }
}