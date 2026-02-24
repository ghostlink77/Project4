using UnityEngine;
using UnityEngine.Pool;
using TMPro;
using System.Collections;

public class DamageTextSpawner : MonoBehaviour
{
    private IObjectPool<TextMeshProUGUI> _damageTextPool;
    [SerializeField] private GameObject _damageTextPrefab;

    [SerializeField] private int _initSize = 10;
    [SerializeField] private int _maxSize = 50;

    [SerializeField] private float _textAnimSpeed;
    [SerializeField] private Transform _spawnParent;
    private void Awake()
    {
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
        obj.StopAllCoroutines();
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
        if (_spawnParent != null)
            damageTextObj.transform.SetParent(_spawnParent, false);
        damageTextObj.transform.position = position;
        damageTextObj.GetComponent<TextMeshProUGUI>().text = damage.ToString();
        StartCoroutine(ReturnPool(damageTextObj));
    }

    private IEnumerator ReturnPool(TextMeshProUGUI obj)
    {
        Color color = new Color(obj.color.r, obj.color.g, obj.color.b, 1);
        obj.color = color;
        while (obj.color.a > 0)
        {
            color.a -= _textAnimSpeed;
            obj.color = color;
            yield return null;
        }
        _damageTextPool.Release(obj);
    }

}
