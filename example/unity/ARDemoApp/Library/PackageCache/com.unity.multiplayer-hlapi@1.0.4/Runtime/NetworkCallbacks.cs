using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618
namespace UnityEngine.Networking
{
    public class NetworkCallbacks : MonoBehaviour
    {
        void LateUpdate()
        {
            NetworkIdentity.UNetStaticUpdate();
        }
    }
}
#pragma warning restore 618