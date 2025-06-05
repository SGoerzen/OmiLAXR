using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using OmiLAXR.Extensions;

namespace OmiLAXR.Composers
{
    /// <summary>
    /// Represents a simple in-memory CSV structure with configurable headers, separator, and modifiable column structure.
    /// </summary>
    public class CsvFormat
    {
        /// <summary>
        /// List of column headers.
        /// </summary>
        public List<string> Headers { get; private set; } = new();

        /// <summary>
        /// Rows stored as list of object lists.
        /// </summary>
        public List<List<object>> Rows { get; } = new();

        /// <summary>
        /// The separator used for CSV formatting (default: comma).
        /// </summary>
        public string Separator { get; set; } = ",";

        /// <summary>
        /// Sets or replaces the current headers.
        /// </summary>
        /// <param name="headers">Array of header names.</param>
        public void SetHeaders(params string[] headers)
        {
            Headers = headers?.ToList() ?? new List<string>();
        }

        /// <summary>
        /// Renames an existing header.
        /// </summary>
        public void RenameHeader(string oldName, string newName)
        {
            int index = Headers.IndexOf(oldName);
            if (index == -1)
                throw new ArgumentException($"Header '{oldName}' not found.");

            Headers[index] = newName;
        }

        /// <summary>
        /// Adds a new row of values.
        /// </summary>
        public void AddRow(params object[] values)
        {
            if (Headers.Count > 0 && values.Length != Headers.Count)
                throw new ArgumentException("Row length must match header count.");

            Rows.Add(values.ToList());
        }

        /// <summary>
        /// Drops a header and removes its corresponding column from all rows.
        /// </summary>
        /// <param name="headerName">The name of the header to remove.</param>
        public void DropHeader(string headerName)
        {
            int index = Headers.IndexOf(headerName);
            if (index == -1)
                throw new ArgumentException($"Header '{headerName}' not found.");

            Headers.RemoveAt(index);

            foreach (var row in Rows)
            {
                if (index < row.Count)
                {
                    row.RemoveAt(index);
                }
            }
        }

        public string GetRow()
            => Rows.Count < 1 ? "" : string.Join(Separator, Rows.First().Select(FormatCsvValue));

        /// <summary>
        /// Returns a CSV string representation.
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();

            if (Headers.Count > 0)
            {
                sb.AppendLine(string.Join(Separator, Headers.Select(FormatCsvValue)));
            }

            foreach (var row in Rows)
            {
                sb.AppendLine(string.Join(Separator, row.Select(FormatCsvValue)));
            }

            return sb.ToString();
        }

        private static string FormatCsvValue(object value)
        {
            if (value == null) return "";
            var str = value.ToString();

            if (str.Contains(",") || str.Contains("\"") || str.Contains("\n"))
            {
                str = "\"" + str.Replace("\"", "\"\"") + "\"";
            }

            return str;
        }

        /// <summary>
        /// Merges another CsvFormat into this one. Combines headers and aligns all rows accordingly.
        /// </summary>
        /// <param name="other">The CsvFormat to merge.</param>
        public void MergeWith(CsvFormat other)
        {
            var combinedHeaders = Headers.Union(other.Headers).ToList();
            var thisIndexMap = Headers.Select(h => combinedHeaders.IndexOf(h)).ToList();
            var otherIndexMap = other.Headers.Select(h => combinedHeaders.IndexOf(h)).ToList();

            // Remap current rows
            var newThisRows = new List<List<object>>();
            foreach (var row in Rows)
            {
                var newRow = Enumerable.Repeat<object>(null, combinedHeaders.Count).ToList();
                for (int i = 0; i < row.Count; i++)
                    newRow[thisIndexMap[i]] = row[i];
                newThisRows.Add(newRow);
            }

            // Remap other rows
            foreach (var row in other.Rows)
            {
                var newRow = Enumerable.Repeat<object>(null, combinedHeaders.Count).ToList();
                for (int i = 0; i < row.Count; i++)
                    newRow[otherIndexMap[i]] = row[i];
                newThisRows.Add(newRow);
            }

            Headers = combinedHeaders;
            Rows.Clear();
            Rows.AddRange(newThisRows);
        }

        /// <summary>
        /// Merges a list of CsvFormat instances into one.
        /// </summary>
        /// <param name="csvs">CSV files to merge.</param>
        public static CsvFormat MergeMany(IEnumerable<CsvFormat> csvs)
        {
            var result = new CsvFormat();
            foreach (var csv in csvs)
            {
                if (result.Headers.Count == 0 && result.Rows.Count == 0)
                {
                    // Start with first CSV
                    result.SetHeaders(csv.Headers.ToArray());
                    foreach (var row in csv.Rows)
                        result.Rows.Add(new List<object>(row));
                }
                else
                {
                    result.MergeWith(csv);
                }
            }

            return result;
        }

        public static CsvFormat FromJson(string json)
            => FromJson(JToken.Parse(json));
        /// <summary>
        /// Parses a JSON string into a CsvFormat, whether the root is an object or array.
        /// </summary>
        public static CsvFormat FromJson(JToken token)
        {
            List<Dictionary<string, object>> flattenedRows = new();
            var allKeys = new HashSet<string>();

            if (token is JArray array)
            {
                foreach (var item in array)
                {
                    if (item is JObject obj)
                    {
                        var flat = obj.Flatten();
                        flattenedRows.Add(flat);
                        foreach (var key in flat.Keys)
                            allKeys.Add(key);
                    }
                    else
                    {
                        throw new ArgumentException("Array elements must be JSON objects.");
                    }
                }
            }
            else if (token is JObject singleObject)
            {
                var flat = singleObject.Flatten();
                flattenedRows.Add(flat);
                foreach (var key in flat.Keys)
                    allKeys.Add(key);
            }
            else
            {
                throw new ArgumentException("Root must be a JSON object or array of objects.");
            }

            var headers = allKeys.OrderBy(k => k).ToList();
            var csv = new CsvFormat();
            csv.SetHeaders(headers.ToArray());

            foreach (var rowDict in flattenedRows)
            {
                var row = headers.Select(h => rowDict.TryGetValue(h, out var val) ? val : null).ToArray();
                csv.AddRow(row);
            }

            return csv;
        }
    }
}