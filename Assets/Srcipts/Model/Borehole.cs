using UnityEngine;
using System.Collections.Generic;
using System;

public class Borehole : MonoBehaviour
{
    public string _boreholeId { get; set; }
    private List<SoilLayer> _soilLayer;

    public Borehole(String boreholeId, List<SoilLayer> sLayers)
    {
        this._boreholeId = boreholeId;
        this._soilLayer = new List<SoilLayer>(sLayers);
    }
    public List<SoilLayer> GetSoilLayersList()
    {
        return _soilLayer;
    }
    public Borehole initBorehole()
    {
        SoilLayer level_0 = new SoilLayer(" ", " ", "0.0 m");
        SoilLayer TopSoil = new SoilLayer("TOP SOIL", " ", "- 0.1 m");
        SoilLayer CLAYTILLSandy = new SoilLayer("CLAY TILL, sandy", "{λ_2 m = 1.54 W/m/K", "-6.0 m");
        SoilLayer GravelPorlySorted = new SoilLayer("Gravel, porly sorted", "{λ_7 m = 2.36 W/m/K", "-9.0 m");
        SoilLayer WaterTABLE = new SoilLayer(" WATER TABLE", " ", "-15.4 m");
        SoilLayer SANDMediumSizeCoarseGrained = new SoilLayer("SAND, medium size, coarse grained", "{λ_15.5 m = 2.34 W/m/", "-24.0 m");
        SoilLayer CLAYSilty = new SoilLayer("CLAY, silty", "{λ_27 m = 1.74 W/m/K", "-50.0 m");
        SoilLayer MICACLAY = new SoilLayer("MICA CLAY", "{λ_46.8 m = 1.31 W/m/K, -57.0 m, λ_49.3 m = 1.10 W/m/K", "-57.0 m");
        SoilLayer MICACLAYSilty = new SoilLayer("MICA CLAY, silty", " ", "-100.0 m");

        Borehole newBorehole = new Borehole("VIA14", new List<SoilLayer> { level_0, TopSoil, CLAYTILLSandy, GravelPorlySorted, WaterTABLE, SANDMediumSizeCoarseGrained, CLAYSilty, MICACLAY, MICACLAYSilty });
        return newBorehole;
    }
}
