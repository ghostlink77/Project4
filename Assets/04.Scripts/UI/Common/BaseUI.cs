using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class BaseUI : MonoBehaviour
{
    public PlayableDirector FadeIn;
    public PlayableDirector FadeOut;


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
        if (FadeIn != null)
        {
            FadeIn.Play();
        }
    }

    public virtual void Close(bool isCloseAll = false)
    {
        UIManager.Instance.CloseUI(this);
    }

    public virtual void OnClickCloseButton()
    {
        if (FadeOut != null)
        {
            Debug.Log("FadeOUt Ω««‡¡ﬂ..");
            FadeOut.Play();
        }
        AudioManager.Instance.Play(AudioType.SFX, "Button_Click_Close");
        Close(true);
        
    }

}
