using System;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using NLog;
using client.services;
using client.models;

namespace client.api
{
    public class ClientApi
    {
        ClientService service = new ClientService();
        ClientModel client;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public void Start()
        {
            string[] prefixes = { "http://localhost:8080/client/" };
            HttpListener listener = new HttpListener();

            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }

            listener.Start();
            logger.Info("Server started and listening...");

            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                logger.Info($"Request: {request.HttpMethod} {request.Url}");

                Console.WriteLine($"Request: {request.HttpMethod} {request.Url}");

                if (request.HttpMethod == "POST" && request.Url.AbsolutePath == "/client")
                {
                    InsertClient(request, response);
                }
                else
                {
                    // Rota padrão
                    DefaultRequest(response);
                }
            }
        }

        private void InsertClient(HttpListenerRequest request, HttpListenerResponse response)
        {
            string requestBody;
            using (StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                requestBody = reader.ReadToEnd();
            }
            // Desserializar a string JSON para um objeto Cliente
            try
            {
                client = JsonConvert.DeserializeObject<ClientModel>(requestBody);
            }
            catch (JsonException ex)
            {
                logger.Error($"Error deserializing JSON: {ex.Message}");
            }

            var responseInsert = service.Insert(client);
            
            if(responseInsert){
                SendResponse(response, HttpStatusCode.OK, "Client Inserted successfully");

            }else{
                SendResponse(response, HttpStatusCode.BadRequest, "All customer data must be filled in correctly!");     
            }
        }
    
        private void SendResponse(HttpListenerResponse response, HttpStatusCode statusCode, string message)
        {
            response.StatusCode = (int)statusCode;
            response.ContentType = "text/plain";

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes($"{(int)statusCode} - " + message);
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }
        private void DefaultRequest(HttpListenerResponse response)
        {
            //PRECISO MELHORAR ESSE METODO E VER SE REALMENTE É NECESSÁRIO
            string responseString = "<html><body><h1>Endpoint não encontrado</h1></body></html>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }
    }
}
