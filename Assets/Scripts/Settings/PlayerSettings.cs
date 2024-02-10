using UnityEngine;

[CreateAssetMenu(menuName = "Player/Player Settings")]
public class PlayerSettings : ScriptableObject
{
    public short FOV { get; private set; } = 60;
    public uint FPS { get; private set; } = 60;
    public short RESX { get; private set; } = 1920;
    public short RESY { get; private set; } = 1080;
    public bool FULLSCREEN { get; private set; } = true;

    internal void SetValues(short fov, uint fps, short resx, short resy, bool full)
    {
        this.FOV = fov;
        this.FPS = fps;
        this.RESX = resx;
        this.RESY = resy;
        this.FULLSCREEN = full;
        GameManager.game_manager.LoadGraphics();
    }

    public string GetInfo()
    {
        return $"FOV: {FOV} | FPS: {FPS} | RES: {RESX}x{RESY} | FULLSCREEN: {FULLSCREEN}";
    }
}