using System;
using System.Linq;

namespace OmiLAXR.Pipeline
{
    public delegate bool PipelineDataFilter<T>(T data);
    public class PipelineData
    {
        public readonly object[] Data;

        private PipelineData(object[] data)
        {
            Data = data;
        }

        public static PipelineData From<T>(T data) 
        {
            return new PipelineData(data as object[]);
        }

        public static PipelineData Empty => new PipelineData(Array.Empty<object>());

        public PipelineData Clone()
        {
            var destData = new object[Data.Length];
            Buffer.BlockCopy(Data, 0, destData, 0, Data.Length);
            return From(destData);
        }

        public PipelineData Filter<T>(PipelineDataFilter<T> filter)
        {
            var filteredData = Data.Where(d => filter((T)d));
            return From(filteredData.ToArray());
        }
    }
}