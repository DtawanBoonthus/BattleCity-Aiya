using UnityEngine;

namespace BC.UI.WorldUI
{
    public class LockWorldUI : MonoBehaviour
    {
        private void LateUpdate()
        {
            transform.rotation = Quaternion.identity;
        }
    }
}