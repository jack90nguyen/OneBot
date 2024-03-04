
using System.Net;

namespace OneBot;

public class Helper
{
  public static void GetConfig(out List<string> links, out int time, out int replay)
  {
    time = 0;
    replay = 0;
    links = new();

    string path = AppDomain.CurrentDomain.BaseDirectory + "config.txt";
    if (File.Exists(path))
    {
      var data = File.ReadAllLines(path);
      time = Convert.ToInt32(data[0].Replace("TIME: ", ""));
      replay = Convert.ToInt32(data[1].Replace("REPLAY: ", ""));
      for (int i = 2; i < data.Count(); i++)
        links.Add(data[i]);
    }
  }

  public static HttpClient http = new HttpClient();
  public static async Task<string> CallAPI(string link)
  {
    try
    {
      var res = await http.GetAsync(link);
      if(res.StatusCode == HttpStatusCode.OK)
      {
        Console.WriteLine("Status: " + res.StatusCode);
        string result = await res.Content.ReadAsStringAsync();
        if(!result.Contains("An error occurred while processing your request."))
          return result;
        else
        {
          TelegramBot($"{link}|Server Error: An error occurred while processing your request");
          Console.ForegroundColor = ConsoleColor.Red;
          return "Server Error"; 
        }
      }
      else
      {
        TelegramBot($"{link}|Status Error: {res.StatusCode}");
        Console.ForegroundColor = ConsoleColor.Red;
        return "Server Error. Status: " + res.StatusCode;
      }
    }
    catch (System.Exception ex)
    {
      TelegramBot($"{link}|API Error: {ex.Message}");
      Console.ForegroundColor = ConsoleColor.Red;
      return "API Error";
    }
  }

  public static async Task Waiting(int minutes)
  {
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write($"Waiting {minutes} minutes");
    for (int i = 0; i < minutes; i += 5)
    {
      await Task.Delay(TimeSpan.FromMinutes(5));
      Console.Write(".");
    }
    Console.WriteLine("\n");
  }

  private static readonly string _botId = "bot5195155040";
  private static readonly string _botKey = "AAEBXhtC_Z0498EyE6eJmltvDfBYlQD1yzA";
  private static readonly string _botChat = "-731595697";

  public static void TelegramBot(string message)
  {
    message = message.Replace("|", "\n").Trim();
    new Task(async () =>
    {
      var data = new Dictionary<string, string>
      {
        {"chat_id", _botChat},
        {"text", $"{DateTime.Now.ToString("dd/MM, HH:mm:ss")}\n{message}"}
      };

      ServicePointManager.Expect100Continue = true;
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

      var url = $"https://api.telegram.org/{_botId}:{_botKey}/sendMessage";
      var client = new HttpClient();
      var response = await client.PostAsync(url, new System.Net.Http.FormUrlEncodedContent(data));

      var result = await response.Content.ReadAsStringAsync();
    }).Start();
  }
}