using UnityEngine;

public class EnemyTargetSetter : MonoBehaviour
{
    private Rigidbody2D agit;
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        agit = GameObject.FindGameObjectWithTag("Agit").GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        enemy.SetTarget(agit);
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            enemy.SetTarget(collision.GetComponent<Rigidbody2D>());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            enemy.SetTarget(agit);
        }
    }
}
