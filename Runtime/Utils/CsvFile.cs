namespace OmiLAXR.Utils
{
    public class CsvFile : BufferedUtf8Writer
    {
        public string[] Headers { get; private set; }
        public bool AutoFlush { get; set; }
        public CsvFile(string path, string[] headers = null, bool autoFlush = false, int bufferSize = 8192) : base(path, bufferSize)
        {   
            if (headers != null)
                SetHeaders(headers);
            AutoFlush = autoFlush;
        }

        public void SetHeaders(params string[] headers)
        {
            Headers = headers;
            WriteLine(string.Join(",", headers));
            if (AutoFlush)
                Flush();
        }

        public void WriteRow(params object[] values)
        {
            WriteLine(string.Join(",", values));
            if (AutoFlush)
                Flush();       
        }
    }
}