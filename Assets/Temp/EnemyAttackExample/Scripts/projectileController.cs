/*
플레이어에게 데미지를 입히기 위한 투사체에 탑재할 스크립트
*/
using Unity.VisualScripting;
using UnityEngine;

public class projectileController : MonoBehaviour
{
    [SerializeField] private int _projDamage;
    
    public string targetTag = "Player";
    
    //목표
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            GiveDamage();
            Destroy(gameObject);
        }
    }
    
    //플레이어에게 데미지를 주는 함수
    private void GiveDamage()
    {
        if (PlayerManager.Instance == null)
        {
            Debug.LogError("플레이어 매니저 인스턴스 null");
            return;
        }
        PlayerManager.Instance.GetHurt(_projDamage);
    }
}
