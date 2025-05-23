using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerCameraSwitcher : InteractiveManager
{
    [SerializeField] private CameraViewChanger cameraChanger;
    [SerializeField] private Transform cameraPoint;

    private bool isInteracting;

    public override void Interact()
    {
        if(!cameraChanger.IsWorking)
        {
            if (!cameraChanger.storeEditorModeEnabled)
            {
                isInteracting = cameraChanger.cameraSwitched;

                isInteracting = !isInteracting;

                if (isInteracting)
                {
                    cameraChanger.MovePlayerCamera(cameraPoint.position, new Vector3(0f, -90f, 0f));
                    Core.Quest.TryChangeQuest(QuestType.OpenComputer);
                }
                else cameraChanger.ResetCameraPosition();
            }
        }
    }

    public void PlayClick() => Core.Sound.PlayClip(AudioType.MouseClick);
}
