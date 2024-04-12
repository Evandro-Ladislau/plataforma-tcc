using System;
using System.Net;

namespace  client.api
{
    public class ClientApi
    {
        public void Start(string[] prefixes)
        {
            //string prefix = "http://localhost:8080/";
            HttpListener listener = new HttpListener();
            //listener.Prefixes.Add(prefix);
            // Adiciona os prefixos fornecidos
            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }

            listener.Start();

            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                Console.WriteLine($"Recebido pedido: {request.HttpMethod} {request.Url}");

                // Aqui você pode adicionar a lógica para processar a requisição
                // Por exemplo, verificar o método da requisição e a URL para decidir o que fazer

                // Aqui estamos apenas enviando uma resposta simples de "Olá Mundo!"
                string responseString = "<html><body><h1>Olá mundo!</h1></body></html>";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);
                response.OutputStream.Close();
            }
        }
    }

}
