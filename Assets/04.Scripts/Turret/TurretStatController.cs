using UnityEngine;

public class TurretStatController : MonoBehaviour, IItemStatController
{
    private int level;

    public int GetLevel()
    {
        return level;
    }
}
