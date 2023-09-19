using OneBot;

string title = "OneBot - by Jack Nguyen";

Console.Title = title;

Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine($"::::: {title} :::::\n");

Helper.GetConfig(out string link, out int time, out int replay);

Console.WriteLine("Link: " + link);
if(replay == 0)
  Console.WriteLine("Time: " + time + " hour\n");
else
  Console.WriteLine("Replay: " + replay + " minutes\n");

while (true)
{
  if(DateTime.Now.Hour == 0)
    Console.Clear();
  
  Console.ForegroundColor = ConsoleColor.White;
  Console.WriteLine("Runtime: " + DateTime.Now.ToString("yyyy-MM-dd, HH:mm"));

  if(replay == 0)
  {
    // Chạy theo mốc giờ
    if(DateTime.Now.Hour == time)
    {
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine("CallAPI: " + link);

      var results = await Helper.CallAPI(link);
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("Results: " + results);
    }

    await Helper.Waiting(60);
  }
  else
  {
    // Chạy lặp lại theo phút
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("CallAPI: " + link);

    var results = await Helper.CallAPI(link);
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Results: " + results);

    await Helper.Waiting(replay);
  }
}


//Console.ReadKey();