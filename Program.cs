using System;
using Client.Models;

namespace plataforma_tcc
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientModel cli1 = new ClientModel();
            cli1.Name = "Evandro";
            
            Console.WriteLine(cli1.Name);
        }
    }
}
