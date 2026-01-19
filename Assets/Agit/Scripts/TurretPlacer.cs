using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

// 플레이어 오브젝트에 부착되는 터렛 배치 스크립트
public class TurretPlacer : MonoBehaviour
{

    [SerializeField] private LayerMask tileLayer;
    [SerializeField] private TurretData[] turrets = new TurretData[4];

    private Tile currentTile;
    private int selectedTurretIndex = 0;


    void Update()
    {
        if (Keyboard.current.spaceKey.isPressed)
        {
            TryBuildTurret();
        }
    }

    void TryBuildTurret()
    {
        Tile tile = GetTilePlayerPosition();
        if(tile != null && tile.CanPlaceTurret())
        {
            TurretData selectedTurret = turrets[selectedTurretIndex];
            tile.PlaceTurret(selectedTurret.turretPrefab);
        }
    }   

    Tile GetTilePlayerPosition()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position, tileLayer);

        if(hit != null)
        {
            return hit.GetComponent<Tile>();
            Console.WriteLine("Tile found at player position.");
        }
        return null;
    }
}
