namespace OmiLAXR
{
    public interface IDataProviderExtension
    {
        DataProvider GetDataProvider();
        void Extend(DataProvider dataProvider);
    }
}