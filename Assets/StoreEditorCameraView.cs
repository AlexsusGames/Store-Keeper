using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreEditorCameraView : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private RawImage targetRawImage; 

    private Vector2Int textureResolution = new Vector2Int(1920, 1080); 

    private RenderTexture renderTexture;

    private void Start()
    {
        renderTexture = new RenderTexture(textureResolution.x, textureResolution.y, 24);

        targetCamera.targetTexture = renderTexture;
        targetCamera.aspect = (float)textureResolution.x / textureResolution.y;

        targetRawImage.texture = renderTexture;
    }

    private void Update()
    {
        targetCamera.Render();
    }

    private void OnDestroy()
    {
        renderTexture.Release();
    }

}
