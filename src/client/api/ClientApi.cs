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
            string[] prefixes = { "http://localhost:8080/" };
            HttpListener listener = new HttpListener();

            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }

            listener.Start();
            
            string port = listener.Prefixes.First().Split(':')[2].TrimEnd('/');
            logger.Info($"Server started and listening on port {port}...");

            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                logger.Info($"Request: {request.HttpMethod} {request.Url}");

                if (request.HttpMethod == "POST" && request.Url.AbsolutePath == "/client")
                {
                    InsertClient(request, response);
                }
                else if(request.HttpMethod == "GET" && request.Url.AbsolutePath == "/client" || request.Url.AbsolutePath == "/client/")
                {
                    SelectClient(request, response);
                }
                else if(request.HttpMethod == "GET" && request.Url.AbsolutePath.StartsWith("/client/"))
                {
                    string[] segments = request.Url.Segments;
                    int id;
                    
                    if(segments.Length > 2 && int.TryParse(segments[2], out id)){
                        selectClientById(request, response, id);
                    }
                }
                else if(request.HttpMethod == "PUT" && request.Url.AbsolutePath.StartsWith("/client/"))
                {
                    string[] segments = request.Url.Segments;
                    int id;
                    
                    if(segments.Length > 2 && int.TryParse(segments[2], out id)){
                        UpdateClient(request, response, id);
                    }
                }
                else if (request.HttpMethod == "DELETE" && request.Url.AbsolutePath.StartsWith("/client/"))
                {
                    string[] segments = request.Url.Segments;
                    int id;
                    
                    if(segments.Length > 2 && int.TryParse(segments[2], out id)){
                        DeleteClient(request, response, id);
                    }
                }
                else
                {
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
                SendResponse(response, HttpStatusCode.BadRequest, "Invalid JSON format");
                return;
            }
            try
            {
                var responseInsert = service.Insert(client);
            
                if(responseInsert)
                {
                    SendResponse(response, HttpStatusCode.OK, "Client Inserted successfully");
                }
                else
                {
                    SendResponse(response, HttpStatusCode.BadRequest, "All customer data must be filled in correctly!");     
                }
            }
            catch (Exception ex)
            {
                SendResponse(response, HttpStatusCode.InternalServerError, $"Error: {ex.Message}");
            }
        }

        private void SelectClient(HttpListenerRequest request, HttpListenerResponse response)
        {
            try
            {
                List<ClientModel> clientList = service.Select();

                if(clientList != null)
                {
                    string responseBody = JsonConvert.SerializeObject(clientList);
                    SendResponse(response, HttpStatusCode.OK, responseBody);
                }
                else
                {
                    SendResponse(response, HttpStatusCode.NotFound, "No clients found");
                }
            }
            catch (Exception ex)
            {
                SendResponse(response, HttpStatusCode.InternalServerError, $"Error: {ex.Message}");
            }
        }

        private void selectClientById(HttpListenerRequest request, HttpListenerResponse response, int id)
        {
            try
            {
                client = service.selectById(id);
            
                if(client != null)
                {
                    string responseBody = JsonConvert.SerializeObject(client);
                    SendResponse(response, HttpStatusCode.OK, responseBody);
                }
                else
                {
                    SendResponse(response, HttpStatusCode.NotFound, "client not found");
                }
            }
            catch (Exception ex)
            {
                SendResponse(response, HttpStatusCode.InternalServerError, $"Error: {ex.Message}");
            }
            
        }

        private void UpdateClient(HttpListenerRequest request, HttpListenerResponse response, int id)
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
                SendResponse(response, HttpStatusCode.BadRequest, "Invalid JSON format");
                return;
            }
            try
            {
                var responseUpdate = service.Update(id, client);

                if (responseUpdate)
                {
                    SendResponse(response, HttpStatusCode.OK, "Client updated successfully");
                }
                else
                {
                    SendResponse(response, HttpStatusCode.BadRequest, "Invalid client data");
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error updating client: {ex.Message}");
                SendResponse(response, HttpStatusCode.InternalServerError, $"Error updating client: {ex.Message}");
            } 
        }

        private void DeleteClient(HttpListenerRequest request, HttpListenerResponse response, int id)
        {
            try
            {
                var clientDelete = service.Delete(id);
            
                if(clientDelete)
                {
                    SendResponse(response, HttpStatusCode.OK, "client deleted  successfully");
                }
                else
                {
                    SendResponse(response, HttpStatusCode.NotFound, "client not found");
                }
            }
            catch (Exception ex)
            {
                SendResponse(response, HttpStatusCode.InternalServerError, $"Error: {ex.Message}");
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
            string responseString = "Endpoint not found!";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }
    }
}
