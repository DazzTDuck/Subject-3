using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignWave : MonoBehaviour
{
    Vector2 floatY;
    float originalY;
    public float strength = 0.3f;

    void Start()
    {
        originalY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        var floatY = transform.position;
        floatY.y = originalY + (Mathf.Sin(Time.time) * strength);
        transform.position = floatY;
    }
}
