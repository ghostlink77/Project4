using Unity.VisualScripting;
using UnityEngine;

public class projectileController : MonoBehaviour
{
    [SerializeField] private string atkTargetTag;
    [SerializeField] private int projDamage;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(atkTargetTag))
        {
            Debug.Log("플레이어 감지됨");
            // 여기에 데미지 입히는 코드 추가 필요
            Destroy(gameObject);
        }
    }
}
