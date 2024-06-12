using System.ComponentModel.DataAnnotations;

namespace LogHub2.Models.Entities
{
    public class Parent
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }
        public int UserId { get; set; }

        private static int currentId = 1;

        public Parent(string name, string type, int userId)
        {
            Id = currentId;
            Name = name;
            Type = type;
            UserId = userId;

            currentId += 1;
        }
    }
}
