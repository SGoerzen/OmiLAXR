using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using OmiLAXR.Extensions;

namespace OmiLAXR.Utils
{
    public delegate string HeaderFormatter(string header);

    public class CsvFormat
    {
        public CsvFormat(params string[] headers)
        {
            Headers = headers.ToList();
            Rows = new List<List<object>>();
            RebuildHeaderIndexCache();
        }

        public CsvFormat(int rowsCapacity, params string[] headers)
        {
            Headers = headers.ToList();
            Rows = new List<List<object>>(rowsCapacity);
            RebuildHeaderIndexCache();
        }

        public List<string> Headers { get; private set; }
        public List<List<object>> Rows { get; }
        public string Separator { get; set; } = ",";

        public Regex IncludedHeaderPattern { get; set; } = null;
        public Regex ExcludedHeaderPattern { get; set; } = null;

        private Dictionary<string, int> _headerIndexCache;
        
        private void RebuildHeaderIndexCache()
        {
            _headerIndexCache = new Dictionary<string, int>(Headers.Count);
            for (var i = 0; i < Headers.Count; i++)
                _headerIndexCache[Headers[i]] = i;
        }

        private Dictionary<string, int> HeaderIndexCache
        {
            get
            {
                if (_headerIndexCache == null || _headerIndexCache.Count != Headers.Count)
                    RebuildHeaderIndexCache();
                return _headerIndexCache;
            }
        }

        public void SetHeaders(params string[] headers)
        {
            Headers = headers?.ToList() ?? new List<string>(50);
            RebuildHeaderIndexCache();
        }

        public void FormatHeaders(HeaderFormatter formatter)
        {
            Headers = Headers.Select(h => formatter(h)).ToList();
        }

        public void ClearRows()
        {
            Rows.Clear();
        }

        public void RenameHeader(string oldName, string newName)
        {
            if (!HeaderIndexCache.TryGetValue(oldName, out var index))
                throw new ArgumentException($"Header '{oldName}' not found.");

            Headers[index] = newName;
            RebuildHeaderIndexCache();
        }

        public void AddRow(Dictionary<string, object> values)
        {
            var headerChanged = false;

            foreach (var key in values.Keys)
            {
                var include = (IncludedHeaderPattern == null || IncludedHeaderPattern.IsMatch(key)) &&
                              (ExcludedHeaderPattern == null || !ExcludedHeaderPattern.IsMatch(key));

                if (include && !Headers.Contains(key))
                {
                    Headers.Add(key);
                    headerChanged = true;
                }
            }

            if (headerChanged)
                RebuildHeaderIndexCache();
            
            var row = new List<object>(Headers.Count);
            foreach (var header in Headers)
            {
#if UNITY_2021_2_OR_NEWER
                row.Add(values.GetValueOrDefault(header));
#else
                values.TryGetValue(header, out var value);
                row.Add(value);
#endif
            }

            Rows.Add(row);
        }


        public void DropHeader(string headerName)
        {
            if (!HeaderIndexCache.TryGetValue(headerName, out var index))
                throw new ArgumentException($"Header '{headerName}' not found.");

            Headers.RemoveAt(index);
            foreach (var row in Rows.Where(row => index < row.Count))
            {
                row.RemoveAt(index);
            }

            RebuildHeaderIndexCache();
        }

        public string GetFirstRow() =>
            Rows.Count < 1 ? "" : string.Join(Separator, GetFilteredStringValues(Rows.First()));

        public string[] GetRowStrings()
        {
            var result = new string[Rows.Count];
            var filteredHeaders = GetFilteredHeaders().ToList();

            for (var i = 0; i < Rows.Count; i++)
            {
                var row = Rows[i];
                var formatted = new List<string>(filteredHeaders.Count);

                foreach (var header in filteredHeaders)
                {
                    var index = HeaderIndexCache.TryGetValue(header, out var idx) ? idx : -1;
                    var value = (index >= 0 && index < row.Count) ? row[index] : null;
                    formatted.Add(FormatCsvValue(value));
                }

                result[i] = string.Join(Separator, formatted);
            }

            return result;
        }

        /// <summary>
        /// Returns a CSV string representation.
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();

            var filteredHeaders = GetFilteredHeaders();
            sb.AppendLine(string.Join(Separator, filteredHeaders.Select(FormatCsvValue)));

            foreach (var row in Rows)
                sb.AppendLine(string.Join(Separator, GetFilteredStringValues(row)));

            return sb.ToString();
        }

        /// <summary>
        /// Returns a CSV string representation without the headers.
        /// </summary>
        /// <returns></returns>
        public string GetHeadersString()
            => string.Join(Separator, GetFilteredHeaders());

        /// <summary>
        /// Returns a CSV string representation without the headers.
        /// </summary>
        /// <returns></returns>
        public string GetRowsString()
        {
            var sb = new StringBuilder();

            foreach (var row in Rows)
                sb.AppendLine(string.Join(Separator, GetFilteredStringValues(row).Select(FormatCsvValue)));

            return sb.ToString();
        }

