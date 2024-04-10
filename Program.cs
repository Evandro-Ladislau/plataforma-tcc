using System;
using client.models;
using client.storage;

namespace plataforma_tcc
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientModel cli1 = new ClientModel();
            cli1.Name = "Evandro";
            
            Console.WriteLine(cli1.Name);

            ClientStorage conn = new ClientStorage();

            var listNames = conn.GetClientNames();
            foreach (var item in listNames)
            {
                Console.WriteLine(item);
            }

        }
    }
}
