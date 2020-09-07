using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSphereColor : MonoBehaviour
{
    Material material;
    Color currentColor;
    readonly string colorPropertyName = "Color_B18FBE4B";

    public void ChangeColor(Color colorTo)
    {
        material.SetColor(colorPropertyName, colorTo);
    }

    void GetColor()
    {
        material = GetComponent<Renderer>().material;
        currentColor = material.GetColor(colorPropertyName);
    }

    // Start is called before the first frame update
    void Awake()
    {
        GetColor();
    }

}
