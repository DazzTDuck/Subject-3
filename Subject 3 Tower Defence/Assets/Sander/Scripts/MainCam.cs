using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class MainCam : MonoBehaviour
{
    public Vector3 camPos;
    public float defaultCamRotX;
    public float defaultCamRotY;

    //is also used to set the cam in the right posision so make sure the manager is on 0,0,0
    public GameObject manager;

    public Vector3 topCamPos;
    public float topCamRotX;
    public float topCamRotY;

    public bool isGunFocus = false;
    public GameObject focusGun;
    public Vector3 focusGunOffset;
    public float gunZoom = 35f;
    public float camDefaultFov = 60f;


    [HideInInspector]
    public bool isTopView;

    bool canLerp = true;
    readonly float lerpSpeed = 7;
    Vector3 currentCamPos;
    Quaternion currentCamRot;
    BuyingPanelHandler buyingPanelHandler;


    // Start is called before the first frame update
    void Awake()
    {
        camPos = transform.position;
        MoveCamera(StartPosistion(), StartRotation());
        buyingPanelHandler = FindObjectOfType<BuyingPanelHandler>();
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
        if (Input.GetButtonDown("Sprint"))
        {
            if (!isTopView)
            {
                focusGun.GetComponentInParent<TowerControl>().ResetHeadRot();
                MoveCamera(TopPosistion(), TopRotation());
                isTopView = true;
                isGunFocus = false;
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                MoveCamera(StartPosistion(), StartRotation());
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
                isGunFocus = true;
                Cursor.lockState = CursorLockMode.Locked;

                if(buyingPanelHandler.PanelOpen())
                buyingPanelHandler.ClosePanel();
            }
            else
            {
                focusGun.GetComponentInParent<TowerControl>().ResetHeadRot();
                MoveCamera(StartPosistion(), StartRotation());
                isGunFocus = false;
                isTopView = false;
                Cursor.lockState = CursorLockMode.Confined;
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
        canLerp = true;
    }

    void LerpCamera(Vector3 position, Quaternion rotation)
    {
        if (transform.position != position && canLerp)
        {
            transform.position = Vector3.Lerp(transform.position, position, lerpSpeed * Time.deltaTime);
        }
        else canLerp = false;

        if (transform.rotation != rotation && canLerp)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, lerpSpeed * Time.deltaTime);
        }
        else canLerp = false;
    }

    public bool CanLerp()
    {
        return canLerp;
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
        return camPos;
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
