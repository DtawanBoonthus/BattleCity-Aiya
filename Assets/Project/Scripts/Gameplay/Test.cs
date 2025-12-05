using BC.Shared.Inputs;
using UnityEngine;
using VContainer;

namespace BC.Gameplay
{
    public class Test : MonoBehaviour
    {
        [Inject] private IInputProvider input;

        private void Update()
        {
            Debug.Log(input.Move);
        }
    }
}