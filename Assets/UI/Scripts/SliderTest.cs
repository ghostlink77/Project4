using UnityEngine;
using UnityEngine.UI;

public class SliderTest : MonoBehaviour
{
    public float Volume = 100;
    public float VolumeMax = 100;
    public Slider slider;


    private void Update()
    {
        slider.value = Volume / VolumeMax;
        Debug.Log(slider.value);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            Volume += 1;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            Volume -= 1;
    }


}
