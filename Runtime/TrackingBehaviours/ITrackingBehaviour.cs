using System.Reflection;

namespace OmiLAXR.TrackingBehaviours
{
    public interface ITrackingBehaviour
    {
        FieldInfo[] GetTrackingBehaviourEvents();
        Actor GetActor();
        Instructor GetInstructor();
    }
}