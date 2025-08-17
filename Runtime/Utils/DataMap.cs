using System.Collections.Generic;

namespace OmiLAXR.Utils
{
    public class DataMap : Dictionary<string, object>
    {
        public static readonly DataMap empty = new DataMap();
    }
}