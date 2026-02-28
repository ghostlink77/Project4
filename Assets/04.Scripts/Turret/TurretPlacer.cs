/*
 * 플레이어 오브젝트에 부착되는 터렛 배치 스크립트
 * 스페이스바를 눌러 터렛 배치 모드로 진입
 * 마우스 드래그하여 방향에 따라 상하좌우 4방향 중 터렛 선택
*/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class TurretPlacer : MonoBehaviour
{
    [SerializeField] private int _turretLayerIndex;
    [SerializeField] private LayerMask _detectLayer;

    private Tile _currentTile;
    private int _selectedTurretIndex = -1;
    private Vector2 _startMousePos;
    private float _selectThreshold = 10f;

    private bool _isSelecting = false;
    [SerializeField] private int _scrap = 0;

    public Action UseScrap;
    public Action EndPlaceTurret;

    public const string IMPOSSIBLE = "error_btn_sound";

    // 인벤토리에서 가져온 포탑 목록 (최대 4개, 방향 선택 UI에 대응)
    private List<GameObject> _selectableTurretPrefabs = new List<GameObject>();

    void OnEnable()
    {
        PlayerManager.Instance.PlayerEventController.ScrapCollected += CollectScrap;
    }

    void OnDisable()
    {
        PlayerManager.Instance.PlayerEventController.ScrapCollected -= CollectScrap;
    }

    void Update()
    {
        /*if (Keyboard.current.spaceKey.wasPressedThisFrame)
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

#if UNITY_EDITOR
        // T키: _testTurretDatas의 포탑을 순서대로 인벤토리 슬롯에 추가
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            TestAddTurretsToSlot();
        }
#endif*/
    }

    [Header("테스트용")]
    [SerializeField] private TurretData[] _testTurretDatas;

    public void SetTurret(string turretName, Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, _detectLayer);
        if (hit.collider != null)
        {
            Debug.Log($"클릭 좌표 : {position}, 대상 : {hit.collider.name}");

            if (hit.collider.gameObject.TryGetComponent<Tile>(out Tile component))
            {
                GameObject turretPrefab = Resources.Load<GameObject>($"Turret/{turretName}");
                if (turretPrefab.TryGetComponent<TurretBase>(out var turretBasecomponent))
                {
                    if (CheckScrapAmount(turretBasecomponent.GetCost()))
                    {
                        component.PlaceTurret(turretPrefab);
                        UseScrapPoint(turretBasecomponent.GetCost());
                        EndPlaceTurret?.Invoke();
                        return;
                    }
                    else
                    {
                        Debug.Log("스크랩 재화가 부족합니다..");
                        InGameManager.Instance.InGameUIController.PrintMessge("스크랩 재화가 부족합니다.");
                    }
                }
            }
        }
        else
        {
            Debug.Log("현재 클릭 지점에 포탑을 설치할 수 없습니다.");
            InGameManager.Instance.InGameUIController.PrintMessge("현재 클릭 지점에 포탑을 설치할 수 없습니다.");
        }
        AudioManager.Instance.Play(AudioType.SFX, IMPOSSIBLE);
    }

    public bool CheckScrapAmount(int cost)
    {
        return cost <= _scrap;
    }

    private void UseScrapPoint(int cost)
    {
        _scrap -= cost;
        UseScrap?.Invoke();
        InGameManager.Instance.InGameUIController.UpdateScrapAmountText(_scrap);
    }
    public void CollectScrap()
    {
        Debug.Log($"현재 개수 : {_scrap}");
        _scrap++;
        Debug.Log($"고철 획득: {_scrap}");
        InGameManager.Instance.InGameUIController.UpdateScrapAmountText(_scrap);
    }

    public int GetScrapAmount()
    {
        return _scrap;
    }

    private void TrySelect()
    {
        _currentTile = GetTilePlayerPosition();
        if (_currentTile == null || !_currentTile.CanPlaceTurret())
        {
            Debug.Log("No valid tile to place turret.");
            return;
        }

        RefreshSelectableTurrets();

        _startMousePos = Mouse.current.position.ReadValue();
        _isSelecting = true;
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

        if (angle >= 45 && angle < 135)
            _selectedTurretIndex = 0;
        else if (angle >= 135 && angle < 225)
            _selectedTurretIndex = 1;
        else if (angle >= 225 && angle < 315)
            _selectedTurretIndex = 2;
        else
            _selectedTurretIndex = 3;
    }

    private void ConfirmSelect()
    {
        _isSelecting = false;
        if (_selectedTurretIndex != -1)
        {
            TryBuildTurret();
        }
        _selectedTurretIndex = -1;
    }

    private void TryBuildTurret()
    {
        Tile tile = GetTilePlayerPosition();
        if (tile == null || !tile.CanPlaceTurret()) return;

        if (_selectedTurretIndex >= _selectableTurretPrefabs.Count || _selectableTurretPrefabs[_selectedTurretIndex] == null)
        {
            Debug.Log("No turret in selected slot.");
            return;
        }

        GameObject prefab = _selectableTurretPrefabs[_selectedTurretIndex];
        TurretBase turretBase = prefab.GetComponent<TurretBase>();
        if (_scrap < turretBase.TurretData.ScrapCost)
        {
            Debug.Log("Not enough scrap to build turret.");
            return;
        }

        tile.PlaceTurret(prefab);
        _scrap -= turretBase.TurretData.ScrapCost;
    }

    private Tile GetTilePlayerPosition()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position, _turretLayerIndex);
        if (hit != null)
            return hit.GetComponent<Tile>();
        return null;
    }


#if UNITY_EDITOR
    private void TestAddTurretsToSlot()
    {
        if (_testTurretDatas == null || _testTurretDatas.Length == 0)
        {
            Debug.LogWarning("[테스트] Inspector에서 _testTurretDatas를 설정해주세요.");
            return;
        }
        foreach (TurretData data in _testTurretDatas)
        {
            if (data == null) continue;
            PlayerManager.Instance.PlayerItemController.AddItemToSlot<TurretData>(data);
            Debug.Log($"[테스트] 포탑 슬롯에 추가됨: {data.GetName()}");
            RefreshSelectableTurrets();
            Debug.Log($"[테스트] 현재 선택 가능한 포탑 수: {_selectableTurretPrefabs.Count}");
        }
    }
#endif

    private void RefreshSelectableTurrets()
    {
        _selectableTurretPrefabs.Clear();
        Dictionary<string, GameObject> turretSlots =
            PlayerManager.Instance.PlayerItemController.GetSlots<TurretData>();

        if (turretSlots == null) return;

        foreach (GameObject prefab in turretSlots.Values)
        {
            _selectableTurretPrefabs.Add(prefab);
            if (_selectableTurretPrefabs.Count >= 4) break;
        }
    }
}
