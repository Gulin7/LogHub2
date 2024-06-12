namespace LogHub2.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        private static int currentId = 1;

        public User(string username, string password)
        {
            Id = currentId;
            Username = username;
            Password = password;
            
            currentId+=1;
        }
    }
}
