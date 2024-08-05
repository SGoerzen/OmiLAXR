using System;
using System.Linq;

namespace OmiLAXR.Pipelines
{
    public delegate bool PipelineDataFilter<T>(T data);
    public class PipelineData<TDataType>
    {
        public readonly TDataType[] Data;

        private PipelineData(TDataType[] data)
        {
            Data = data;
        }

        public static PipelineData<TDataType> From(TDataType[] data) 
        {
            return new PipelineData<TDataType>(data);
        }

        public static PipelineData<TDataType> Empty => new PipelineData<TDataType>(Array.Empty<TDataType>());

        public PipelineData<TDataType> Clone()
        {
            var destData = new TDataType[Data.Length];
            Buffer.BlockCopy(Data, 0, destData, 0, Data.Length);
            return From(destData);
        }

        public PipelineData<TDataType> Filter(PipelineDataFilter<TDataType> filter)
        {
            var filteredData = Data.Where(d => filter(d));
            return From(filteredData.ToArray());
        }

        public PipelineData<T> ConvertTo<T>()
        {
            var neutralData = Data as object[];
            var data = neutralData as T[];
            return new PipelineData<T>(data);
        }
    }
}