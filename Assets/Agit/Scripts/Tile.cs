using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] bool isPlaceable = true;
    [SerializeField] GameObject currentTurret;

    public bool CanPlaceTurret()
    {
        return isPlaceable;
    }

    public void PlaceTurret(GameObject turretPrefab)
    {
        if (!CanPlaceTurret()) return;

        currentTurret = Instantiate(turretPrefab, transform.position, Quaternion.identity);
        currentTurret.transform.parent = this.transform;
        isPlaceable = false;
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
