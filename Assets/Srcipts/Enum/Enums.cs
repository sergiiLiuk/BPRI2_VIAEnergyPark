using UnityEngine;
using System.Collections;

public enum LoadAdditionalDataState
{
    CheckInternetConnection,
    ObtainIp,
    ObtainCurrentCityLoc,
    ObtainWeatherCondition,
    DefaultMode
};
public enum LoadRuntimeDataState
{
    ObtainDataFromServer,
    UseRuntimeData,
    UseStaticData,
    DefaultMode
};
public enum PogramFlowState
{
    OnStart,
    IsScenario1,
    IsScenario2,
    IsScenarioInfo,
    isTargetLost,
    isTargetFound,
    DefaultMode
};
