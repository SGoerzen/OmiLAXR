namespace OmiLAXR.Data
{
    public abstract class BindedStatementComposer<T> : StatementComposer 
        where T : TrackingBehaviour
    {
        public T trackingBehaviour;

        protected virtual void Awake()
        {
            trackingBehaviour = GetTrackingBehaviour<T>();
        }
    }
}