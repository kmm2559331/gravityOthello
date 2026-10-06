using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference mouseInput;
    [SerializeField] private InputActionReference mouseButtonInput;

    [SerializeField] private GameObject _mouse;
    private Vector3 mousePos;

    private int Black = 255;
    private int White = 0;

    private float cameraDistance = 10;

    private FieldGenerator FieldGenerator;
    [SerializeField] private GameObject GameObject;

    public void Start()
    {
        //_mouse = this.GetComponent<GameObject>();
        //FieldGenerator = new FieldGenerator();
        FieldGenerator = GameObject.GetComponent<FieldGenerator>();
    }

    public void OnEnable()
    {
        mouseButtonInput.action.performed += StoneDrop;

        mouseButtonInput.action.Enable();
    }

    public void OnDisable()
    {
        mouseButtonInput.action.performed -= StoneDrop;

        mouseButtonInput.action.Disable();
    }

    public void StoneDrop(InputAction.CallbackContext ctx)
    {
        if(ctx.phase != InputActionPhase.Performed) return;

        mousePos = _mouse.transform.position;
        int pos = (int)MathF.Floor(mousePos.x);
        if (pos < 0 || pos >7) return;

        StoneState state = FieldGenerator._turn;

        FieldGenerator.FallStone(pos, state);

        //_mouse = this.GameObject.GetComponent<Renderer>.material.color;
    }

    public void FixedUpdate()
    {
        Vector2 value = mouseInput.action.ReadValue<Vector2>();
        Vector3 vector3 = new Vector3(value.x, value.y, cameraDistance);
        transform.position = Camera.main.ScreenToWorldPoint(vector3);
    }
}
