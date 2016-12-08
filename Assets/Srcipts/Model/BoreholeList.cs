using UnityEngine;
using System.Collections.Generic;

public class BoreholeList : MonoBehaviour
{
    public List<Borehole> _boreholeList;
    private Borehole borehole;

    public void InitBoreholeList()
    {
        InitDataList();
        borehole = FindObjectOfType<Borehole>();
        Borehole via14 = borehole.initBorehole();
        AddNewBoreholeToBoreholeList(via14);
    }
    private void InitDataList()
    {
        if (_boreholeList != null)
            return;
        _boreholeList = new List<Borehole>(_boreholeList);
    }
    public List<Borehole> GetBoreholesList()
    {
        return _boreholeList;
    }
    public void AddNewBoreholeToBoreholeList(Borehole arg)
    {
        if (!_boreholeList.Contains(arg))
        {
            _boreholeList.Add(arg);
        }
        else
        {
            Debug.Log("singleBoreholeMode: " + arg._boreholeId + " already exist in singleBoreholeMode list");
        }
    }
}
