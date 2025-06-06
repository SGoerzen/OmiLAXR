using System.Collections.Generic;
using System.IO;
using OmiLAXR.Composers;

namespace OmiLAXR.Endpoints
{
    public class CsvWriterEndpoint : LocalFileEndpoint
    {
        public bool flatten = false;
        public List<string> includeHeaders;
        public List<string> excludeHeaders;

        protected override string GetDefaultExtension() => "csv";

        private CsvFormat ToCsvFormat(IStatement statement)
        {
            var csvFormat = statement.ToCsvFormat();
            csvFormat.ExcludedHeaders = excludeHeaders;
            csvFormat.IncludedHeaders = includeHeaders;
            
            return csvFormat;
        }
        
        protected override void BeforeWrite(StreamWriter writer, IStatement statement, bool isFirstLine)
        {
            if (!isFirstLine)
                return;
            
            var csv = ToCsvFormat(statement);
            writer.WriteLine(csv.GetHeaderRow());
            writer.Flush();
        }

        protected override string FormatLine(IStatement statement)
        {
            var csv = ToCsvFormat(statement);
            return csv.GetFirstRow();
        }
    }
}