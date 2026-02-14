using UnityEngine;
using UnityEngine.UI;

public class UnScaledTimeWrapper : MonoBehaviour
{
    private Material _material;

    private void Awake()
    {
        _material = GetComponent<Image>().material;
    }

    private void Update()
    {
        _material.SetFloat("_UnscaledTime", Time.unscaledTime);
    }
}
