using System.Text;
using System.Threading.Tasks;
using FellowOakDicom;
using FellowOakDicom.IO.Buffer;

public class DicomWalker : IDicomDatasetWalker
{
  private StringBuilder _dicomContent = new StringBuilder();
  public string DicomContent => _dicomContent.ToString();
  private int _depth = 0;
  public void Walk(DicomDataset dataset)
  {
    var walker = new DicomDatasetWalker(dataset);
    walker.Walk(this);
  }

  public void OnBeginWalk()
  {
    _dicomContent.AppendLine("Starting DICOM dataset traversal");
  }

  public bool OnElement(DicomElement element)
  {
    var tag = element.Tag;
    DicomTag dtag;
    var vr = element.ValueRepresentation;
    var value = GetElementValue(element);
    var indent = new string(' ', _depth * 2);
    _dicomContent.AppendLine($"{indent}Tag: {tag} ({tag.DictionaryEntry.Name}), VR: {vr}, Value: {value}");
    return true;
  }

  public Task<bool> OnElementAsync(DicomElement element) { return Task.FromResult(true); }
  public bool OnBeginSequence(DicomSequence sequence) { return true; }
  public bool OnBeginSequenceItem(DicomDataset dataset) { return true; }
  public bool OnEndSequenceItem() { return true; }
  public bool OnEndSequence() { return true; }
  public bool OnBeginFragment(DicomFragmentSequence fragment) { return true; }
  public bool OnFragmentItem(IByteBuffer item) { return true; }
  public Task<bool> OnFragmentItemAsync(IByteBuffer item) { return Task.FromResult(true); }
  public bool OnEndFragment() { return true; }
  public void OnEndWalk() { }

  private string GetElementValue(DicomElement element)
  {
    if (element.Tag == DicomTag.PixelData || element.ValueRepresentation == DicomVR.OB)
        return "<pixel data>";
    try
    {
      // Handle multi-valued elements and different VR types
      if (element.Count == 0)
        return "<empty>";
      if (element.Count > 1)
        return $"[{string.Join(", ", element.Get<string[]>())}]";
      return element.Get<string>(0) ?? "<null>";
    }
    catch
    {
      return "<unable to retrieve>";
    }
  }
}
