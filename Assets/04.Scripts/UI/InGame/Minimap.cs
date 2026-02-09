using NUnit.Framework.Constraints;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BoxCollider2D _mapReference;
    [SerializeField] private Transform _playerTransform;

    [Header("References - UI")]
    [SerializeField] private RectTransform _playerIndicator;

    [Header("Parameters")]
    [SerializeField] private Vector2 _mapTextureSize = new Vector2(1024, 1024);
    [SerializeField] private Bounds _mapBounds;

    [Header("Player Options")]
    [SerializeField] private float minimapScale = 1.0f;

    Vector2 _unitScale;
    Vector2 _mapPosition = Vector2.zero;

    private void Awake()
    {
        if (_mapReference)
        {
            _mapReference.gameObject.SetActive(true);
            _mapBounds = _mapReference.bounds;
            _mapReference.gameObject.SetActive(false);

            _unitScale = new Vector2(_mapTextureSize.x / _mapBounds.size.x,
            _mapTextureSize.y / _mapBounds.size.y);
        }
    }

    private void LateUpdate()
    {
        if (_playerTransform == null || _playerIndicator == null) return;

        Transform positionReference = _playerTransform;
        Vector3 positionOffset = _mapBounds.center - positionReference.position;

        _mapPosition.x = positionOffset.x * _unitScale.x * -1 * minimapScale;
        _mapPosition.y = positionOffset.y * _unitScale.y * -1 * minimapScale;

        _playerIndicator.localPosition = _mapPosition;

    }

}
