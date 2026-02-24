using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform _playerTransform;
    private Vector3 position = Vector2.zero;

    private void Start()
    {
        if (PlayerManager.Instance != null)
            _playerTransform = PlayerManager.Instance.gameObject.transform;
        position.z = transform.position.z;
    }

    private void LateUpdate()
    {
        position.x = _playerTransform.position.x;
        position.y = _playerTransform.position.y;
        transform.position = position;
    }
}
