using System;

namespace client.models
{
    public class ClientModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        private DateTime _birthDate;
        public DateTime Created_At {get; set;}
        public DateTime GetBithDate()
        {
            return _birthDate.Date;
        }
        public void SetBirthDate(DateTime birthDate)
        {
            _birthDate = birthDate;
        }
    }
}
