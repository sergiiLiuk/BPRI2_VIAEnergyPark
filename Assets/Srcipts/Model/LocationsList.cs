using UnityEngine;
using System.Collections.Generic;

public class LocationsList : MonoBehaviour
{
    public List<BoreholeLocation> _locationsList;

    public void InitLocationList()
    {
        InitDataList();
        BoreholeLocation viaEnergyPark = new BoreholeLocation("VIA Energy Park, Horsens", "ARmarkerVIAEnergyStorage");
        AddNewLocationToList(viaEnergyPark);
    }
    private void InitDataList()
    {
        if (_locationsList != null)
            return;
        _locationsList = new List<BoreholeLocation>(_locationsList);
    }
    public List<BoreholeLocation> GetLocationsList()
    {
        return _locationsList;
    }
    public void AddNewLocationToList(BoreholeLocation arg)
    {
        if (!_locationsList.Contains(arg))
        {
            _locationsList.Add(arg);
        }
        else
        {
            Debug.Log("singleBoreholeMode: " + arg._locationName + " already exist in singleBoreholeMode list");
        }
    }
}
