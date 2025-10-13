using System;
using Newtonsoft.Json;

namespace OmiLAXR.Types
{
    [Serializable]
    public struct UnitValue
    {
        [JsonProperty("value")] public readonly double Value;
        [JsonProperty("unit")] public readonly string Unit;

        public UnitValue(string unit, double value)
        {
            Unit = unit;
            Value = value;
        }
    }
}