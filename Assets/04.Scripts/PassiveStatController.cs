using UnityEngine;

public class PassiveStatController : MonoBehaviour, IItemStatController
{
    [SerializeField] private TurretStatData _turretStatData;

    private int level;
    public int GetLevel()
    {
        return 1;
    }
}
