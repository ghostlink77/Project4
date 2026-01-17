/*
플레이어의 하위 오브젝트로 생성된 무기의 월드 좌표, 
*/
using UnityEngine;

public class GetEnemyAngle : MonoBehaviour
{
    private Vector2 playerPos;
    private Vector2 targetPos;
    private GameObject targetObj;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetPlayerPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void GetPlayerPosition()
    {
        playerPos = transform.position;
    }
    
    void GetEnemyPosition()
    {
        // 타겟인 적의 게임 오브젝트 할당
        
        // 변수에 가져오기
        targetPos = targetObj.transform.position;
    }
    
    // 무기에서 총알이 나가야 하는 방향 계산
    public Vector2 GetShootDirection()
    {
        GetPlayerPosition(); GetEnemyPosition();
        return targetPos - playerPos;
    }
}
