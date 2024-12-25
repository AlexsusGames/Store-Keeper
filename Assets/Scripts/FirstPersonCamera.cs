using System;
using System.Collections;
using System.Threading;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private Transform playerBody;

    private Coroutine moveCoroutine = null;

    private bool cameraBlock;
    private Vector3 standartPos;
    private float xRotation = 0f;
    private float animationDuration = 0.5f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
    }

    private void LateUpdate()
    {
        if(cameraBlock == false)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 60f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    public void SetCameraBlockEnabled(bool enabled)
    {
        cameraBlock = enabled;
    }

    public void SetRotation(Vector3 direction)
    {
        this.transform.localRotation = Quaternion.Euler(Vector3.zero);
        playerBody.localRotation = Quaternion.Euler(direction);
    }

    public void SetNewCameraPosition(Vector3 globalCord, Action callback)
    {
        if(moveCoroutine == null)
        {
            standartPos = transform.position;
            moveCoroutine = StartCoroutine(MoveCameraCoroutine(globalCord, callback));
        }
    }

    public void ResetCameraPosition(Action callback)
    {
        if (moveCoroutine == null)
        {
            moveCoroutine = StartCoroutine(MoveCameraCoroutine(standartPos, callback));
        }
    }

    private IEnumerator MoveCameraCoroutine(Vector3 targetPosition, Action callback)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / animationDuration);

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            yield return null;
        }
        transform.position = targetPosition;
        moveCoroutine = null;

        callback?.Invoke();
    }

}
