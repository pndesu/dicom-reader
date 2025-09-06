using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;

namespace dicom_reader.ViewModels;

public partial class MainWindowViewModel : ReactiveObject
{
  public string Greeting { get; } = "Welcome to Avalonia!";
  public TestWindowViewModel TestWindowViewModel {get;} = new TestWindowViewModel();
}
