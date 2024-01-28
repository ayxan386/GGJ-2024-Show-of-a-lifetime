using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 movementVector;
    [SerializeField] private Sprite[] possibleLooks;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] Animator playerAnim;

    private void Awake()
    {
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        renderer.sprite = possibleLooks[Random.Range(0, possibleLooks.Length)];
    }

    public void SetSpeedMult(float newFactor)
    {
        rb.velocity = movementVector * newFactor;
        rb.angularVelocity = 45;
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