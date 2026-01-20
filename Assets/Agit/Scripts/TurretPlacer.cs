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
    private int selectedTurretIndex = -1;
    private Vector2 startMousePos;
    private float selectThreshold = 5f;

    private bool isSelecting = false;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TrySelect();
            //TryBuildTurret();
        }

        if (isSelecting)
        {
            UpdateMouseDirection();
        }

        if (Keyboard.current.spaceKey.wasReleasedThisFrame && isSelecting)
        {
            ConfirmSelect();
        }
    }

    void TrySelect()
    {
        startMousePos = Mouse.current.position.ReadValue();
        isSelecting = true;
    }
    void UpdateMouseDirection()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 delta = mousePos - startMousePos;

        if (delta.magnitude < selectThreshold)
        {
            selectedTurretIndex = -1;
            return;
        }

        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;
        
        if(angle >= 45 && angle < 135)
        {
            selectedTurretIndex = 0; // Up
        }
        else if(angle >= 135 && angle < 225)
        {
            selectedTurretIndex = 1; // Left
        }
        else if(angle >= 225 && angle < 315)
        {
            selectedTurretIndex = 2; // Down
        }
        else
        {
            selectedTurretIndex = 3; // Right
        }
    }

    void ConfirmSelect()
    {
        isSelecting = false;
        if(selectedTurretIndex != -1)
        {
            TryBuildTurret();
        }
        selectedTurretIndex = -1;
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
            Debug.Log("Tile found at player position.");
        }
        return null;
    }
}
