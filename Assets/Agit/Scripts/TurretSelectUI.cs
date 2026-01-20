using UnityEngine;
using UnityEngine.UI;

public class TurretSelectUI : MonoBehaviour
{
    [SerializeField] private TurretPlacer turretPlacer;
    [SerializeField]private Image[] turretIcons = new Image[4];

    void OnEnable()
    {
        SetTurretIcons(turretPlacer.Turrets);
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
            
            turretIcons[i].sprite = turrets[i].icon.sprite;
            turretIcons[i].color = turrets[i].icon.color;
        }
    }
}
