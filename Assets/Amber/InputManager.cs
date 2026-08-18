using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
   public void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Debug.Log("OnMove");
        }
    }
}
