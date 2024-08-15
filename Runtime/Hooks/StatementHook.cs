using OmiLAXR.Composers;
using UnityEngine;

namespace OmiLAXR.Hooks
{
    public abstract class StatementHook : MonoBehaviour
    {
        public abstract IStatement AfterCompose(IStatement statement);
    }
}