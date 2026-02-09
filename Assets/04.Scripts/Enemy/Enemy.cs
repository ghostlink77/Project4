using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int damage;
    [SerializeField] private float damageDelay; // 데미지가 들어가는 간격
    private float currentDamageDelay;

    [SerializeField] private float speed;
    [SerializeField] private int maxHp;
    [SerializeField] private string enemyType;
    private int currentHp;
    private bool isLive;

    private EnemySpawner spawner;

    private Rigidbody2D rigid;
    private CapsuleCollider2D collider;
    private Rigidbody2D target;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        spawner = spawner = EnemySpawner.Instance;
        collider = GetComponent<CapsuleCollider2D>();
    }
    private void Update()
    {
        if (isLive) 
        {
            ElapseTime();
        }

    }

    private void FixedUpdate()
    {
        TrackTarget();
        rigid.linearVelocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (target != null && isLive)
        {
            spriteRenderer.flipX = target.position.x < rigid.position.x;
        }
    }

    public void Initialize()
    {
        currentHp = maxHp;
        isLive = true;
        collider.enabled = true;

        animator.Play("Walk", 0, 0f);
    }

    private void TrackTarget()
    {
        if (target != null && isLive)
        {
            Vector2 direction = (target.position - rigid.position).normalized;
            Vector2 moveAmount = direction * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + moveAmount);
        }
    }
    public void SetTarget(Rigidbody2D targetRigidbody)
    {
        target = targetRigidbody;
    }

    // 일정 시간마다 데미지 입히기 위한 시간 경과 처리
    private void ElapseTime()
    {
        if (!isLive)
            return;

        if (currentDamageDelay > 0f)
        {
            currentDamageDelay -= Time.deltaTime;
        }
    }

    // 적과 닿아있으면 지속적으로 데미지 입히기
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(!isLive)
            return;

        if (collision.collider.CompareTag("Agit") || collision.collider.CompareTag("Player"))
        {
            if (currentDamageDelay <= 0f)
            {
                currentDamageDelay = damageDelay;
                collision.collider.GetComponent<IDamageable>()?.TakeDamage(damage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0 && isLive)
        {
            Die();
        }
    }

    private void Die()
    {
        isLive = false;
        collider.enabled = false;
        rigid.linearVelocity = Vector2.zero;
        animator.SetTrigger("Dead");
        // 경험치 오브젝트 드랍
    }

    // 애니메이션이 끝난 후 Animation Event 호출
    public void OnDeathAnimationEnd()
    {
        spawner.ReturnToPool(enemyType, gameObject);
    }
}
