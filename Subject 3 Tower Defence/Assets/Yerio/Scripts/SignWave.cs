using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignWave : MonoBehaviour
{
    Vector2 floatY;
    float originalY;
    public float strength = 0.08f;
    public float timeStrength = 2;

    float time = 0;

    void Start()
    {
        originalY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        var floatY = transform.position;
        floatY.y = originalY + (Mathf.Sin(time * timeStrength) * strength);
        transform.position = floatY;
    }
}
