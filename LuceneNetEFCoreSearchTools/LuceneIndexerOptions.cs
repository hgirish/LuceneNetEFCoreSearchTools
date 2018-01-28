namespace LuceneNetEFCoreSearchTools
{
    public class LuceneIndexerOptions
    {
        public bool UseRamDirectory { get;  set; }
        public string Path { get;  set; }
        public int? MaximumFieldLength { get;  set; }

        //public LuceneIndexerOptions(
        //    string path,
        //    int? maximumFieldLength = null,
        //    bool useRamDirectory = false)
        //{
        //    Path = path;
        //    MaximumFieldLength = maximumFieldLength;
        //    UseRamDirectory = useRamDirectory;
        //}
    }
}
