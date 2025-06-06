using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using OmiLAXR.Extensions;

namespace OmiLAXR.Composers
{
    public class CsvFormat
    {
        public List<string> Headers { get; private set; } = new List<string>();
        public List<List<object>> Rows { get; } = new List<List<object>>();
        public string Separator { get; set; } = ",";

        public List<string> IncludedHeaders { get; set; } = null;
        public List<string> ExcludedHeaders { get; set; } = null;

        private Dictionary<string, int> _headerIndexCache;
        private void RebuildHeaderIndexCache()
        {
            _headerIndexCache = new Dictionary<string, int>(Headers.Count);
            for (int i = 0; i < Headers.Count; i++)
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
            Headers = headers?.ToList() ?? new List<string>();
            RebuildHeaderIndexCache();
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
            foreach (var key in values.Keys)
            {
                if (!Headers.Contains(key))
                {
                    // Check filters before adding
                    bool include = !((IncludedHeaders != null && IncludedHeaders.Count > 0 && !IncludedHeaders.Contains(key)) ||
                                     (ExcludedHeaders != null && ExcludedHeaders.Contains(key)));

                    if (include)
                    {
                        Headers.Add(key);
                    }
                }
            }
            RebuildHeaderIndexCache();

            var row = new List<object>(Headers.Count);
            foreach (var header in Headers)
            {
                row.Add(values.TryGetValue(header, out var val) ? val : null);
            }
            Rows.Add(row);
        }

        public void DropHeader(string headerName)
        {
            if (!HeaderIndexCache.TryGetValue(headerName, out var index))
                throw new ArgumentException($"Header '{headerName}' not found.");

            Headers.RemoveAt(index);
            foreach (var row in Rows)
            {
                if (index < row.Count)
                    row.RemoveAt(index);
            }
            RebuildHeaderIndexCache();
        }

        public string GetHeaderRow() => string.Join(Separator, Headers);
        public string GetFirstRow() => Rows.Count < 1 ? "" : string.Join(Separator, GetFilteredValues(Rows.First()));

        /// <summary>
        /// Returns a CSV string representation.
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();

            var filteredHeaders = GetFilteredHeaders();
            sb.AppendLine(string.Join(Separator, filteredHeaders.Select(FormatCsvValue)));

            foreach (var row in Rows)
                sb.AppendLine(string.Join(Separator, GetFilteredValues(row).Select(FormatCsvValue)));

            return sb.ToString();
        }

        private IEnumerable<string> GetFilteredHeaders()
        {
            var filtered = Headers;
            if (IncludedHeaders != null && IncludedHeaders.Count > 0)
                filtered = Headers.Where(h => IncludedHeaders.Contains(h)).ToList();
            if (ExcludedHeaders != null && ExcludedHeaders.Count > 0)
                filtered = filtered.Where(h => !ExcludedHeaders.Contains(h)).ToList();
            return filtered;
        }

        private IEnumerable<object> GetFilteredValues(List<object> row)
        {
            var filteredHeaders = GetFilteredHeaders().ToList();
            foreach (var header in filteredHeaders)
            {
                var index = HeaderIndexCache.TryGetValue(header, out var idx) ? idx : -1;
                yield return (index >= 0 && index < row.Count) ? row[index] : null;
            }
        }

        private static string FormatCsvValue(object value)
        {
            if (value == null) return "";
            var str = value.ToString();

            if (str.Contains(',') || str.Contains('"') || str.Contains('\n'))
                str = '"' + str.Replace("\"", "\"\"") + '"';

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
                for (int i = 0; i < row.Count; i++)
                    newRow[thisIndexMap[i]] = row[i];
                newRows.Add(newRow);
            }

            foreach (var row in other.Rows)
            {
                var newRow = Enumerable.Repeat<object>(null, combinedHeaders.Count).ToList();
                for (int i = 0; i < row.Count; i++)
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

            void AddObject(JObject obj, Dictionary<string, object> sharedContext = null)
            {
                var dict = flatten ? obj.Flatten() : obj.Properties().ToDictionary(
                    p => p.Name,
                    p => p.Value is JObject or JArray ? p.Value.ToString(Newtonsoft.Json.Formatting.None) : p.Value.ToObject<object>()
                );

                if (sharedContext != null)
                {
                    foreach (var kvp in sharedContext)
                        dict[kvp.Key] = kvp.Value;
                }

                foreach (var key in dict.Keys)
                    headers.Add(key);

                result.AddRow(dict);
            }

            switch (token)
            {
                case JArray array:
                    foreach (var item in array)
                    {
                        if (item is JObject obj)
                        {
                            var expandable = obj.Properties().FirstOrDefault(p => p.Value is JArray);
                            if (expandable != null)
                            {
                                var shared = obj.Properties().Where(p => p != expandable)
                                    .ToDictionary(p => p.Name, p => p.Value.ToObject<object>());

                                foreach (var subItem in (JArray)expandable.Value)
                                {
                                    if (subItem is JObject subObj)
                                        AddObject(subObj, shared);
                                }
                            }
                            else
                            {
                                AddObject(obj);
                            }
                        }
                    }
                    break;
                case JObject single:
                    AddObject(single);
                    break;
                default:
                    throw new ArgumentException("Root must be a JSON object or array of objects.");
            }

            result.SetHeaders(headers.OrderBy(k => k).ToArray());
            return result;
        }
    }
}
