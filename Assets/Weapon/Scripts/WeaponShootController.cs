/*
무기의 사격을 관리하는 스크립트
*/
using UnityEngine;

public class WeaponShootController : MonoBehaviour
{
    PlayerStatController playerStat;

    void Start()
    {
        playerStat = GetComponent<PlayerStatController>();
    }

    void Update()
    {
        
    }
}
