using System.Net;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        HttpListener server = new HttpListener();
        // установка адресов прослушки
        server.Prefixes.Add("http://127.0.0.1:8888/connection/");
        server.Start(); // начинаем прослушивать входящие подключения

        while (true)
        {
            // получаем контекст
            var context = server.GetContextAsync();

            var response = context.Result.Response;

            var request = context.Result.Request;


            Console.WriteLine($"адрес приложения: {request.LocalEndPoint}");
            Console.WriteLine($"адрес клиента: {request.RemoteEndPoint}");
            Console.WriteLine(request.RawUrl);
            Console.WriteLine($"Запрошен адрес: {request.Url}");
            Console.WriteLine("Заголовки запроса:");
            foreach (string item in request.Headers.Keys)
            {
                Console.WriteLine($"{item}:{request.Headers[item]}");
            }

            // отправляемый в ответ код htmlвозвращает
            string responseText = "";


            using (StreamReader sr = new StreamReader(@"d:\\db\index.html"))
            {
                responseText = sr.ReadToEnd();
            }

            byte[] buffer = Encoding.UTF8.GetBytes(responseText);
            // получаем поток ответа и пишем в него ответ
            response.ContentLength64 = buffer.Length;
            using Stream output = response.OutputStream;
            // отправляем данные
            output.WriteAsync(buffer);
            output.FlushAsync();
            output.Close();

            Console.WriteLine("Запрос обработан");
        }

        server.Stop();
    }
}