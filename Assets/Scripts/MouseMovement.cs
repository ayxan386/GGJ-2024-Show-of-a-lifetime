using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 15f;
    private float spriteWidth;
    private float spriteHeight;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        spriteWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
        spriteHeight = GetComponent<SpriteRenderer>().bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        mousePosition.y = 0f;
        Vector3 clampedPosition = new Vector3(
            Mathf.Clamp(mousePosition.x, Camera.main.ScreenToWorldPoint(Vector3.zero).x + spriteWidth,
                        Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x - spriteWidth),
            Mathf.Clamp(mousePosition.y, Camera.main.ScreenToWorldPoint(Vector3.zero).y + spriteHeight,
                        Camera.main.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f)).y - spriteHeight),
            0f
        );
        transform.position = Vector3.MoveTowards(transform.position, clampedPosition, moveSpeed * Time.deltaTime);


    }

}
