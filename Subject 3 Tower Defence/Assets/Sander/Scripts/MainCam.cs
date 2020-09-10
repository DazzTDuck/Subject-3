using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCam : MonoBehaviour
{
    public float camRotX;
    public float camRotY;

    Camera mainCam;
    public Vector3 camOffset;
    public GameObject camFocusObject;

    public float minZoom = 20;
    public float maxZoom = 60;
    public float zoomSpeed = 20;
    float zoomValue = 60;


    public GameObject focusGun;
    public Vector3 focusGunOffset;
    public bool isGunFocus;
    public float gunZoom = 35;
    
    

    [HideInInspector]
    public bool isZoomed;
    
    

    // Start is called before the first frame update
    void Awake()
    {
        mainCam = Camera.main;

        SetDefaultCamPos();
    }

    // Update is called once per frame
    void Update()
    {
        CamZoom();
        FocusOnGun();
    }


    void SetDefaultCamPos()
    {
        transform.position = camFocusObject.transform.position + camOffset;
        mainCam.transform.rotation = Quaternion.Euler(camRotX, camRotY, 0f);
    }
    
    void CamZoom()
    {
        if (isGunFocus)
        {
            if (!isZoomed)
            {
                if (Input.GetButtonDown("Fire3"))
                {
                    Camera.main.fieldOfView = gunZoom;
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire3"))
                {
                    Camera.main.fieldOfView = maxZoom;
                }
            }
        }
        else
        {
            zoomValue -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

            zoomValue = Mathf.Clamp(zoomValue, minZoom, maxZoom);

            isZoomed = zoomValue < maxZoom;

            Camera.main.fieldOfView = zoomValue;
        }
    }

    void FocusOnGun()
    {
        if (Input.GetButtonDown("Jump"))
        {

            if (!isGunFocus)
            {
                
                transform.position = focusGun.transform.position + focusGunOffset;
                transform.rotation = focusGun.transform.rotation;
                isGunFocus = true;
            }
            else
            {
                SetDefaultCamPos();
                isGunFocus = false;
            }
            Camera.main.fieldOfView = maxZoom;
        }
    }

}
