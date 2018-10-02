using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perekatyvalki2D
{
    /// <summary></summary>
    public class GameSettings : MonoBehaviour
    {
        /// <summary>Данные о настройках.</summary>
        public static SettingsData Data { get; private set; }

        // 
        private void Awake()
        {
            TextAsset textAsset = Resources.Load<TextAsset>("settings");
            Data = JsonConvert.DeserializeObject<SettingsData>(textAsset.text);

            //// 
            //SettingsData settings = new SettingsData();
            //settings.CountSeconds = 180;
            //settings.Colors = new ColorVector[6];
            //Color[] colors = new Color[]
            //{
            //    Color.blue, Color.green, Color.red, Color.cyan, Color.yellow, Color.magenta
            //};
            //for (int i = 0; i < colors.Length; i++)
            //{
            //    ColorVector colorVector = new ColorVector();
            //    colorVector.r = colors[i].r;
            //    colorVector.g = colors[i].g;
            //    colorVector.b = colors[i].b;
            //    settings.Colors[i] = colorVector;
            //}
            //string str = JsonConvert.SerializeObject(settings);
        }
    }


    /// <summary></summary>
    public class SettingsData
    {
        public int CountSeconds;
        public ColorVector[] Colors;
    }
    /// <summary></summary>
    public class ColorVector
    {
        public float r;
        public float g;
        public float b;
    }
}