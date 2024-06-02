using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

internal class CropsManager : MonoBehaviour
{
    [SerializeField] string _cropsSavePath = "/saves/crops.cfg";

    internal void SaveCrops()
    {
        string _savedContent = "";
        List<Plot> _crops = GameManager.GameManagerInstance.AllCrops;

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

        Dictionary<int, Plot> _allPlots = GameManager.GameManagerInstance.AllCropsDictionary;
        List<Plot> _crops = GameManager.GameManagerInstance.AllCrops;

        if (File.Exists(_path))
        {
            StreamReader _r = new StreamReader(_path);

            string _content = _r.ReadLine();

            if (_content != null)
            {
                string[] _items = _content.Split(';');

                for (int i = 0; i < _items.Length; i++)
                {
                    string _item = _items[i];
                    if (_item != String.Empty)
                    {
                        string[] _cropItems = _item.Split(':');

                        int _plotIndex = Convert.ToInt32(_cropItems[0]);
                        int _cropIndex = Convert.ToInt32(_cropItems[1]);
                        int _cropDays = Convert.ToInt32(_cropItems[2]);

                        Plot _crop = _crops.Find(_plot => _plot.PlotIndex == _plotIndex);

                        if (_crop != null)
                            _crop.LoadPlot(GameManager.GameManagerInstance.FindPlantObject(_cropIndex), _cropDays);
                        else
                            Debug.Log(GameManager.GameManagerInstance.AllCrops.Count);
                    }
                }
            }
            else
                Debug.Log("Empty");
            _r.Close();
        }
        else
            Debug.Log("Path doesn't exist!");
    }
}