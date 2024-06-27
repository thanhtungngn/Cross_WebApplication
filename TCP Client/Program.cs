using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

class Program
{
    private static StreamWriter logWriter;
    private static readonly Random random = new Random();

    static async Task Main(string[] args)
    {
        string host = "localhost";  
        int port = 13001;           

        using var timer = new System.Timers.Timer(5000);
        timer.Elapsed += async (sender, e) => await SendEventAsync(host, port);
        timer.Start();

        // Initialize log file
        logWriter = new StreamWriter("tcp-communications.log", append: true);
        await logWriter.WriteLineAsync($"--- Started logging at {DateTime.Now} ---");

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();

        timer.Stop();
        await logWriter.WriteLineAsync($"--- Stopped logging at {DateTime.Now} ---");
        await logWriter.FlushAsync();
        logWriter.Close();
    }

    private static async Task SendEventAsync(string host, int port)
    {
        var client = new TcpClient();

        try
        {
            await client.ConnectAsync(host, port);
            using var stream = client.GetStream();
            var eventMessage = GenerateEvent();
            var data = Encoding.ASCII.GetBytes(eventMessage);

            Console.WriteLine($"Sending: {eventMessage}");
            await logWriter.WriteLineAsync($"Sending: {eventMessage}");
            await logWriter.FlushAsync();
            await stream.WriteAsync(data, 0, data.Length);

            // Close the client
            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            await logWriter.WriteLineAsync($"Exception: {ex.Message}");
        }
    }

    private static string GenerateEvent()
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string randomData = GenerateRandomString(20);
        return $"{timestamp} -- {randomData}";
    }

    private static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var stringChars = new char[length];
        for (int i = 0; i < length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }
        return new string(stringChars);
    }
}
