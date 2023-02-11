namespace MetaTranspiler
{
    public partial class ParserGenerator
    {
        internal struct FileData
        {
            public string FileName { get; private set; }
            public string Path { get; private set; }
            public string Content { get; private set; }
            public FileData(string name, string path, string content)
            {
                FileName = name;
                Path = path;
                Content = content;
            }
        }

    }
}
