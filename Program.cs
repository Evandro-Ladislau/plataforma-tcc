using System;
using client.models;
using client.storage;
using client.api;

namespace plataforma_tcc
{
    class Program
    {
        static void Main(string[] args)
        {
            // Cria uma instância de ClientApi e inicia o servidor
            ClientApi api = new ClientApi();
            api.Start();
        }
    }
}
