/*
 * 타일 오브젝트 스크립트
 * 타일 상태(포탑 설치 가능 여부)
 * 포탑 설치, 제거 기능
*/
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] bool isPlaceable = true;
    [SerializeField] GameObject currentTurret;

    public bool CanPlaceTurret()
    {
        return isPlaceable;
    }

    public TurretBase PlaceTurret(GameObject turretPrefab)
    {
        if (!CanPlaceTurret()) return null;

        currentTurret = Instantiate(turretPrefab, transform.position, Quaternion.identity);
        currentTurret.transform.parent = this.transform;
        isPlaceable = false;

        return currentTurret.GetComponent<TurretBase>();
    }

    public void RemoveTurret()
    {
        if (currentTurret != null)
        {
            Destroy(currentTurret);
            currentTurret = null;
            isPlaceable = true;
        }
    }

}
