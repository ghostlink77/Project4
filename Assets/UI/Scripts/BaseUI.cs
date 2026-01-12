using UnityEngine;

public class BaseUI : MonoBehaviour
{    
    public string AnimOnOpen;
    public Animator Anim;

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
        if (AnimOnOpen != null)
        {
            Anim.Play(AnimOnOpen);
        }
    }

    public virtual void Close(bool isCloseAll = false)
    {
        //UIManager.Instance.CloseUI(this);
    }

    public virtual void OnClickCloseButton()
    {
        //AudioManager.Instance.Play();
        Close();
    }

}
