using UnityEngine;
using UnityEngine.Pool;
using TMPro;
using System.Collections;

public class DamageTextSpawner : MonoBehaviour
{
    private IObjectPool<GameObject> _damageTextPool;
    [SerializeField] private GameObject _damageTextPrefab;
    [SerializeField] private Transform _parentTransform;

    [SerializeField] private int _initSize = 10;
    [SerializeField] private int _maxSize = 50;

    [SerializeField] private Camera _uiCamera;

    [SerializeField] private float _textAnimSpeed;
    private void Awake()
    {
        CreatePools();
    }

    private void CreatePools()
    {
        var pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_damageTextPrefab),
            actionOnGet: ActivateText,
            actionOnRelease: DisableText,
            collectionCheck: false,
            defaultCapacity: _initSize,
            maxSize: _maxSize);
        _damageTextPool = pool;
    }

    private void ActivateText(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void DisableText(GameObject obj)
    {
        obj.SetActive(false);
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
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(position);
        Vector3 uiWorldPos = _uiCamera.ViewportToWorldPoint(new Vector3(viewportPoint.x, viewportPoint.y, 100));
        damageTextObj.transform.position = uiWorldPos;

        damageTextObj.GetComponent<TextMeshProUGUI>().text = damage.ToString();
        StartCoroutine(ReturnPool(damageTextObj));
    }

    private IEnumerator ReturnPool(GameObject obj)
    {
        TextMeshProUGUI text = obj.GetComponent<TextMeshProUGUI>();

        float alpha = 1;
        while (text.color.a > 0)
        {
            alpha -= _textAnimSpeed;
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            yield return null;
        }
        _damageTextPool.Release(obj);
    }

}
