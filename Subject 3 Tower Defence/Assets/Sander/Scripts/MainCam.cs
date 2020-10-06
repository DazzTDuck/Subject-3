using System;
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

    readonly float lerpSpeed = 7;
    Vector3 currentCamPos;
    Quaternion currentCamRot;


    // Start is called before the first frame update
    void Awake()
    {
        MoveCamera(StartPosistion(), StartRotation());
        //MoveCamToStartLocation();
    }

    // Update is called once per frame
    void Update()
    {
        CamChangeTopView();
        FocusOnGun();

        UpdateCameraLerp();
    }

    void CamChangeTopView()
    {
        if (Input.GetButtonDown("Debug1"))
        {
            if (!isTopView)
            {
                //MoveCamToTopLocation();
                MoveCamera(TopPosistion(), TopRotation());
                isTopView = true;
                isGunFocus = false;
            }
            else
            {
                MoveCamera(StartPosistion(), StartRotation());
                //MoveCamToStartLocation();
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
                MoveCamera(TowerPosistion(), TowerRotation());
                //MoveCamToTower();
                isGunFocus = true;
            }
            else
            {
                MoveCamera(StartPosistion(), StartRotation());
                //MoveCamToStartLocation();
                isGunFocus = false;
                isTopView = false;
            }
            Camera.main.fieldOfView = camDefaultFov;
            manager.GetComponent<TowerBuilder>().CancelPlacement();
        }
    }
    private void UpdateCameraLerp()
    {
        LerpCamera(currentCamPos, currentCamRot);
    }

    void MoveCamera(Vector3 position, Quaternion rotation)
    {
        currentCamPos = position;
        currentCamRot = rotation;
    }

    void LerpCamera(Vector3 position, Quaternion rotation)
    {
        if (transform.position != position)
            transform.position = Vector3.Lerp(transform.position, position, lerpSpeed * Time.deltaTime);

        if(transform.rotation != rotation)
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, lerpSpeed * Time.deltaTime);
    }

    Vector3 TowerPosistion()
    {
        return focusGun.transform.position + focusGunOffset;
    }
    Quaternion TowerRotation()
    {
        return focusGun.transform.rotation;
    }
    Vector3 StartPosistion()
    {
        return Vector3.zero + camOffset;
    }
    Quaternion StartRotation()
    {
        return Quaternion.Euler(defaultCamRotX, defaultCamRotY, 0f);
    }
    Vector3 TopPosistion()
    {
        return topCamPos;
    }
    Quaternion TopRotation()
    {
        return Quaternion.Euler(topCamRotX, topCamRotY, 0f);
    }


}
