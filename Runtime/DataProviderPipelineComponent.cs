namespace OmiLAXR
{
    public abstract class DataProviderPipelineComponent : PipelineComponent
    {
        protected DataProvider DataProvider { get; private set; }
        protected virtual void OnEnable()
        {
            #if UNITY_2019
            DataProvider = GetComponentInParent<DataProvider>();
            #else 
            DataProvider = GetComponentInParent<DataProvider>(true);
            #endif
        }
    }
}