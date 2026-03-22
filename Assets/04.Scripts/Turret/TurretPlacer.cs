using System;
using System.Collections.Generic;
using UnityEngine;


public class TurretPlacer : MonoBehaviour
{
    [SerializeField] private int _turretLayerIndex;
    [SerializeField] private LayerMask _detectLayer;

    [SerializeField] private int _scrap = 6;

    public Action UseScrap;
    public Action EndPlaceTurret;

    public const string IMPOSSIBLE = "error_btn_sound";

    private List<GameObject> _selectableTurretPrefabs = new List<GameObject>();

    void OnEnable()
    {
        PlayerManager.Instance.PlayerEventController.ScrapCollected += CollectScrap;
    }

    void OnDisable()
    {
        PlayerManager.Instance.PlayerEventController.ScrapCollected -= CollectScrap;
    }

    private void Start()
    {
        if(InGameManager.Instance.InGameUIController != null)
        {
            InGameManager.Instance.InGameUIController.UpdateScrapAmountText(_scrap); 
        }
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
                        TurretBase placedTurret = component.PlaceTurret(turretPrefab);
                        if (placedTurret != null)
                        {
                            int currentLevel = TurretManager.Instance.GetTurretLevel(turretName);
                            placedTurret.Initialize(currentLevel);
                            TurretManager.Instance.RegisterPlacedTurret(turretName, placedTurret);
                        }
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
        AudioManager.Instance.Play(AudioType.UISFX, IMPOSSIBLE);
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
