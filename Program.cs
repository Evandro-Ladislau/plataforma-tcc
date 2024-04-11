using System;
using client.models;
using client.storage;

namespace plataforma_tcc
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime dateT = new DateTime(1979, 03, 20);
            ClientModel cli1 = new ClientModel("Cecilia", "Ladislau", "cecilialadsialu@plataformaimpact.com", dateT);

            //ClientStorage storage = new ClientStorage();
            //storage.Insert(cli1);
        }
    }
}
