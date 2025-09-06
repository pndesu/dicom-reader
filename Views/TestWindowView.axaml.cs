using System;
using System.IO;
using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using FellowOakDicom;
using FellowOakDicom.Imaging;
using NPOI.XWPF.UserModel;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

namespace dicom_reader.Views;

public partial class TestWindowView : UserControl
{
  public TestWindowView()
  {
    InitializeComponent();
  }

  public static FilePickerFileType DicomFileType {get;} = new FilePickerFileType("DicomFile"){
    Patterns = new[] {"*.dicom", "*.dcm"}
  };
  private async void OpenFileButton_Clicked(object sender, RoutedEventArgs args)
  {
    var topLevel = TopLevel.GetTopLevel(this);
    var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
    {
      Title = "Open text file",
      AllowMultiple = false,
      FileTypeFilter = new[] {DicomFileType}
    });

    if (files.Count >= 1)
    {
      await using var stream = await files[0].OpenReadAsync();
      DicomTag dtag;
      var dicomFile = await DicomFile.OpenAsync(stream);
      var walker = new DicomWalker();
      walker.Walk(dicomFile.Dataset);
      DicomTextContent.Text = walker.DicomContent;
      Console.Write(walker.DicomContent);
      Image<Bgra32> renderedImage = new DicomImage(dicomFile.Dataset).RenderImage().AsSharpImage();
      using var memoryStream = new MemoryStream();
      renderedImage.Save(memoryStream, new PngEncoder());
      memoryStream.Position = 0;
      Bitmap avaloniaImage = new Bitmap(memoryStream);
      DicomImg.Source = avaloniaImage;
    }
  }
}