        private List<string> GetFilteredHeaders()
        {
            var result = new List<string>(Headers.Count);
            for (var i = 0; i < Headers.Count; i++)
            {
                var h = Headers[i];
                if ((IncludedHeaderPattern == null || IncludedHeaderPattern.IsMatch(h)) &&
                    (ExcludedHeaderPattern == null || !ExcludedHeaderPattern.IsMatch(h)))
                {
                    result.Add(h);
                }
            }

            return result;
        }

        private List<string> GetFilteredStringValues(List<object> row)
        {
            if (Headers == null || row == null)
                return new List<string>();

            var result = new List<string>(Headers.Count); // conservative guess

            foreach (var header in Headers)
            {
                if ((IncludedHeaderPattern != null && !IncludedHeaderPattern.IsMatch(header)) ||
                    (ExcludedHeaderPattern != null && ExcludedHeaderPattern.IsMatch(header)))
                    continue;

                if (!HeaderIndexCache.TryGetValue(header, out var idx) || idx >= row.Count)
                {
                    result.Add(""); // missing column
                    continue;
                }

                var value = row[idx];
                result.Add(FormatCsvValue(value));
            }

            return result;
        }

        public static string FormatCsvValue(object value)
        {
            if (value == null) return "";
            var str = value.ToString();

            // Escape if needed
            if (str.Contains(',') || str.Contains('"') || str.Contains('\n'))
                str = "\"" + str.Replace("\"", "\"\"") + "\"";

            return str;
        }

        public void Append(CsvFormat other)
        {
            if (other == null || other.Rows.Count == 0)
                return;

            var combinedHeaders = Headers.Union(other.Headers).ToList();
            var combinedIndexMap = combinedHeaders
                .Select((h, i) => new { h, i })
                .ToDictionary(x => x.h, x => x.i);

            var thisIndexMap = Headers.Select(h => combinedIndexMap[h]).ToList();
            var otherIndexMap = other.Headers.Select(h => combinedIndexMap[h]).ToList();

            var newRows = new List<List<object>>();

            foreach (var row in Rows)
            {
                var newRow = Enumerable.Repeat<object>(null, combinedHeaders.Count).ToList();
                for (var i = 0; i < row.Count; i++)
                    newRow[thisIndexMap[i]] = row[i];
                newRows.Add(newRow);
            }

            foreach (var row in other.Rows)
            {
                var newRow = Enumerable.Repeat<object>(null, combinedHeaders.Count).ToList();
                for (var i = 0; i < row.Count; i++)
                    newRow[otherIndexMap[i]] = row[i];
                newRows.Add(newRow);
            }

            Headers = combinedHeaders;
            Rows.Clear();
            Rows.AddRange(newRows);
            RebuildHeaderIndexCache();
        }

        public static CsvFormat MergeMany(IEnumerable<CsvFormat> csvs)
        {
            var result = new CsvFormat();
            foreach (var csv in csvs)
            {
                if (result.Headers.Count == 0 && result.Rows.Count == 0)
                {
                    result.SetHeaders(csv.Headers.ToArray());
                    foreach (var row in csv.Rows)
                        result.Rows.Add(new List<object>(row));
                }
                else
                {
                    result.Append(csv);
                }
            }

            return result;
        }

        public static CsvFormat FromJson(string json, bool flatten = false) => FromJson(JToken.Parse(json), flatten);

        public static CsvFormat FromJson(JToken token, bool flatten = false)
        {
            var result = new CsvFormat();
            var headers = new HashSet<string>();

            if (token is JArray array)
            {
                foreach (var item in array)
                {
                    var obj = item as JObject;
                    if (obj == null)
                        continue;

                    var expandable = obj.Properties().FirstOrDefault(prop => prop.Value is JArray);

                    if (expandable != null)
                    {
                        var sharedContext = new Dictionary<string, object>();
                        foreach (var prop in obj.Properties())
                        {
                            if (prop != expandable)
                                sharedContext[prop.Name] = prop.Value.ToObject<object>();
                        }

                        foreach (var subItem in (JArray)expandable.Value)
                        {
                            if (subItem is JObject subObj)
                                AddObject(subObj, sharedContext);
                        }
                    }
                    else
                    {
                        AddObject(obj);
                    }
                }
            }
            else if (token is JObject single)
            {
                AddObject(single);
            }
            else
            {
                throw new ArgumentException("Root must be a JSON object or array of objects.");
            }

            result.SetHeaders(headers.ToArray());
            return result;

            void AddObject(JObject obj, Dictionary<string, object> sharedContext = null)
            {
                Dictionary<string, object> dict;

                if (flatten)
                {
                    dict = obj.Flatten();
                }
                else
                {
                    dict = new Dictionary<string, object>();
                    foreach (var prop in obj.Properties())
                    {
                        if (prop.Value is JObject || prop.Value is JArray)
                            dict[prop.Name] = prop.Value.ToString(Newtonsoft.Json.Formatting.None);
                        else
                            dict[prop.Name] = prop.Value.ToObject<object>();
                    }
                }

                if (sharedContext != null)
                {
                    foreach (var kvp in sharedContext)
                        dict[kvp.Key] = kvp.Value;
                }

                foreach (var key in dict.Keys)
                    headers.Add(key);

                result.AddRow(dict);
            }
        }
    }
}