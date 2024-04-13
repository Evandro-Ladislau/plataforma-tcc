using System;
using client.models;
using client.storage;

namespace client.services
{
    class ClientService 
    {
        private ClientStorage storage = new ClientStorage();
        public void Insert(ClientModel client)
        {
           storage.Insert(client);
        }
    }
}