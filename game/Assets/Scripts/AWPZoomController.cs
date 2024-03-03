using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AWPZoomController : MonoBehaviour
{
    public Camera awpCamera; // Trage camera AWP aici în inspector
    public Camera scopeCamera; // Trage camera pentru scop aici în inspector
    public float zoomFOV1 = 30f; // FOV-ul primului nivel de zoom
    public float zoomFOV2 = 10f; // FOV-ul celui de-al doilea nivel de zoom

    private int zoomLevel = 0;
    private GunSystem gunSystem;


    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ToggleZoom();
        }
    }

    private void Awake()
    {
        gunSystem = GetComponent<GunSystem>();
    }

    private void ToggleZoom()
    {
        zoomLevel = (zoomLevel + 1) % 3; // Ciclu între cele trei stări de zoom
        switch (zoomLevel)
        {
            case 0:
                awpCamera.fieldOfView = 60f; // FOV normal
                Debug.Log("Zoom level 0");
                gunSystem.spreadModification(0.1f) ; // Modificarea spread-ului la zoom-out
                break;
            case 1:
                awpCamera.fieldOfView = zoomFOV1; // FOV primului nivel de zoom
                Debug.Log("Zoom level 1");
                gunSystem.spreadModification(0.01f);
                break;
            case 2:
                awpCamera.fieldOfView = zoomFOV2; // FOV celui de-al doilea nivel de zoom
                Debug.Log("Zoom level 2");
                gunSystem.spreadModification(0f);
                break;
        }
    }
}