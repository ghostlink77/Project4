/*
 * 포탑 건설 선택 UI
 * UI오브젝트에 부착
 */

using UnityEngine;
using UnityEngine.UI;

public class TurretSelectUI : MonoBehaviour
{
    [SerializeField] private TurretPlacer turretPlacer;
    [SerializeField]private Image[] turretIcons = new Image[4];

    void OnEnable()
    {
    }

    public void Show(Vector3 pos)
    {
        gameObject.SetActive(true);
        transform.position = pos;

    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void SetTurretIcons(TurretData[] turrets)
    {
        for (int i = 0; i < turretIcons.Length; i++)
        {
            if(turrets[i] == null)
            {
                turretIcons[i].sprite = null;
                turretIcons[i].color = new Color(0,0,0,0);
                continue;
            }
            
        }
    }
}
