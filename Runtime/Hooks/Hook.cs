using OmiLAXR.Composers;
using UnityEngine;

namespace OmiLAXR.Hooks
{
    public abstract class Hook : MonoBehaviour
    {
        public abstract IStatement AfterCompose(IStatement statement);
    }
}