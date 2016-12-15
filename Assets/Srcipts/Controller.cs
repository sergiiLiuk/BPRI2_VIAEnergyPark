using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{
    private View view;
    private Model model;

    private IEnumerator coroutineOnStartAppAnimation;
    private IEnumerator coroutineRetrieveAppWebData;
    private IEnumerator coroutineRetrieveRuntimeDataFromWebServer;

    public static LoadAdditionalDataState loadAdditionalDataState;
    public static LoadRuntimeDataState runtimeDataState;
    public static PogramFlowState sceneState;

    // Use this for initialization
    void Start()
    {
        view = FindObjectOfType<View>();
        model = FindObjectOfType<Model>();

        coroutineOnStartAppAnimation = model.PlayOnStartAppAnimation(3.0f);
        StartCoroutine(coroutineOnStartAppAnimation);

        coroutineRetrieveAppWebData = ManageAdditionalData(0.05f);
        StartCoroutine(coroutineRetrieveAppWebData);

        coroutineRetrieveRuntimeDataFromWebServer = ManageRuntimeData(0.5f);
        StartCoroutine(coroutineRetrieveRuntimeDataFromWebServer);

        loadAdditionalDataState = LoadAdditionalDataState.CheckInternetConnection;
        runtimeDataState = LoadRuntimeDataState.ObtainDataFromServer;

    }

    // Update is called once per frame
    void Update()
    {
        model.DetectLocation();
        SwitchApplicationCase();
        model.MarkerDetection();
        model.ChangeObjVisibility(); // Change alpha channel on Energy house, Soil layers 3d models (depends on distance);

#if UNITY_ANDROID
        model.Swipe();
        model.GetTouch();   // Detect touches on device's screen;
        model.ObjectScaling();
#endif
    }
    public void SwitchApplicationCase()
    {
        switch (sceneState)
        {
            case PogramFlowState.OnStart:
                view.ShowUpperMenuBar();
                view.ShowLowerMenuBar();
                view.HideBtnsScenario();
                view.HideToggles();
                view.HideDataScenario2();
                view.scenario1.SetActive(false);
                view.scenario2.SetActive(false);
                break;
            case PogramFlowState.IsScenario1:
                if (Model.isMenuOn == true)
                {
                    view.scenarioInfo.SetActive(false);
                    view.scenario1.SetActive(true);
                    view.scenario2.SetActive(false);
                    model.DisplayRuntimeData_Scenario1();
                    view.HideDataScenario2();
                    view.ShowToggles();
                }
                break;
            case PogramFlowState.IsScenario2:
                if (Model.isMenuOn == true)
                {
                    view.scenarioInfo.SetActive(false);
                    view.scenario1.SetActive(false);
                    view.scenario2.SetActive(true);
                    view.ShowDataScenario2();
                    view.HideToggles();
                    model.BuildSoillayerDescription_Scenario2("VIA14");
                    model.DisplayRuntimeData_Scenario2("VIA14");
                }
                break;
            case PogramFlowState.IsScenarioInfo:
                view.scenarioInfo.SetActive(true);
                model.DisplayData_ScenarioInfo();
                break;
            case PogramFlowState.DefaultMode:
                view.scenarioInfo.SetActive(false);
                view.scenario1.SetActive(false);
                view.scenario2.SetActive(false);
                view.ShowSelectAppScenarioAnimation();
                view.ShowMainMenu();
                view.ShowBtnsScenario();
                view.HideToggles();
                view.HideTargetGuide();
                Model.isMenuOn = true;
                break;
        }
    }
    public IEnumerator ManageRuntimeData(float waitTime)
    {
        while (Model.tryEstablishConnectionToWebServer)
        {
            if (Model.isLocationDetected&&Model.isInternetConnectionEstablished)
            {
                switch (runtimeDataState)
                {
                    case LoadRuntimeDataState.ObtainDataFromServer:
                        view.ShowLoadDataAnimation();
                        yield return new WaitForSeconds(1);
                        model.DataServerConnector("http://9b15d8f2.ngrok.io/dbtest.php");
                        break;
                    case LoadRuntimeDataState.UseRuntimeData:
                        view.HideUsedStaticDataLabel();
                        view.HideLoadDataAnimation();
                        Debug.Log("Needed Web Data has been obtained");
                        view.HideLoadDataAnimation();
                        Model.tryEstablishConnectionToWebServer = false;
                        break;
                    case LoadRuntimeDataState.UseStaticData:
                        view.ShowServerConnectionErrorLabel();
                        view.ShowUsedStaticDataLabel();
                        model.SetStaticSystemData();
                        view.HideLoadDataAnimation();
                        Debug.Log("Used Static Data");
                        view.HideLoadDataAnimation();
                        break;
                    case LoadRuntimeDataState.DefaultMode:

                        break;
                }
            }
            yield return new WaitForSeconds(waitTime);
        }
    }
    public IEnumerator ManageAdditionalData(float waitTime)
    {
        while (model.retrieveAdditionalData == true)
        {
            yield return new WaitForSeconds(waitTime / 5);
            switch (loadAdditionalDataState)
            {
                case LoadAdditionalDataState.CheckInternetConnection:
                    if (model.allowCheckInternetConnection)
                    {
                        view.ShowLoadDataAnimation();
                        model.coroutineCheckInternetConnection = model.CheckInternetConnection(1f);
                        model.StartCoroutine(model.coroutineCheckInternetConnection);
                    }
                    break;
                case LoadAdditionalDataState.ObtainIp:
                    view.ShowLoadDataAnimation();
                    Model.currentIP = model.GetLocalIP("http://canhazip.com");
                    break;
                case LoadAdditionalDataState.ObtainCurrentCityLoc:
                    view.ShowLoadDataAnimation();
                    Model.currentCity = model.GetCurrentCityLocation(Model.currentIP);
                    break;
                case LoadAdditionalDataState.ObtainWeatherCondition:
                    view.ShowLoadDataAnimation();
                    StartCoroutine(model.RetrieveWeatherInformationFromWebAPI(Model.currentCity));
                    model.retrieveAdditionalData = false;
                    //loadAdditionalDataState = LoadAdditionalDataState.DefaultMode;
                    break;
                case LoadAdditionalDataState.DefaultMode:

                    break;
            }
        }
    }
    //---------------------------------------
    // Activate app scenario based on entered value (1 - scenario1, 2- scenario2);
    public void SelectScenario(int myInt)
    {
        if (myInt == 1)
        {
            sceneState = PogramFlowState.IsScenario1;
        }
        else if (myInt == 2)
        {
            sceneState = PogramFlowState.IsScenario2;
        }
        else if (myInt == 3)
        {
            sceneState = PogramFlowState.IsScenarioInfo;
        }
        //Debug.Log("Scenario: " + myInt);
    }
    //---------------------------------------

}
