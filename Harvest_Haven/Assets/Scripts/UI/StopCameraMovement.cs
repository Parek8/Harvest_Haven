using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class StopCameraMovement : MonoBehaviour
{
    [field: SerializeField] List<UI_Behaviour> screens;
    CinemachineFreeLook camera;
    private void Start() => camera = GameManager.game_manager.FreeCamera;

    private void Update()
    {
        if (screens.Find(screen => screen.is_visible))
            camera.enabled = false;
        else if (!camera.enabled)
            camera.enabled = true;
    }
}