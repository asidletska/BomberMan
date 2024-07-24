using UnityEngine;

public class ComputerPlayer : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }
    private Vector2 direction = Vector2.down;
    public float speed = 5f;
    public float bombCooldown = 2f;
    private float bombTimer;

    public AnimatedSprite spriteRendererUp;
    public AnimatedSprite spriteRendererDown;
    public AnimatedSprite spriteRendererLeft;
    public AnimatedSprite spriteRendererRight;
    public AnimatedSprite spriteRendererDeath;
    private AnimatedSprite activeSprite;

    private Transform player;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        activeSprite = spriteRendererDown;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bombTimer = bombCooldown;
    }

    private void Update()
    {
        if (player != null)
        {
            Vector2 playerPosition = player.position;
            Vector2 currentPosition = rigidbody.position;

            Vector2 directionToPlayer = playerPosition - currentPosition;
            if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.y))
            {
                direction = directionToPlayer.x > 0 ? Vector2.right : Vector2.left;
                SetDirection(direction, direction == Vector2.right ? spriteRendererRight : spriteRendererLeft);
            }
            else
            {
                direction = directionToPlayer.y > 0 ? Vector2.up : Vector2.down;
                SetDirection(direction, direction == Vector2.up ? spriteRendererUp : spriteRendererDown);
            }

            bombTimer -= Time.deltaTime;
            if (bombTimer <= 0)
            {
                PlaceBomb();
                bombTimer = bombCooldown;
            }
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

    private void PlaceBomb()
    {
        // ֿונוהבאקא÷עüסÿ, שמ BombController לא÷ לועמה הכÿ נמחל³שוםםÿ במלבט
        GetComponent<BombController>().PlaceBomb();
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

