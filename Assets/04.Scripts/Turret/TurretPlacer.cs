/*
 * 플레이어 오브젝트에 부착되는 터렛 배치 스크립트
 * 스페이스바를 눌러 터렛 배치 모드로 진입
 * 마우스 드래그하여 방향에 따라 상하좌우 4방향 중 터렛 선택
*/

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;


public class TurretPlacer : MonoBehaviour
{

    [SerializeField] private LayerMask tileLayer;
    [SerializeField] private TurretData[] turrets = new TurretData[4];

    public TurretData[] Turrets { get { return turrets; } }

    private Tile currentTile;
    private int selectedTurretIndex = -1;
    private Vector2 startMousePos;
    private float selectThreshold = 10f;

    private bool isSelecting = false;

    [Header("UI")]
    [SerializeField] private TurretSelectUI turretSelectUI;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TrySelect();
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
        currentTile = GetTilePlayerPosition();
        if (currentTile == null || !currentTile.CanPlaceTurret())
        {
            Debug.Log("No valid tile to place turret.");
            return;
        }

        GetPlayerTurret();

        startMousePos = Mouse.current.position.ReadValue();
        isSelecting = true;
        turretSelectUI?.Show(startMousePos);
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
        turretSelectUI?.Hide();
    }

    void TryBuildTurret()
    {
        Tile tile = GetTilePlayerPosition();
        if(tile != null && tile.CanPlaceTurret())
        {
            if(turrets[selectedTurretIndex] == null)
            {
                Debug.Log("No turret in selected slot.");
                return;
            }
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
        }
        return null;
    }
    void GetPlayerTurret()
    {
        // TODO: 추후 플레이어가 가진 포탑 정보를 가져오는 로직 추가
    }
}
