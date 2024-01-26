using UnityEngine;

public class KeyController : MonoBehaviour
{
    [SerializeField] private Vector3 movementSpeed;
    [field: SerializeField] public Transform selfRef { get; set; }
    [field: SerializeField] public KeyValue value { get; set; }
    [SerializeField] private Transform leftBoundary;
    [SerializeField] private Transform rightBoundary;
    [SerializeField] private SpriteRenderer spriteRef;
    [SerializeField] private Sprite[] icons;

    public KeyState State { get; set; } = KeyState.Waiting;
    public float MovementFactor { get; set; }

    public void SetValue(KeyValue newVal)
    {
        value = newVal;
        spriteRef.sprite = icons[(int)value];
    }

    public void Move()
    {
        selfRef.Translate(movementSpeed * (Time.deltaTime * MovementFactor));
    }

    public void Perfect()
    {
        spriteRef.color = Color.green;
        State = KeyState.Perfect;
    }

    public void CanBePressed()
    {
        spriteRef.color = Color.blue;
        State = KeyState.Passable;
    }

    public void TooLate()
    {
        spriteRef.color = Color.red;
        State = KeyState.Late;
    }
}


public enum KeyValue
{
    Up,
    Down,
    Left,
    Right
}

public enum KeyState
{
    Waiting,
    Passable,
    Perfect,
    Late
}