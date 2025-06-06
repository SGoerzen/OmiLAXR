namespace OmiLAXR.Endpoints
{
    public delegate void EndpointAction(IEndpoint sender);
    public delegate void EndpointAction<in T>(IEndpoint sender, T obj);
    public delegate void EndpointAction<in T1, in T2>(IEndpoint sender, T1 obj1, T2 obj2);
    public delegate void EndpointAction<in T1, in T2, in T3>(IEndpoint sender, T1 obj1, T2 obj2, T3 obj3);
    public delegate void EndpointAction<in T1, in T2, in T3, in T4>(IEndpoint sender, T1 obj1, T2 obj2, T3 obj3, T4 obj4);
    public delegate void EndpointAction<in T1, in T2, in T3, in T4, in T5>(IEndpoint sender, T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5);
    public delegate void EndpointAction<in T1, in T2, in T3, in T4, in T5, in T6>(IEndpoint sender, T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5, T6 obj6);
}