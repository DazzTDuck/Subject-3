using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCam : MonoBehaviour
{


    public Vector3 camOffset;
    public float defaultCamRotX;
    public float defaultCamRotY;

    //is also used to set the cam in the right posision so make sure the manager is on 0,0,0
    public GameObject manager; 

    public Vector3 topCamPos;
    public float topCamRotX;
    public float topCamRotY;

    public bool isGunFocus;
    public GameObject focusGun;
    public Vector3 focusGunOffset;
    public float gunZoom = 35f;
    public float camDefaultFov = 60f;
    
    

    [HideInInspector]
    public bool isTopView;


    // Start is called before the first frame update
    void Awake()
    {

        MoveCamToStartLocation();
    }

    // Update is called once per frame
    void Update()
    {
        CamChangeTopView();
        FocusOnGun();
    }

    
    void CamChangeTopView()
    {
        if (Input.GetButtonDown("Debug1"))
        {
            if (!isTopView)
            {
                MoveCamToTopLocation();
                isTopView = true;
                isGunFocus = false;
            }
            else
            {
                MoveCamToStartLocation();
                isTopView = false;
            }
        }
    }

    void FocusOnGun()
    {
        if (Input.GetButtonDown("Jump"))
        {

            if (!isGunFocus)
            {
                MoveCamToTower();
                isGunFocus = true;
            }
            else
            {
                MoveCamToStartLocation();
                isGunFocus = false;
                isTopView = false;
            }
            Camera.main.fieldOfView = camDefaultFov;
            manager.GetComponent<TowerBuilder>().CancelPlacement();
        }
    }

    void MoveCamToStartLocation()
    {
        transform.position = manager.transform.position + camOffset;
        transform.rotation = Quaternion.Euler(defaultCamRotX, defaultCamRotY, 0f);   
    }

    void MoveCamToTopLocation()
    {
        transform.position = topCamPos;
        transform.rotation = Quaternion.Euler(topCamRotX, topCamRotY, 0f);
    }

    void MoveCamToTower()
    {
        transform.position = focusGun.transform.position + focusGunOffset;
        transform.rotation = focusGun.transform.rotation;
    }


}
