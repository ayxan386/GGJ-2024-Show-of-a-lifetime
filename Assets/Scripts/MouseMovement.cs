using UnityEngine;
using UnityEngine.InputSystem;

public class MouseMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float mouseMoveSpeed = 5f;
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;
    [SerializeField] private Camera mainCamera;

    private Vector2 mousePos;
    private Vector2 delta;
    private bool isGamepad ;
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        var newPos = transform.position;
        Vector3 mouseWorldPos = GetMouseWorldPosition();

        newPos.x += delta.x * Time.deltaTime * (isGamepad ? moveSpeed : mouseMoveSpeed);
        newPos.x = Mathf.Clamp(newPos.x, leftLimit.position.x, rightLimit.position.x);

        transform.position = newPos;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseWorldPos = Vector3.zero;

        if (mainCamera != null)
        {
            mousePos = Mouse.current.position.ReadValue();
            mouseWorldPos = mainCamera.ScreenToWorldPoint(mousePos);
            mouseWorldPos.y = startPos.y;
            mouseWorldPos.z = startPos.z;
        }
        else
        {
            Debug.LogWarning("Main camera reference not set!");
        }

        return mouseWorldPos;
    }

    public void DeviceChanged(PlayerInput ctx)
    {
        isGamepad = ctx.currentControlScheme == "Gamepad";
    }

    public void OnMousePosPass(InputAction.CallbackContext ctx)
    {
        mousePos = ctx.ReadValue<Vector2>();
        Debug.Log(mousePos);
    }

    public void OnLeftStickPass(InputAction.CallbackContext ctx)
    {
        delta = ctx.ReadValue<Vector2>();
        delta.y = 0;
    }
}