using System;

namespace client.models
{
    public class ClientModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public  DateTime BirthDate {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
        public bool Active {get; set;}

         public ClientModel(string name, string surname, string email, DateTime birthDate)
        {
            Name = name;
            Surname = surname;
            Email = email;
            BirthDate = birthDate;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Active = true;
        }
    }
}
