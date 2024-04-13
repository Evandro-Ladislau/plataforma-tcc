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

        public ClientModel() 
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Active = true;
        }
    }
}
