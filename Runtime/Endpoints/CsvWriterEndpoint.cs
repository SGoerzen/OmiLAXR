using System.Collections.Generic;
using UnityEngine;

namespace OmiLAXR.Endpoints
{
    public class CsvWriterEndpoint : LocalFileEndpoint
    {
        public bool includeUri = false;
        public List<string> excludedHeaders;
        // todo
        [Tooltip("Will be ordered first.")]
        public List<string> priorityHeaders;
        private void OnValidate() {
		}
    }
}