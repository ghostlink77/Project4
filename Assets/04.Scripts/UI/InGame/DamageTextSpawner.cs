using UnityEngine;
using UnityEngine.Pool;
using TMPro;
using System.Collections;

public class DamageTextSpawner : MonoBehaviour
{
    private IObjectPool<TextMeshProUGUI> _damageTextPool;
    [SerializeField] private GameObject _damageTextPrefab;
    [SerializeField] private Transform _parentTransform;

    [SerializeField] private int _initSize = 10;
    [SerializeField] private int _maxSize = 50;

    [SerializeField] private Camera _uiCamera;
    private Camera _mainCamera;

    [SerializeField] private float _textAnimSpeed;
    private void Awake()
    {
        _mainCamera = Camera.main;
        CreatePools();
    }

    private void CreatePools()
    {
        TextMeshProUGUI textObj = _damageTextPrefab.GetComponent<TextMeshProUGUI>();
        var pool = new ObjectPool<TextMeshProUGUI>(
            createFunc: () => Instantiate(textObj),
            actionOnGet: ActivateText,
            actionOnRelease: DisableText,
            collectionCheck: false,
            defaultCapacity: _initSize,
            maxSize: _maxSize);
        _damageTextPool = pool;
    }

    private void ActivateText(TextMeshProUGUI obj)
    {
        obj.gameObject.SetActive(true);
    }

    private void DisableText(TextMeshProUGUI obj)
    {
        obj.gameObject.SetActive(false);
    }

    public void ShowDamageText(float damage, Vector3 position)
    {
        if (_damageTextPool == null)
        {
            Debug.Log("Damage Text Pools is nothing.");
            return;
        }

        var damageTextObj = _damageTextPool.Get();

        damageTextObj.transform.SetParent(_parentTransform, false);
        Vector3 viewportPoint = _mainCamera.WorldToViewportPoint(position);
        Vector3 uiWorldPos = _uiCamera.ViewportToWorldPoint(new Vector3(viewportPoint.x, viewportPoint.y, 100));
        damageTextObj.transform.position = uiWorldPos;

        damageTextObj.GetComponent<TextMeshProUGUI>().text = damage.ToString();
        StartCoroutine(ReturnPool(damageTextObj));
    }

    private IEnumerator ReturnPool(TextMeshProUGUI obj)
    {
        //TextMeshProUGUI text = obj.GetComponent<TextMeshProUGUI>();

        Color color = new Color(obj.color.r, obj.color.g, obj.color.b, 1);
        while (obj.color.a > 0)
        {
            color.a -= _textAnimSpeed;
            obj.color = color;
            yield return null;
        }
        _damageTextPool.Release(obj);
    }

}
