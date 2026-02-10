/*
 * 적 타겟 설정 스크립트
 * 기본적으로 적은 아지트를 타겟으로 설정
 * 적 시야 안에 플레이어가 들어오면 플레이어를 타겟으로 설정
 */
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
