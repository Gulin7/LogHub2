namespace LogHub2.Models.Entities
{
    public class Log
    {
 
        public int Id { get; set; }
        public string Type { get; set; }
        public string Severity { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }

    }
}
