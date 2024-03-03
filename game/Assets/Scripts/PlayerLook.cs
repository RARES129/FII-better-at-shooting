using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    private float xRotation = 0f;
    private float yRotation = 0f;
    private float recoilAmountX = 0f; // New variable for recoil
    private float recoilAmountY = 0f; // New variable for recoil

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        yRotation += (mouseX * Time.deltaTime) * xSensitivity; // Add recoil effect for y-axis

        // Apply recoil effect
        xRotation += recoilAmountX;
        yRotation += recoilAmountY;
        ResetRecoil();

        cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);

    }

    // Function to apply recoil
    public void ApplyRecoil(float amountX, float amountY)
    {
        recoilAmountX += amountX;
        recoilAmountY += amountY;
    }

    // Function to reset recoil
    public void ResetRecoil()
    {
        recoilAmountX = 0f;
        recoilAmountY = 0f;
    }
}
