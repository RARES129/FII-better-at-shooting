using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowCursor : MonoBehaviour
{
    public float sensitivity = 30f; // Sensibilitatea camerei la mișcarea cursorului

    private void Update()
    {
        // Obține poziția cursorului în lumea 3D
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

        // Calculează direcția de privire a camerei către cursor
        Vector3 lookDirection = cursorPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

        // Interpolare liniară pentru o tranziție mai lină
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * sensitivity);
    }
}