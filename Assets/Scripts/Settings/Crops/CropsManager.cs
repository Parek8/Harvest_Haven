using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

internal class CropsManager : MonoBehaviour
{
    [SerializeField] string _cropsSavePath = "/saves/crops.cfg";

    internal void SaveCrops()
    {
        string _savedContent = "";
        List<Plot> _crops = GameManager.game_manager.all_crops;

        foreach (Plot _crop in _crops)
            if (_crop.plantedPlant != null)
            _savedContent += $"{_crop.PlotIndex}:{_crop.plantedPlant.PlantObjectIndex}:{_crop.Days};";

        string _path = Directory.GetCurrentDirectory() + _cropsSavePath;
        if (File.Exists(_path))
        {
            StreamWriter _w = new StreamWriter(_path);

            _w.WriteLine(_savedContent);
            _w.Close();
        }
        else
        {
            File.Create(_path).Close();
            SaveCrops();
        }
    }

    internal void LoadCrops()
    {
        string _path = Directory.GetCurrentDirectory() + _cropsSavePath;

        Dictionary<int, Plot> _allPlots = GameManager.game_manager._allCrops;
        List<Plot> _crops = GameManager.game_manager.all_crops;


        if (File.Exists(_path))
        {
            StreamReader _r = new StreamReader(_path);

            string _content = _r.ReadLine();

            if (_content != null)
            {
                string[] _items = _content.Split(';');

                for (int i = 0; i < _items.Length - 1; i++)
                {
                    string _item = _items[i];
                    string[] _cropItems = _item.Split(':');

                    int _plotIndex = Convert.ToInt32(_cropItems[0]);
                    int _cropIndex = Convert.ToInt32(_cropItems[1]);
                    int _cropDays = Convert.ToInt32(_cropItems[2]);

                    _crops.Find(_plot => _plot.PlotIndex == _plotIndex).LoadPlot(GameManager.game_manager.FindPlantObject(_cropIndex), _cropDays);
                }
            }
            _r.Close();
        }
    }
}