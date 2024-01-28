using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 movementVector;
    [SerializeField] Animator playerAnim;

    private void Awake()
    {
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    public void SetSpeedMult(float newFactor)
    {
        rb.velocity = movementVector * newFactor;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("ItemDrop")) return;
        if (other.transform.CompareTag("Player"))
        {
            playerAnim.SetTrigger("Hit");
            KeyManager.Instance.ItemDropped();
        }


        Destroy(gameObject);
    }
}