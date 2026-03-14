using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class BaseUI : MonoBehaviour
{
    public PlayableDirector Director;

    public PlayableAsset FadeInAsset;
    public PlayableAsset FadeOutAsset;


    public virtual void Init(Transform canvas)
    {
        transform.SetParent(canvas.transform);

        var rectTransform = transform as RectTransform;
        rectTransform.localPosition = Vector3.zero;
        rectTransform.localScale = Vector3.one;
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.offsetMin = Vector3.zero;
        rectTransform.offsetMax = Vector3.zero;
    }

    public virtual void Show()
    {
        if (Director != null)
        {
            Director.playableAsset = FadeInAsset;
            Director.Play();
        }
    }

    public virtual void Close(bool isCloseAll = false)
    {
        UIManager.Instance.CloseUI(this);
    }

    public virtual void OnClickCloseButton()
    {
        AudioManager.Instance.Play(AudioType.UISFX, "Button_Click_Close");

        if (Director != null)
        {
            Debug.Log("FadeOUt Ω««‡¡ﬂ..");
            Director.playableAsset = FadeOutAsset;
            Director.Play();
        }
        else
        {
            Close(true);
        }
        

        
    }

}
