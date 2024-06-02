using System;
using System.IO;
using UnityEngine;

internal class PlayerManager : MonoBehaviour
{
    [SerializeField] string _playerSavePath = "/saves/player.cfg";
    internal void SavePlayer()
    {
        PlayerSettings _playerSettings = GameManager.GameManagerInstance.PlayerSettings;
        PlayerStats _playerStats = GameManager.GameManagerInstance.PlayerTransform.GetComponent<PlayerStats>();
        Transform _player = GameManager.GameManagerInstance.PlayerTransform;

        string _savedContent = "";

        if (_playerSettings != null)
            _savedContent += $"{_playerSettings.FOV};{_playerSettings.FPS};{_playerSettings.RESX};{_playerSettings.RESY};{_playerSettings.FULLSCREEN};";
        if (_playerStats != null)
            _savedContent += $"{_playerStats.MovementSpeed};{_playerStats.JumpForce};{_playerStats.PickUpDistance};{_playerStats.AttackDistance};{_playerStats.AttackDamage};{_playerStats.FoodDelay};{_playerStats.OnHungerHitDelay};{_playerStats.MaxHealthPoints};{_playerStats.MaxFoodPoints};{_playerStats.CurrentHealthPoints};{_playerStats.CurrentFoodPoints};{_player.position.x};{_player.position.z};{_player.position.z};";

        string _path = Directory.GetCurrentDirectory() + _playerSavePath;
        if (File.Exists(_path))
        {
            StreamWriter _w = new StreamWriter(_path);

            _w.WriteLine(_savedContent);
            _w.Close();
        }
        else
        {
            File.Create(_path).Close();
            SavePlayer();
        }
    }

    internal void LoadPlayer()
    {
        PlayerSettings _playerSettings = GameManager.GameManagerInstance.PlayerSettings;
        PlayerStats _playerStats = GameManager.GameManagerInstance.PlayerTransform.GetComponent<PlayerStats>();
        Transform _player = GameManager.GameManagerInstance.PlayerTransform;

        string _path = Directory.GetCurrentDirectory() + _playerSavePath;

        if (File.Exists(_path))
        {
            StreamReader _r = new StreamReader(_path);

            string _content = _r.ReadLine();

            if (_content != null)
            {
                string[] _items = _content.Split(';');

                _playerSettings.SetValues(Convert.ToInt16(_items[0]), Convert.ToUInt32(_items[1]), Convert.ToInt16(_items[2]), Convert.ToInt16(_items[3]), Convert.ToBoolean(_items[4]));

                _playerStats.LoadPlayer((float)Convert.ToDouble(_items[5]), (float)Convert.ToDouble(_items[6]), (float)Convert.ToDouble(_items[7]), (float)Convert.ToDouble(_items[8]), (float)Convert.ToDouble(_items[9]), (float)Convert.ToDouble(_items[10]), (float)Convert.ToDouble(_items[11]), (float)Convert.ToDouble(_items[12]), (float)Convert.ToDouble(_items[13]), (float)Convert.ToDouble(_items[14]), (float)Convert.ToDouble(_items[15]));
                _player.position = new Vector3((float)Convert.ToDouble(_items[16]), (float)Convert.ToDouble(_items[17]), (float)Convert.ToDouble(_items[18]));
            }
            _r.Close();
        }
    }
}