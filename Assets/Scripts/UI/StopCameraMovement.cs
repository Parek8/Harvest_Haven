using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

internal sealed class StopCameraMovement : MonoBehaviour
{
    static StopCameraMovement _instance;
    internal static StopCameraMovement StopCameraMovementInstance => _instance;

    [field: SerializeField] List<UI_Behaviour> screens;
    Player_Movement _player;

    new CinemachineVirtualCamera camera;

    internal bool IsAnyScreenActive => m_IsActive;
    private bool m_IsActive;
    private StopCameraMovement() { }
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
    private void Start()
    {
        if (_instance == null)
            _instance = this;

        camera = GameManager.GameManagerInstance.FreeCamera;
        _player = GameManager.GameManagerInstance.PlayerTransform.GetComponent<Player_Movement>();
    }

    private void Update()
    {
        if (screens.Find(screen => screen.is_visible))
        {
            _player.StopAllAnimations();
            camera.enabled = false;
            _player.enabled = false;
            m_IsActive = true;
        }
        else if (!camera.enabled)
        {
            camera.enabled = true;
            _player.enabled = true;
            m_IsActive = false;
        }
    }
    internal void AddScreen(UI_Behaviour screen)
    {
        if (!screens.Contains(screen))
            screens.Add(screen);
    }     
}