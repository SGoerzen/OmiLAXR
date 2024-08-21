namespace OmiLAXR.Endpoints
{
    public delegate void EndpointAction(Endpoint sender);
    public delegate void EndpointAction<in T>(Endpoint sender, T obj);
    public delegate void EndpointAction<in T1, in T2>(Endpoint sender, T1 obj1, T2 obj2);
    public delegate void EndpointAction<in T1, in T2, in T3>(Endpoint sender, T1 obj1, T2 obj2, T3 obj3);
    public delegate void EndpointAction<in T1, in T2, in T3, in T4>(Endpoint sender, T1 obj1, T2 obj2, T3 obj3, T4 obj4);
    public delegate void EndpointAction<in T1, in T2, in T3, in T4, in T5>(Endpoint sender, T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5);
    public delegate void EndpointAction<in T1, in T2, in T3, in T4, in T5, in T6>(Endpoint sender, T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5, T6 obj6);
}