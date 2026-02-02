using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    private bool isLive;

    private Rigidbody2D rigid;
    private Rigidbody2D target;
    private SpriteRenderer spriteRenderer;


    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        TrackTarget();
        rigid.linearVelocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            spriteRenderer.flipX = target.position.x < rigid.position.x;
        }
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
        isLive = true;
    }
}
