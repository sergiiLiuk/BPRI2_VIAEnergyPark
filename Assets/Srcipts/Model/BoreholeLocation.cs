using UnityEngine;

public class BoreholeLocation : MonoBehaviour
{
    public string _locationName { get; set; }
    public string _arMarkerId { get; set; }

    public BoreholeLocation(string placeName, string arMarkerId)
    {
        this._locationName = placeName;
        this._arMarkerId = arMarkerId;
    }
}
