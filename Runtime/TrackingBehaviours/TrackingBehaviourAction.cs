namespace OmiLAXR.TrackingBehaviours
{
    public delegate void TrackingBehaviourAction(ITrackingBehaviour sender);
    public delegate void TrackingBehaviourAction<in T>(ITrackingBehaviour sender, T obj);
    public delegate void TrackingBehaviourAction<in T1, in T2>(ITrackingBehaviour sender, T1 obj1, T2 obj2);
    public delegate void TrackingBehaviourAction<in T1, in T2, in T3>(ITrackingBehaviour sender, T1 obj1, T2 obj2, T3 obj3);
    public delegate void TrackingBehaviourAction<in T1, in T2, in T3, in T4>(ITrackingBehaviour sender, T1 obj1, T2 obj2, T3 obj3, T4 obj4);
    public delegate void TrackingBehaviourAction<in T1, in T2, in T3, in T4, in T5>(ITrackingBehaviour sender, T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5);
    public delegate void TrackingBehaviourAction<in T1, in T2, in T3, in T4, in T5, in T6>(ITrackingBehaviour sender, T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5, T6 obj6);
}