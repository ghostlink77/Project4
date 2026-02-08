using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSoundData", menuName = "Scriptable Objects/WeaponSoundData")]
public class WeaponSoundData : ScriptableObject
{
    [Header("총기 발사 소리")]
    [SerializeField]
    private AudioClip shootSound;
    public AudioClip ShootSound {get => shootSound; private set => shootSound = value;}
    
    [Header("총알 타격 소리")]
    [SerializeField]
    private AudioClip bulletHitSound;
    public AudioClip BulletHitSound {get => bulletHitSound; private set => bulletHitSound = value;}
}
