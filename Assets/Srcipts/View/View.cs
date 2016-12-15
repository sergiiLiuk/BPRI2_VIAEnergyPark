using UnityEngine;
using UnityEngine.UI;

public class View : MonoBehaviour
{
    private Controller controller;

    // Buttons
    public Button btnScenario1;
    public Button btnScenario2;
    public Button btnScenarioInfo;

    // Game Objects
    public GameObject scenarioInfo;
    public GameObject scenario2;
    public GameObject scenario1;
    public GameObject scenario1ObjectScaling;
    public GameObject scenario2ObjectScaling;
    public GameObject rotationCenterScenario1;
    public GameObject rotationCenterScenario2;
    public GameObject measureDistanceTo;

    // Weater Icons
    public GameObject wIcon_01d;
    public GameObject wIcon_02d;
    public GameObject wIcon_03d;
    public GameObject wIcon_09d;
    public GameObject wIcon_10d;
    public GameObject wIcon_11d;
    public GameObject wIcon_13d;
    public GameObject wIcon_nodata;
    public Text temperature;

    // GUI
    public GameObject labelInternetConnectionStatus;
    public GameObject labelServerConnectionStatus;
    public GameObject labelUsedStaticData;
    public GameObject dataset_Scenario2; // available in SingleBoreholeMode mode.
    public GameObject targetGuide;
    public GameObject selectAppScenarioAnimation;
    public GameObject toggleBtns; // available only in EntireSystem mode;
    public GameObject upperMenuBar;
    public GameObject lowerMenuBar;
    public GameObject btnsScenario;
    public GameObject dataLoadScreen;
    public GameObject onStartAppAnimation;
    public GameObject weatherSet;

    public Text txtCurrentLocation;

    // Change Obect Visibility
    public GameObject soilLayer1;
    public GameObject soilLayer2;
    public GameObject soilLayer3;
    public GameObject soilLayer4;
    public GameObject soilLayer5;
    public GameObject soilLayer6;
    public GameObject soilLayer7;
    public GameObject soilLayer8;
    public GameObject grassLayer;

    // Upper bar GUI elements Scenario 2
    public Text boreholeName;
    public Text tempInScenario2;
    public Text tempOutScenario2;
    public Text waterFlowSpeedScenario2;

    // Scenario Info GUI elements
    public Text infopage_tempBrineIn;
    public Text infopage_tempBrineOut;
    public Text infopage_waterFlowSpeedBrine;

    public Text infopage_tempVarmeIn;
    public Text infopage_tempVarmeOut;
    public Text infopage_waterFlowSpeedVarme;
    public Text infopageCoPvalue;

    // Run time data elements
    public TextMesh tempInScenario1Brine;
    public TextMesh tempOutScenario1Brine;
    public TextMesh waterFlowSpeedScenario1Brine;
    public TextMesh currentCoPvalueScenario1;

    public TextMesh tempInScenario1Varme;
    public TextMesh tempOutScenario1Varme;
    public TextMesh waterFlowSpeedScenario1Varme;


    void Start()
    {
        controller = FindObjectOfType<Controller>();
    }
    void OnEnable()
    {
        btnScenario1.onClick.AddListener(delegate { controller.SelectScenario(1); });
        btnScenario2.onClick.AddListener(delegate { controller.SelectScenario(2); });
        btnScenarioInfo.onClick.AddListener(delegate { controller.SelectScenario(3); });
    }

    public void ShowInternetConnectionErrorLabel()
    {
        labelInternetConnectionStatus.SetActive(true);
    }
    public void HideInternetConnectionErrorLabel()
    {
        labelInternetConnectionStatus.SetActive(false);
    }
    public void ShowUsedStaticDataLabel()
    {
        labelUsedStaticData.SetActive(true);
    }
    public void HideUsedStaticDataLabel()
    {
        labelUsedStaticData.SetActive(false);
    }
    public void ShowServerConnectionErrorLabel()
    {
        labelServerConnectionStatus.SetActive(true);
    }
    public void HideServerConnectionErrorLabel()
    {
        labelServerConnectionStatus.SetActive(false);
    }
    public void ShowTargetGuide()
    {
        targetGuide.SetActive(true);
    }
    public void HideTargetGuide()
    {
        targetGuide.SetActive(false);
    }
    public void HideSelectAppScenarioAnimation()
    {
        selectAppScenarioAnimation.SetActive(false);
    }
    public void ShowSelectAppScenarioAnimation()
    {
        selectAppScenarioAnimation.SetActive(true);
    }
    public void HideMainMenu()
    {
        upperMenuBar.SetActive(false);
        lowerMenuBar.SetActive(false);
        toggleBtns.SetActive(false);
        selectAppScenarioAnimation.SetActive(false);
    }
    public void ShowMainMenu()
    {
        upperMenuBar.SetActive(true);
        lowerMenuBar.SetActive(true);
        toggleBtns.SetActive(true);
    }
    public void HideBtnsScenario()
    {
        btnsScenario.SetActive(false);
    }
    public void ShowBtnsScenario()
    {
        btnsScenario.SetActive(true);
    }
    public void HideLoadDataAnimation()
    {
        dataLoadScreen.SetActive(false);
    }
    public void ShowLoadDataAnimation()
    {
        dataLoadScreen.SetActive(true);
    }
    public void HideUpperMenuBar()
    {
        upperMenuBar.SetActive(false);
    }
    public void ShowLowerMenuBar()
    {
        upperMenuBar.SetActive(true);
    }
    public void HideLowerMenuBar()
    {
        lowerMenuBar.SetActive(false);
    }
    public void ShowUpperMenuBar()
    {
        lowerMenuBar.SetActive(true);
    }
    public void HideToggles()
    {
        toggleBtns.SetActive(false);
    }
    public void ShowToggles()
    {
        toggleBtns.SetActive(true);
    }
    public void ShowDataScenario2()
    {
        dataset_Scenario2.SetActive(true);
    }
    public void HideDataScenario2()
    {
        dataset_Scenario2.SetActive(false);
    }
}
