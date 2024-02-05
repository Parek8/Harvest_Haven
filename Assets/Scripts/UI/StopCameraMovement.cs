using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

internal sealed class StopCameraMovement : MonoBehaviour
{
    static StopCameraMovement _instance;
    internal static StopCameraMovement StopCameraMovementInstance => _instance;

    [field: SerializeField] List<UI_Behaviour> screens;
    Player_Movement _player;

    CinemachineFreeLook camera;
    private StopCameraMovement() { }
    private void Start()
    {
        if (_instance == null)
            _instance = this;

        camera = GameManager.game_manager.FreeCamera;
        _player = GameManager.game_manager.player_transform.GetComponent<Player_Movement>();
    }

    private void Update()
    {
        if (screens.Find(screen => screen.is_visible))
        {
            camera.enabled = false;
            _player.enabled = false;
        }
        else if (!camera.enabled)
        {
            camera.enabled = true;
            _player.enabled = true;
        }
    }
    internal void AddScreen(UI_Behaviour screen)
    {
        if (!screens.Contains(screen))
            screens.Add(screen);
    }     
}