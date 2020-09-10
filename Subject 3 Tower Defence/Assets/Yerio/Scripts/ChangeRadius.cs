using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRadius : MonoBehaviour
{
    Material material;
    string scalePropertyName = "Vector1_A81039F6";

    public void SetScale(float scale)
    {
        material.SetFloat(scalePropertyName, scale);
    }

    void GetMaterial()
    {
        material = GetComponent<Renderer>().material;
    }

    private void Awake()
    {
        GetMaterial();
    }
}
