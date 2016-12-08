using UnityEngine;
using System.Collections;

public class SoilLayer : MonoBehaviour
{
    public string _nameSoilLayer { get; set; }
    public string _descriptionSoilLayer { get; set; }
    public string _height { get; set; }
    public SoilLayer(string nSoilLayer, string descSoilLayer, string height)
    {
        this._nameSoilLayer = nSoilLayer;
        this._descriptionSoilLayer = descSoilLayer;
        this._height = height;
    }
}