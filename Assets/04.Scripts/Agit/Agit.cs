using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Agit : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHP = 250f;
    private float currentHP;

    [SerializeField] float cameraMoveDuration = 0.5f;

    [SerializeField] private PlayableDirector _agitTimeLine;
    [SerializeField] private bool _isWarning;
    [SerializeField] private TimelineAsset _warningTimeline;
    [SerializeField] private TimelineAsset _gameOverTimeline;

    private bool isDestroyed = false;

    void Start()
    {
        currentHP = maxHP;
        isDestroyed = false;
        _agitTimeLine.playableAsset = _warningTimeline;
    }

    public void TakeDamage(float damage)
    {
        if (isDestroyed) return;

        currentHP = Mathf.Max(0, currentHP - damage);
        Debug.Log($"아지트 피해: {damage} | 남은 HP: {currentHP}/{maxHP}");

        InGameManager.Instance.InGameUIController.UpdateAgitHpBar(currentHP, maxHP);
        ShowDamagedAnim();

        if (currentHP <= 0)
        {
            InGameManager.Instance.EndGame();
            StartCoroutine(DestroyAgitCoroutine());
        }
    }

    private void ShowDamagedAnim()
    {
        if (_isWarning) return;
        _agitTimeLine.Play();
        _isWarning = true;
    }

    public void ShowDamagedUIAnim()
    {
        InGameManager.Instance.InGameUIController.ShowMinimapWarning();
        InGameManager.Instance.InGameUIController.PrintMessge("아지트가 공격받고 있습니다.");
    }
    public void EndDamagedAnim()
    {
        _isWarning = false;
        InGameManager.Instance.InGameUIController.HideMinimapWarning();
    }

    public void ShowEndGameAnim()
    {
        _agitTimeLine.playableAsset = _gameOverTimeline;
        _agitTimeLine.Play();
    }

    public void ShowGameOverUI()
    {
        InGameManager.Instance.InGameUIController.ShowEndGameUI();
    }

    IEnumerator DestroyAgitCoroutine()
    {
        isDestroyed = true;

        // 카메라 아지트로 이동
        //yield return StartCoroutine(MoveCameraToAgit());


        // 아지트 파괴 효과
        yield return new WaitForSeconds(1f);

        // 게임 오버 처리
        Debug.Log("Agit Destroyed! Game Over.");
    }

    IEnumerator MoveCameraToAgit()
    {
        Camera mainCamera = Camera.main;
        if(mainCamera == null)
        {
            yield break;
        }

        Vector3 startPos = mainCamera.transform.position;
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, startPos.z);

        float elapsedTime = 0f;
        while (Vector3.Distance(mainCamera.transform.position, targetPos) > 0.1f)
        {
            elapsedTime += Time.deltaTime;
            mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / cameraMoveDuration);
            yield return null;
        }
        mainCamera.transform.position = targetPos;
    }


}
