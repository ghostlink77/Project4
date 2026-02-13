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
    [SerializeField] private LayerMask _tileLayer;
    [SerializeField] private TurretData[] _turrets = new TurretData[4];

    public TurretData[] Turrets { get { return _turrets; } }

    private Tile _currentTile;
    private int _selectedTurretIndex = -1;
    private Vector2 _startMousePos;
    private float _selectThreshold = 10f;

    private bool _isSelecting = false;

    private int _scrap = 0;

    [Header("UI")]
    [SerializeField] private TurretSelectUI _turretSelectUI;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TrySelect();
        }

        if (_isSelecting)
        {
            UpdateMouseDirection();
        }

        if (Keyboard.current.spaceKey.wasReleasedThisFrame && _isSelecting)
        {
            ConfirmSelect();
        }
    }

    public void CollectScrap()
    {
        _scrap++;
        Debug.Log($"고철 획득: {_scrap}");
    }

    private void TrySelect()
    {
        _currentTile = GetTilePlayerPosition();
        if (_currentTile == null || !_currentTile.CanPlaceTurret())
        {
            Debug.Log("No valid tile to place turret.");
            return;
        }

        GetPlayerTurret();

        _startMousePos = Mouse.current.position.ReadValue();
        _isSelecting = true;
        _turretSelectUI?.Show(_startMousePos);
    }
    private void UpdateMouseDirection()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 delta = mousePos - _startMousePos;

        if (delta.magnitude < _selectThreshold)
        {
            _selectedTurretIndex = -1;
            return;
        }

        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;
        
        if(angle >= 45 && angle < 135)
        {
            _selectedTurretIndex = 0;
        }
        else if(angle >= 135 && angle < 225)
        {
            _selectedTurretIndex = 1;
        }
        else if(angle >= 225 && angle < 315)
        {
            _selectedTurretIndex = 2;
        }
        else
        {
            _selectedTurretIndex = 3;
        }
    }

    private void ConfirmSelect()
    {
        _isSelecting = false;
        if(_selectedTurretIndex != -1)
        {
            TryBuildTurret();
        }
        _selectedTurretIndex = -1;
        _turretSelectUI?.Hide();
    }

    private void TryBuildTurret()
    {
        Tile tile = GetTilePlayerPosition();
        if(tile != null && tile.CanPlaceTurret())
        {
            if(_turrets[_selectedTurretIndex] == null)
            {
                Debug.Log("No turret in selected slot.");
                return;
            }
            if(_scrap < _turrets[_selectedTurretIndex].scrapCost)
            {
                Debug.Log("Not enough scrap to build turret.");
                return;
            }
            TurretData selectedTurret = _turrets[_selectedTurretIndex];
            tile.PlaceTurret(selectedTurret.turretPrefab);
            _scrap -= selectedTurret.scrapCost;
        }
    }   

    private Tile GetTilePlayerPosition()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position, _tileLayer);

        if(hit != null)
        {
            return hit.GetComponent<Tile>();
        }
        return null;
    }
    private void GetPlayerTurret()
    {
        // TODO: 추후 플레이어가 가진 포탑 정보를 가져오는 로직 추가
    }
}
