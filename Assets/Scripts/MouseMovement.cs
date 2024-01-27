using UnityEngine;
using UnityEngine.InputSystem;

public class MouseMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;
    [SerializeField] private Camera mainCamera;

    private Vector2 mousePos;
    private Vector2 delta;
    private bool isGamepad;
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        var newPos = transform.position;
        if (isGamepad)
        {
            newPos.x += delta.x * moveSpeed * Time.deltaTime;
        }
        else
        {
            var inWorldPos = mainCamera.ScreenToWorldPoint(mousePos);
            inWorldPos.z = startPos.z;
            inWorldPos.y = startPos.y;
            newPos = Vector3.MoveTowards(transform.position, inWorldPos, moveSpeed);
        }

        newPos.x = Mathf.Clamp(newPos.x, leftLimit.position.x, rightLimit.position.x);

        transform.position = newPos;
    }

    public void DeviceChanged(PlayerInput ctx)
    {
        isGamepad = ctx.currentControlScheme == "Gamepad";
    }

    public void OnMousePosPass(InputAction.CallbackContext ctx)
    {
        mousePos = ctx.ReadValue<Vector2>();
    }

    public void OnLeftStickPass(InputAction.CallbackContext ctx)
    {
        delta = ctx.ReadValue<Vector2>();
        delta.y = 0;
    }
}