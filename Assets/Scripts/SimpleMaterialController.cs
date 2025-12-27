using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SimpleMaterialController : MonoBehaviour
{
    public Material mat;
    public Image image;
    public Color[] myColors;

    public void ChangeColor(int colorCode)
    {
        mat.color = myColors[colorCode];
    }
    public void ChangeUIColor(int colorCode)
    {
        image.color = myColors[colorCode];
    }
}
