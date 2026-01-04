using Microsoft.CodeAnalysis.Text;

namespace MetaParser;

internal struct FileData
{
    public string FileName { get; private set; }
    public string Path { get; private set; }
    public string Content { get; private set; }
    
    /// <summary>
    /// The source text for creating locations. May be null if not available.
    /// </summary>
    public SourceText? SourceText { get; private set; }

    public FileData(string name, string path, string content, SourceText? sourceText = null)
    {
        FileName = name;
        Path = path;
        Content = content;
        SourceText = sourceText;
    }
}
