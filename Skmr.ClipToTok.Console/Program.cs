using Skmr.ClipToTok.ViewModels;

//default
MainViewModel ctt = new MainViewModel();
ctt.OnLog += Ctt_OnLog;
ctt.Settings.Load();

var argument = Environment.GetCommandLineArgs()[1];
if (File.Exists(argument))
{
    ctt.Settings.Video = argument;
    ctt.Render();
}
else return;



void Ctt_OnLog(string line)
{
    Console.WriteLine(line); ;
}
