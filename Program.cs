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
            ClientApi api = new ClientApi();
            api.Start();
        }
    }
}
