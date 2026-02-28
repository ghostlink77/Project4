using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform _playerTransform;
    [SerializeField] private Transform _agitTransform;
    private Vector3 _position = Vector2.zero;

    [SerializeField] float cameraMoveDuration = 0.5f;

    private void Start()
    {
        if (PlayerManager.Instance != null)
            _playerTransform = PlayerManager.Instance.gameObject.transform;
        _position.z = transform.position.z;

        InGameManager.Instance.EndGameAction += CameraToAgit;
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
            transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / cameraMoveDuration);
            yield return null;
        }
        transform.position = targetPos;

        _agitTransform.gameObject.GetComponent<Agit>().ShowEndGameAnim();
    }
}
