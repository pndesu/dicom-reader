using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;

namespace dicom_reader.ViewModels;

public partial class TestWindowViewModel : ReactiveObject
{
  public ICommand ClickButtonCommand {get;}
}
