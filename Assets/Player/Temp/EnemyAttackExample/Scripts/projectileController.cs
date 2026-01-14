/*
플레이어에게 데미지를 입히기 위한 투사체에 탑재할 스크립트
*/
using Unity.VisualScripting;
using UnityEngine;

public class projectileController : MonoBehaviour
{
    [SerializeField] private int _projDamage;
    
    //목표
    private GameObject _target;
    private PlayerStatController _playerStat;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
        _playerStat = _target.GetComponent<PlayerStatController>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(_target.tag))
        {
            GiveDamage();
            
            // 여기에 데미지 입히는 코드 추가 필요
            Destroy(gameObject);
        }
    }
    
    //플레이어에게 데미지를 주는 함수
    private void GiveDamage()
    {
        _playerStat.GetDamage(_projDamage);
    }
}
