using UnityEngine;

public class PassiveStatController : MonoBehaviour, IItemStatController
{
    [SerializeField] private PassiveStatData _passiveStatData;

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
