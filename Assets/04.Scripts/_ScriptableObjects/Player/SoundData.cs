using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "Scriptable Objects/SoundData")]
public class SoundData : ScriptableObject
{
    public AudioClip[] clips;
    
    public AudioClip GetClip(int position)
    {
        // 에러 방지를 위해 오디오 클립을 출력할 수 없는 경우 null 반환
        if (clips.Length == 0)
        {
            Debug.Log("오디오 클립이 이 스크립터블 오브젝트에 없습니다.");
            return null;
        }
        else if (position < 0 || position >= clips.Length)
        {
            Debug.Log($"{position} 번째 오디오 클립은 존재하지 않습니다.");
            return null;
        }

        return clips[position];
    }
}
