using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShadeCountBar : MonoBehaviour
{
    public Text shadeCountText;

    public void SetShadeCountText(int shadeCount)
    {
        shadeCountText.text = shadeCount.ToString();
    }
}
