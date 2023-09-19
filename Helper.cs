
namespace OneBot;

public class Helper
{
  public static void GetConfig(out string link, out int time, out int replay)
  {
    time = 0;
    replay = 0;
    link = string.Empty;

    string path = AppDomain.CurrentDomain.BaseDirectory + "config.txt";
    if (File.Exists(path))
    {
      var data = File.ReadAllLines(path);
      link = data[0].Replace("LINK: ", "");
      time = Convert.ToInt32(data[1].Replace("TIME: ", ""));
      replay = Convert.ToInt32(data[2].Replace("REPLAY: ", ""));
    }
  }

  public static HttpClient http = new HttpClient();
  public static async Task<string> CallAPI(string link)
  {
    var res = await http.GetAsync(link);
    var result = await res.Content.ReadAsStringAsync();

    return result;
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
}