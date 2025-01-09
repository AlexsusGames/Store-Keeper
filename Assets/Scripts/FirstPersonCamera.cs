using System;
using System.Collections;
using System.Threading;
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
        standartPos = transform.localPosition;
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

    public bool IsWorking => moveCoroutine != null;

    private void SetRotation(Vector3 direction)
    {
        this.transform.localRotation = Quaternion.Euler(Vector3.zero);
        playerBody.localRotation = Quaternion.Euler(direction);
    }

    public void SetNewCameraPosition(Vector3 globalCord, Action callback)
    {
        if(moveCoroutine == null)
        {
            moveCoroutine = StartCoroutine(MoveCameraCoroutine(globalCord, callback, false));
        }
    }

    public void ResetCameraPosition(Action callback)
    {
        if (moveCoroutine == null)
        {
            moveCoroutine = StartCoroutine(MoveCameraCoroutine(standartPos, callback, true));
        }
    }

    private IEnumerator MoveCameraCoroutine(Vector3 targetPosition, Action callback, bool isLocalPos)
    {
        Vector3 startPosition;
        if (!isLocalPos)
        {
            startPosition = transform.position;
        }
        else startPosition = transform.localPosition;


        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            SetRotation(new Vector3(0f, -90f, 0f));

            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / animationDuration);

            if(!isLocalPos)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            }
            else transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);

            yield return null;
        }

        if(!isLocalPos)
        {
            transform.position = targetPosition;
        }
        else transform.localPosition = targetPosition;

        callback?.Invoke();

        moveCoroutine = null;
    }

}
