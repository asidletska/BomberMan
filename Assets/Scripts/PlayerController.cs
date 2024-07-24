using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }
    private Vector2 direction = Vector2.down;
    public float speed = 5f;
    public KeyCode inputUp = KeyCode.W;
    public KeyCode inputDown = KeyCode.S;
    public KeyCode inputLeft = KeyCode.A;
    public KeyCode inputRight = KeyCode.D;
    public KeyCode inputBomb = KeyCode.Space;

    public AnimatedSprite spriteRendererUp;
    public AnimatedSprite spriteRendererDown;
    public AnimatedSprite spriteRendererLeft;
    public AnimatedSprite spriteRendererRight;
    public AnimatedSprite spriteRendererDeath;
    private AnimatedSprite activeSprite;

    private bool bombPlaced;

    private BombController bombController;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        activeSprite = spriteRendererDown;
        bombController = GetComponent<BombController>();
    }

    private void Update()
    {
        bombPlaced = false;

        if (Input.GetKey(inputUp))
        {
            SetDirection(Vector2.up, spriteRendererUp);
        }
        else if (Input.GetKey(inputDown))
        {
            SetDirection(Vector2.down, spriteRendererDown);
        }
        else if (Input.GetKey(inputLeft))
        {
            SetDirection(Vector2.left, spriteRendererLeft);
        }
        else if (Input.GetKey(inputRight))
        {
            SetDirection(Vector2.right, spriteRendererRight);
        }
        else
        {
            SetDirection(Vector2.zero, activeSprite);
        }

        if (Input.GetKeyDown(inputBomb))
        {
            bombController.PlaceBomb();
            bombPlaced = true;
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        Vector2 translation = direction * speed * Time.fixedDeltaTime;
        rigidbody.MovePosition(position + translation);
    }

    private void SetDirection(Vector2 newDirection, AnimatedSprite spriteRenderer)
    {
        direction = newDirection;

        spriteRendererUp.enabled = spriteRenderer == spriteRendererUp;
        spriteRendererDown.enabled = spriteRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = spriteRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = spriteRenderer == spriteRendererRight;

        activeSprite = spriteRenderer;
        activeSprite.idle = direction == Vector2.zero;
    }

    public Vector2 GetCurrentDirection()
    {
        return direction;
    }

    public bool HasPlacedBomb()
    {
        return bombPlaced;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            DeathSequence();
        }
    }

    private void DeathSequence()
    {
        enabled = false;
        GetComponent<BombController>().enabled = false;

        spriteRendererUp.enabled = false;
        spriteRendererDown.enabled = false;
        spriteRendererLeft.enabled = false;
        spriteRendererRight.enabled = false;
        spriteRendererDeath.enabled = true;

        Invoke(nameof(OnDeathSequenceEnded), 1.25f);
    }

    private void OnDeathSequenceEnded()
    {
        gameObject.SetActive(false);
        FindObjectOfType<GameManager>().CheckWinState();
    }
}
