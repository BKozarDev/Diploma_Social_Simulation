using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float fastSpeed = 100f;
    public float sensitivity = 3f;
    public float zoomSensitivity = 10f;
    public float fastZoomSens = 50f;
    private bool looking = false;

    private void Update() {
        var fastMode = Input.GetKey(KeyCode.LeftShift);
        var speed = fastMode ? this.fastSpeed : this.moveSpeed;

        if(Input.GetKey(KeyCode.A))
        {
            transform.position = transform.position + (-transform.right * moveSpeed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.D))
        {
            transform.position = transform.position + (transform.right * moveSpeed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.W))
        {
            transform.position = transform.position + (transform.forward * moveSpeed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.S))
        {
            transform.position = transform.position + (-transform.forward * moveSpeed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.Q))
        {
            transform.position = transform.position + (transform.up * moveSpeed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.E))
        {
            transform.position = transform.position + (-transform.up * moveSpeed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.R))
        {
            transform.position = transform.position + (Vector3.up * moveSpeed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.F))
        {
            transform.position = transform.position + (-Vector3.up * moveSpeed * Time.deltaTime);
        }

        if(looking)
        {
            float newRotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
            float newRotationY = transform.localEulerAngles.x + Input.GetAxis("Mouse Y") * sensitivity;
            transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);
        }

        float axis = Input.GetAxis("Mouse ScrollWheel");
        if(axis != 0)
        {
            var zoomSensitivity = fastMode ? this.fastZoomSens : this.zoomSensitivity;
            transform.position = transform.position + transform.forward * axis * zoomSensitivity;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartLooking();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            StopLooking();
        }
    }

    private void OnDisable() {
        StopLooking();
    }

    public void StartLooking()
    {
        looking = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StopLooking()
    {
        looking = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
