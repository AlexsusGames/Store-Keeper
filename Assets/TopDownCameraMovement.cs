using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TopDownCameraMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float scrollSensivity;

    [SerializeField] private float xMinBound;
    [SerializeField] private float xMaxBound;
    [SerializeField] private float zMinBound;
    [SerializeField] private float zMaxBound;

    private Rigidbody rb;
    private float yRotation = 0f;
    private float yPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        yPosition = transform.position.y;
    }

    void FixedUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        float x = -Input.GetAxis("Horizontal");
        float z = -Input.GetAxis("Vertical");

        if (Input.GetMouseButton(2))
        {
            yRotation -= mouseX;

            transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        }


        Vector3 move = transform.right * x + transform.forward * z;
        Vector3 newPosition = rb.position + move * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(CheckBound(newPosition));
    }

    private void Update()
    {
        float mouseY = Input.GetAxisRaw("Mouse ScrollWheel") * scrollSensivity * Time.deltaTime;

        yPosition -= mouseY;

        yPosition = Mathf.Clamp(yPosition, 25f, 75f);
        transform.localPosition = new Vector3(transform.localPosition.x, yPosition, transform.localPosition.z);
    }

    private Vector3 CheckBound(Vector3 vector)
    {
        float newX = Mathf.Clamp(vector.x, xMinBound, xMaxBound);
        float newZ = Mathf.Clamp(vector.z, zMinBound, zMaxBound);

        return new Vector3(newX, yPosition, newZ);
    }

}
