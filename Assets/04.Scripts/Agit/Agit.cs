using System.Collections;
using UnityEngine;

public class Agit : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHP = 100;
    private int currentHP;

    [SerializeField] float cameraMoveDuration = 0.5f;


    private bool isDestroyed = false;

    void Start()
    {
        currentHP = maxHP;
        isDestroyed = false;
    }

    public void TakeDamage(int damage)
    {
        if (isDestroyed) return;

        currentHP = Mathf.Max(0, currentHP - damage);
        Debug.Log($"아지트 피해: {damage} | 남은 HP: {currentHP}/{maxHP}");

        if (currentHP <= 0)
        {
            StartCoroutine(DestroyAgitCoroutine());
        }
    }

    IEnumerator DestroyAgitCoroutine()
    {
        isDestroyed = true;

        // 카메라 아지트로 이동
        yield return StartCoroutine(MoveCameraToAgit());

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

    [ContextMenu("Test Take Damage 30")]
    public void TestDamage()
    {
        TakeDamage(30);
    }

}
