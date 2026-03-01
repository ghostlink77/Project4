using System.Collections;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform _playerTransform;
    [SerializeField] private Transform _agitTransform;
    private Agit _agit;
    private Vector3 _position = Vector2.zero;

    [SerializeField] private float _cameraMoveDuration = 0.5f;

    private void Start()
    {
        if (PlayerManager.Instance != null)
            _playerTransform = PlayerManager.Instance.gameObject.transform;
        _position.z = transform.position.z;

        InGameManager.Instance.EndGameAction += CameraToAgit;

    }

    private void OnDestroy()
    {
        InGameManager.Instance.EndGameAction -= CameraToAgit;
    }

    private void LateUpdate()
    {
        if (InGameManager.Instance.GameStat == GameStat.Play)
        {
            _position.x = _playerTransform.position.x;
            _position.y = _playerTransform.position.y;
            transform.position = _position;
        }
    }

    private void CameraToAgit()
    {
        if (_agitTransform != null)
            StartCoroutine(CameraToAgitCoroutine());
    }

    private IEnumerator CameraToAgitCoroutine()
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = _agitTransform.position;
        targetPos.z = startPos.z;

        float elapsedTime = 0f;
        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            elapsedTime += Time.unscaledDeltaTime;
            transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / _cameraMoveDuration);
            yield return null;
        }
        transform.position = targetPos;

        if (_agit == null)
        {
            _agit = _agitTransform.gameObject.GetComponent<Agit>();
        }
        _agit.ShowEndGameAnim();
    }
}
