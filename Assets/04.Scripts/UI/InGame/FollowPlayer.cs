using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform _playerTransform;
    private Vector3 _position = Vector2.zero;

    private void Start()
    {
        if (PlayerManager.Instance != null)
            _playerTransform = PlayerManager.Instance.gameObject.transform;
        _position.z = transform.position.z;
    }

    private void LateUpdate()
    {
        _position.x = _playerTransform.position.x;
        _position.y = _playerTransform.position.y;
        transform.position = _position;
    }
}
