namespace OmiLAXR
{
    public abstract class DataProviderPipelineComponent : PipelineComponent
    {
        protected DataProvider dataProvider { get; private set; }
        protected virtual void Awake()
        {
            dataProvider = GetComponentInParent<DataProvider>(true);
        }
    }
}