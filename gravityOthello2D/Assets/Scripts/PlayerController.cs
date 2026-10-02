using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference mouseInput;

    private float cameraDistance = 10; 

    public void FixedUpdate()
    {
        Vector2 value = mouseInput.action.ReadValue<Vector2>();
        Vector3 vector3 = new Vector3(value.x, value.y, cameraDistance);
        transform.position = Camera.main.ScreenToWorldPoint(vector3);
    }
}
