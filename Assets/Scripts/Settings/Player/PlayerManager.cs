using System;
using System.IO;
using UnityEngine;

internal class PlayerManager : MonoBehaviour
{
    [SerializeField] string _playerSavePath = "/saves/player.cfg";
    internal void SavePlayer()
    {
        PlayerSettings _playerSettings = GameManager.game_manager.PlayerSettings;
        Character_Stats _playerStats = GameManager.game_manager.player_transform.GetComponent<Character_Stats>();

        string _savedContent = "";

        if (_playerSettings != null)
            _savedContent += $"{_playerSettings.FOV};{_playerSettings.FPS};{_playerSettings.RESX};{_playerSettings.RESY};{_playerSettings.FULLSCREEN};";
        if (_playerStats != null)
            _savedContent += $"{_playerStats.movement_speed};{_playerStats.jump_force};{_playerStats.pick_up_distance};{_playerStats.attack_distance};{_playerStats.attack_damage};{_playerStats.food_delay};{_playerStats.on_hunger_hit_delay};{_playerStats.max_health_points};{_playerStats.max_food_points};{_playerStats.current_health_points};{_playerStats.current_food_points}";

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
        PlayerSettings _playerSettings = GameManager.game_manager.PlayerSettings;
        Character_Stats _playerStats = GameManager.game_manager.player_transform.GetComponent<Character_Stats>();

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
            }
            _r.Close();
        }
    }
}