namespace OmiLAXR.Endpoints
{
    public delegate void DataEndpointAction(DataEndpoint sender);
    public delegate void DataEndpointAction<in T>(DataEndpoint sender, T obj);
    public delegate void DataEndpointAction<in T1, in T2>(DataEndpoint sender, T1 obj1, T2 obj2);
    public delegate void DataEndpointAction<in T1, in T2, in T3>(DataEndpoint sender, T1 obj1, T2 obj2, T3 obj3);
    public delegate void DataEndpointAction<in T1, in T2, in T3, in T4>(DataEndpoint sender, T1 obj1, T2 obj2, T3 obj3, T4 obj4);
    public delegate void DataEndpointAction<in T1, in T2, in T3, in T4, in T5>(DataEndpoint sender, T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5);
    public delegate void DataEndpointAction<in T1, in T2, in T3, in T4, in T5, in T6>(DataEndpoint sender, T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5, T6 obj6);
}