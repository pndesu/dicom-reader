using Avalonia;
using Avalonia.ReactiveUI;
using FellowOakDicom;
using FellowOakDicom.Imaging;
using System;

namespace dicom_reader;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args){
      BuildDicom();
      BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
    
    public static void BuildDicom(){
      new DicomSetupBuilder()
        .RegisterServices(s => s.AddImageManager<ImageSharpImageManager>())
        .Build();
    }
}
