using NUnit.Framework.Constraints;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [Header("References")]
    public BoxCollider2D _mapReference;
    public Transform _playerTransform;

    [Header("References - UI")]
    public RectTransform _playerIndicator;

    [Header("Parameters")]
    public Vector2 _mapTextureSize = new Vector2(1024, 1024);
    public Bounds _mapBounds;

    [Header("Player Options")]
    public float minimapScale = 1.0f;

    private void Awake()
    {
        if (_mapReference)
        {
            _mapReference.gameObject.SetActive(true);
            _mapBounds = _mapReference.bounds;
            _mapReference.gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        Transform positionReference = _playerTransform;

        Vector2 unitScale = new Vector2(_mapTextureSize.x / _mapBounds.size.x,
            _mapTextureSize.y / _mapBounds.size.y);
        Vector3 positionOffset = _mapBounds.center - positionReference.position;

        Vector2 mapPosition = new Vector2(positionOffset.x * unitScale.x * -1,
            positionOffset.y * unitScale.y * -1) *minimapScale;

        _playerIndicator.localPosition = mapPosition;

    }

}
