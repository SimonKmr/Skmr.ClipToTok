using Skmr.ClipToTok.ViewModels;

//default
MainViewModel ctt = new MainViewModel();
ctt.OnLog += Ctt_OnLog;
ctt.Settings.Load();
ctt.Settings.Video = Environment.GetCommandLineArgs()[1];
ctt.Render();


void Ctt_OnLog(string line)
{
    Console.WriteLine(line); ;
}
