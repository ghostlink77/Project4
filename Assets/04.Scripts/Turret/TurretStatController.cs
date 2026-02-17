using UnityEngine;

public class TurretStatController : MonoBehaviour, IItemStatController
{
    private int level = 1;

    public int GetLevel()
    {
        return level;
    }

    public void LevelUp()
    {
        level++;
    }
}
