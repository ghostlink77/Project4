using UnityEngine;

public class EnemyHpController : MonoBehaviour
{
    [SerializeField]
    private int enemyHp = 10;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckDead();
    }
    
    public void GetDamage(int dmg)
    {
        int afterDmgHp = enemyHp - dmg;
        if (afterDmgHp < 0) enemyHp = 0;
        else enemyHp = afterDmgHp;
        Debug.Log($"{dmg}만큼 데미지 입음, 남은 체력: {enemyHp}");
    }
    
    private void CheckDead()
    {
        if (enemyHp <= 0)
        {
            Debug.Log("적 죽음");
            gameObject.SetActive(false);
        }
    }
}
