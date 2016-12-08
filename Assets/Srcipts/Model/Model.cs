using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml;
using System;
using System.IO;
using Vuforia;

public class Model : MonoBehaviour
{
    private View view;
    private LocationsList locationsList;
    private BoreholeList bList;
    public IEnumerator coroutineCheckInternetConnection;
    private IEnumerator coroutineInitScenario2;

    // Audio sources
    public AudioSource btnClick;
    public AudioSource toggleClick;

    // Color
    private Color textColorBoreholeMode;

    // Variables
    private string jsonString;

    public static string currentIP;
    public static string currentCity;

    private static string tempWarmIn;
    private static string tempWarmOut;
    private static string tempBrineIn;
    private static string tempBrineOut;
    private static string warmLitPerMin;
    private static string brineLitPerMin;

    // Vectors
    private Vector2 startPos;

    public float minSwipeDistY;
    public float minSwipeDistX;

    [SerializeField]
    private float currentAmountVIA14_Scenario1;

    // bool
    public static bool isInternetConnectionEstablished = false;
    public static bool tryEstablishConnectionToWebServer = true;
    public bool retrieveAdditionalData = true;
    public static bool isLocationDetected = false;
    public bool allowCheckInternetConnection = true;
    public static bool isMenuOn = true;

    // Color 
    private Color alphaColor;

    void Start()
    {
        view = FindObjectOfType<View>();
        locationsList = FindObjectOfType<LocationsList>();
        bList = FindObjectOfType<BoreholeList>();
        // set scenario 2  texture  color;
        textColorBoreholeMode = Color.red;
        locationsList.InitLocationList();
        bList.InitBoreholeList();
    }

    //---------------------------------------
    public bool DetectLocation()
    {
        if (!isLocationDetected && DefaultTrackableEventHandler.isARMarkerDetected)
        {

            foreach (BoreholeLocation bLocation in locationsList.GetLocationsList())
            {
                if (bLocation._arMarkerId.Equals(DefaultTrackableEventHandler.markerNameDetected))
                {
                    Controller.sceneState = PogramFlowState.DefaultMode;
                    isLocationDetected = true;
                    view.txtCurrentLocation.text = bLocation._locationName;
                    Debug.Log("Location has been determined: " + bLocation._locationName);
                }
                else
                {
                    Debug.Log("No such a location in locationList");
                }
            }
        }
        return isLocationDetected;
    }
    public void Swipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;
                case TouchPhase.Ended:
                    float swipeDistVertical = (new Vector3(0, touch.position.y, 0) - new Vector3(0, startPos.y, 0)).magnitude;
                    if (swipeDistVertical > minSwipeDistY)
                    {
                        float swipeValue = Mathf.Sign(touch.position.y - startPos.y);
                        if (swipeValue > 0)//up Swipe
                        {
                            // Responsible for manual model move Up;
                            if (Controller.sceneState == PogramFlowState.IsScenario1)
                                view.scenario1ObjectScaling.transform.Translate(new Vector3(0, Time.deltaTime * 200, 0)); // move up;
                            else
                                view.scenario2ObjectScaling.transform.Translate(new Vector3(0, Time.deltaTime * 200, 0)); // move up; 
                        }
                        else if (swipeValue < 0)//down Swipe
                        {
                            // Responsible for manual model move Down;
                            if (Controller.sceneState == PogramFlowState.IsScenario1)
                                view.scenario1ObjectScaling.transform.Translate(new Vector3(0, -Time.deltaTime * 200, 0)); // move down;
                            else
                                view.scenario2ObjectScaling.transform.Translate(new Vector3(0, -Time.deltaTime * 200, 0)); // move down; 
                        }
                    }
                    float swipeDistHorizontal = (new Vector3(touch.position.x, 0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;
                    if (swipeDistHorizontal > minSwipeDistX)
                    {
                        float swipeValue = Mathf.Sign(touch.position.x - startPos.x);
                        if (swipeValue > 0)//right Swipe
                        {
                            // Responsible for manual model rotation;
                            if (Controller.sceneState == PogramFlowState.IsScenario1)
                                view.rotationCenterScenario1.transform.Rotate(new Vector3(0, -Time.deltaTime * 300, 0)); // Rotate Right;
                            else
                                view.rotationCenterScenario2.transform.Rotate(new Vector3(0, -Time.deltaTime * 300, 0)); // Rotate Right;                       
                        }

                        else if (swipeValue < 0)//left Swipe
                        {
                            // Responsible for manual model rotation;
                            if (Controller.sceneState == PogramFlowState.IsScenario1)
                                view.rotationCenterScenario1.transform.Rotate(new Vector3(0, Time.deltaTime * 300, 0)); // Rotate Left;
                            else
                                view.rotationCenterScenario2.transform.Rotate(new Vector3(0, Time.deltaTime * 300, 0)); // Rotate Left;
                        }
                    }
                    break;
            }
        }
    }
    public void GetTouch()
    {
        int nbTouches = Input.touchCount;

        if (nbTouches > 0)
        {
            for (int i = 0; i < nbTouches; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (touch.phase == TouchPhase.Began)
                {
                    if (touch.tapCount >= 2)
                    {
                        if (Controller.sceneState == PogramFlowState.IsScenario1 && Controller.sceneState == PogramFlowState.IsScenario1)
                        {
                            if (isMenuOn == true)
                            {
                                view.HideMainMenu();
                                isMenuOn = false;
                            }
                            else if (isMenuOn == false)
                            {
                                view.ShowMainMenu();
                                isMenuOn = true;
                            }
                        }

                        // Quit from ScenarioInfo mode;
                        if (Controller.sceneState == PogramFlowState.IsScenarioInfo)
                        {
                            Controller.sceneState = PogramFlowState.DefaultMode;
                        }
                    }
                    else
                    {

                    }
                }
            }
        }
        else
        {
            //StopMoving();
        }
    }
    public void MarkerDetection()
    {
        if (Controller.sceneState == PogramFlowState.IsScenario1 || Controller.sceneState == PogramFlowState.IsScenario2)
        {
            Controller.readyToPlay = true;
            view.HideSelectAppScenarioAnimation();

            //Checks if AR marker detected. If marker has been detected then shows data based on chosen scenario.
            if (DefaultTrackableEventHandler.isARMarkerDetected == true)
            {
                view.HideTargetGuide();
                if (Controller.sceneState == PogramFlowState.IsScenario1)
                    view.scenario1.SetActive(true);
                else if (Controller.sceneState == PogramFlowState.IsScenario2)
                    view.scenario2.SetActive(true);
            }
            else
            {
                view.ShowTargetGuide();
                view.scenario2.SetActive(false);
                view.scenario1.SetActive(false);
            }
        }
    }
    public void ChangeObjVisibility()
    {
        float dist = Vector3.Distance(view.measureDistanceTo.transform.position, transform.position);

        alphaColor = view.grassLayer.GetComponent<Renderer>().material.color;

        alphaColor = view.soilLayer1.GetComponent<Renderer>().material.color;
        alphaColor = view.soilLayer2.GetComponent<Renderer>().material.color;
        alphaColor = view.soilLayer3.GetComponent<Renderer>().material.color;
        alphaColor = view.soilLayer4.GetComponent<Renderer>().material.color;
        alphaColor = view.soilLayer5.GetComponent<Renderer>().material.color;
        alphaColor = view.soilLayer6.GetComponent<Renderer>().material.color;
        alphaColor = view.soilLayer7.GetComponent<Renderer>().material.color;
        alphaColor = view.soilLayer8.GetComponent<Renderer>().material.color;

        //Debug.Log(dist);
        if (dist > 680 && dist < 710)
        {
            float v = (1 + ((-dist) / 1000)) * 2.0f;
            alphaColor.a = v;
            //Debug.Log(v);
        }
        else if (dist > 710)
        {
            float v = (1 + ((-dist) / 1000)) * 1.1f;
            alphaColor.a = v;
            // Debug.Log(v);
        }
        else
        {
            alphaColor.a = 1f;
        }

        view.grassLayer.GetComponent<Renderer>().material.SetColor("_Color", alphaColor);

        view.soilLayer1.GetComponent<Renderer>().material.SetColor("_Color", alphaColor);
        view.soilLayer2.GetComponent<Renderer>().material.SetColor("_Color", alphaColor);
        view.soilLayer3.GetComponent<Renderer>().material.SetColor("_Color", alphaColor);
        view.soilLayer4.GetComponent<Renderer>().material.SetColor("_Color", alphaColor);
        view.soilLayer5.GetComponent<Renderer>().material.SetColor("_Color", alphaColor);
        view.soilLayer6.GetComponent<Renderer>().material.SetColor("_Color", alphaColor);
        view.soilLayer7.GetComponent<Renderer>().material.SetColor("_Color", alphaColor);
        view.soilLayer8.GetComponent<Renderer>().material.SetColor("_Color", alphaColor);
    }
    public void ObjectScaling()
    {
        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            if (Controller.sceneState == PogramFlowState.IsScenario1)
            {
                float x = view.scenario1ObjectScaling.gameObject.transform.localScale.x;
                if (x >= 0.0002 && x <= 0.002f)
                {
                    view.scenario1ObjectScaling.gameObject.transform.localScale -= new Vector3((deltaMagnitudeDiff / 700) * Time.deltaTime, (deltaMagnitudeDiff / 700) * Time.deltaTime, (deltaMagnitudeDiff / 700) * Time.deltaTime);
                }
                else if (x < 0.0002)
                {
                    view.scenario1ObjectScaling.gameObject.transform.localScale = new Vector3(0.000201f, 0.000201f, 0.000201f);
                }
                else if (x > 0.002)
                {
                    view.scenario1ObjectScaling.gameObject.transform.localScale = new Vector3(0.0019f, 0.0019f, 0.0019f);
                }
            }

            if (Controller.sceneState == PogramFlowState.IsScenario2)
            {
                float x = view.scenario2ObjectScaling.gameObject.transform.localScale.x;
                if (x >= 0.2 && x <= 1f)
                {
                    view.scenario2ObjectScaling.gameObject.transform.localScale -= new Vector3((deltaMagnitudeDiff / 15) * Time.deltaTime, (deltaMagnitudeDiff / 15) * Time.deltaTime, (deltaMagnitudeDiff / 15) * Time.deltaTime);
                }
                else if (x < 0.2)
                {
                    view.scenario2ObjectScaling.gameObject.transform.localScale = new Vector3(0.21f, 0.21f, 0.21f);
                }
                else if (x > 1)
                {
                    view.scenario2ObjectScaling.gameObject.transform.localScale = new Vector3(0.99f, 0.99f, 0.99f);
                }
            }
        }
    }
    //----------------------------------------

    //------Internet Connection------ 
    public IEnumerator CheckInternetConnection(float waitTime)
    {
        while (!isInternetConnectionEstablished)
        {
            allowCheckInternetConnection = false;
            string HtmlText = SendHttpRequest("http://www.google.com");
            if (HtmlText == "")
            {
                // No connection
                Debug.Log("NO Internet Connection");
                if (Controller.sceneState != PogramFlowState.IsScenarioInfo)
                {
                    view.ShowInternetConnectionErrorLabel();
                    view.ShowUsedStaticDataLabel();
                    view.HideLoadDataAnimation();
                    SetStaticSystemData();
                }
            }
            else if (!HtmlText.Contains("schema.org/WebPage"))
            {
                // Redirecting since the beginning of googles html contains that 
                // phrase and it was not find
            }
            else
            {
                // success
                Debug.Log("Internet Connection has been established");
                isInternetConnectionEstablished = true;
                Controller.loadAdditionalDataState = LoadAdditionalDataState.ObtainIp;
                view.HideInternetConnectionErrorLabel();
            }
            yield return new WaitForSeconds(waitTime);
        }
    }
    public string SendHttpRequest(string resource)
    {
        string html = string.Empty;
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
        try
        {
            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
                if (isSuccess)
                {
                    using (TextReader reader = new StreamReader(resp.GetResponseStream()))
                    {
                        char[] cs = new char[80];
                        reader.Read(cs, 0, cs.Length);
                        foreach (char ch in cs)
                        {
                            html += ch;
                        }
                    }
                }
            }
        }
        catch
        {
            return "";
        }
        return html;

    }
    public string GetLocalIP(string http)
    {
        string curIP = string.Empty;
        try
        {
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(http);
            myHttpWebRequest.Timeout = 3000;
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

            curIP = new WebClient().DownloadString(http);
            if (curIP != null)
            {
                Controller.loadAdditionalDataState = LoadAdditionalDataState.ObtainCurrentCityLoc;
                Debug.Log("Ip Address has been Obtained");
            }
            else
            {
                retrieveAdditionalData = false;
                view.HideLoadDataAnimation();
                Debug.Log("ERROR while obtaining Ip Address ");
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            view.HideLoadDataAnimation();
            retrieveAdditionalData = false;
        }
        // used to find external IP address.


        return curIP;
    }
    public string GetCurrentCityLocation(string currentIp)
    {
        var strFile = "";
        try
        {
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://freegeoip.net/json/" + currentIP);
            myHttpWebRequest.Timeout = 3000;
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

            StringBuilder output = new StringBuilder();
            using (var objClient = new System.Net.WebClient())
            {
                strFile = objClient.DownloadString("http://freegeoip.net/json/" + currentIP);
            }

            string encodedString = strFile;
            JSONObject j = new JSONObject(encodedString);
            ParseJSONData.accessData(j);
            strFile = ParseJSONData.strCity;
            Controller.loadAdditionalDataState = LoadAdditionalDataState.ObtainWeatherCondition;
            Debug.Log("Current city has been determined: " + strFile);
        }
        catch (Exception ex)
        {
            view.HideLoadDataAnimation();
            retrieveAdditionalData = false;
            retrieveAdditionalData = false;
            Debug.Log(ex.ToString());
        }
        return strFile;
    }
    //-------------------------------

    //-------------Data--------------
    public IEnumerator RetrieveWeatherInformationFromWebAPI(string city)
    {
        string url = "http://api.openweathermap.org/data/2.5/find?q=" + city + "&type=accurate&mode=xml&lang=nl&units=metric&appid=22e3dd43d3c0f04d5c7e31d6eefb0038";
        WWW www = new WWW(url);
        yield return www;

        view.HideLoadDataAnimation();
        if (www.error == null)
        {
            yield return new WaitForSeconds(1f);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(www.text);

            //Debug.Log("City: " + xmlDoc.SelectSingleNode("cities/list/item/city/@name").InnerText);
            //Debug.Log("Temperature: " + xmlDoc.SelectSingleNode("cities/list/item/temperature/@value").InnerText);
            string temp = xmlDoc.SelectSingleNode("cities/list/item/temperature/@value").InnerText;
            string[] words = temp.Split('.');
            view.temperature.text = words[0] + "°C";
            //Debug.Log("humidity: " + xmlDoc.SelectSingleNode("cities/list/item/humidity/@value").InnerText);
            // Debug.Log("Wind Speed : " + xmlDoc.SelectSingleNode("cities/list/item/wind/speed/@value").InnerText);
            //Debug.Log("Wind Direction : " + xmlDoc.SelectSingleNode("cities/list/item/wind/direction/@code").InnerText);
            // Debug.Log("Weather code : " + xmlDoc.SelectSingleNode("cities/list/item/weather/@icon").InnerText);
            string weatherCode = xmlDoc.SelectSingleNode("cities/list/item/weather/@icon").InnerText;
            if (weatherCode.Equals("01d") || weatherCode.Equals("01n"))
            {
                view.wIcon_01d.SetActive(true);
                view.wIcon_02d.SetActive(false);
                view.wIcon_03d.SetActive(false);
                view.wIcon_09d.SetActive(false);
                view.wIcon_10d.SetActive(false);
                view.wIcon_11d.SetActive(false);
                view.wIcon_13d.SetActive(false);
            }
            else if (weatherCode.Equals("02d"))
            {
                view.wIcon_01d.SetActive(false);
                view.wIcon_02d.SetActive(true);
                view.wIcon_03d.SetActive(false);
                view.wIcon_09d.SetActive(false);
                view.wIcon_10d.SetActive(false);
                view.wIcon_11d.SetActive(false);
                view.wIcon_13d.SetActive(false);
            }
            else if (weatherCode.Equals("03d"))
            {
                view.wIcon_01d.SetActive(false);
                view.wIcon_02d.SetActive(false);
                view.wIcon_03d.SetActive(true);
                view.wIcon_09d.SetActive(false);
                view.wIcon_10d.SetActive(false);
                view.wIcon_11d.SetActive(false);
                view.wIcon_13d.SetActive(false);
            }
            else if (weatherCode.Equals("09d"))
            {
                view.wIcon_01d.SetActive(false);
                view.wIcon_02d.SetActive(false);
                view.wIcon_03d.SetActive(false);
                view.wIcon_09d.SetActive(true);
                view.wIcon_10d.SetActive(false);
                view.wIcon_11d.SetActive(false);
                view.wIcon_13d.SetActive(false);
            }
            else if (weatherCode.Equals("10d"))
            {
                view.wIcon_01d.SetActive(false);
                view.wIcon_02d.SetActive(false);
                view.wIcon_03d.SetActive(false);
                view.wIcon_09d.SetActive(false);
                view.wIcon_10d.SetActive(true);
                view.wIcon_11d.SetActive(false);
                view.wIcon_13d.SetActive(false);
            }
            else if (weatherCode.Equals("11d"))
            {
                view.wIcon_01d.SetActive(false);
                view.wIcon_02d.SetActive(false);
                view.wIcon_03d.SetActive(false);
                view.wIcon_09d.SetActive(false);
                view.wIcon_10d.SetActive(false);
                view.wIcon_11d.SetActive(true);
                view.wIcon_13d.SetActive(false);
            }
            else if (weatherCode.Equals("13d"))
            {
                view.wIcon_01d.SetActive(false);
                view.wIcon_02d.SetActive(false);
                view.wIcon_03d.SetActive(false);
                view.wIcon_09d.SetActive(false);
                view.wIcon_10d.SetActive(false);
                view.wIcon_11d.SetActive(false);
                view.wIcon_13d.SetActive(true);
            }
            else
            {
                view.wIcon_01d.SetActive(false);
                view.wIcon_03d.SetActive(false);
                view.wIcon_09d.SetActive(false);
                view.wIcon_10d.SetActive(false);
                view.wIcon_11d.SetActive(false);
                view.wIcon_13d.SetActive(false);
                view.wIcon_nodata.SetActive(true);
            }
            Debug.Log("Weather Data Received Successfully");
        }
        else
        {
            view.HideLoadDataAnimation();
            retrieveAdditionalData = false;
            Debug.Log("ERROR: " + www.error);
        }
    }
    public void DataServerConnector(string http)
    {
        try
        {
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(http);
            myHttpWebRequest.Timeout = 3000;
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

            XmlDocument doc = new XmlDocument();
            doc.Load(http);
            XmlElement root = doc.DocumentElement;
            XmlNodeList warmnodes = root.SelectNodes("/Data/WarmData");
            XmlNodeList brinenodes = root.SelectNodes("/Data/BrineData");
            XmlNodeList perminnodes = root.SelectNodes("/Data/PerMinData");
            foreach (XmlNode node in warmnodes)
            {
                tempWarmIn = Convert.ToString(ConverterValue(node["WarmIn"].InnerText));
                tempWarmOut = Convert.ToString(ConverterValue(node["WarmOut"].InnerText));
            }
            foreach (XmlNode node in brinenodes)
            {
                tempBrineIn = Convert.ToString(ConverterValue(node["BrineIn"].InnerText));
                tempBrineOut = Convert.ToString(ConverterValue(node["BrineOut"].InnerText));
            }
            foreach (XmlNode node in perminnodes)
            {
                brineLitPerMin = Convert.ToString(ConverterValue(node["BrinePerMin"].InnerText));
                warmLitPerMin = Convert.ToString(ConverterValue(node["WarmPerMin"].InnerText));
            }
            Controller.runtimeDataState = LoadRuntimeDataState.UseRuntimeData;
            Debug.Log("Web Server Connection has been Established");
        }
        catch (Exception ex)
        {
            Controller.runtimeDataState = LoadRuntimeDataState.UseStaticData;
            Debug.Log(ex.ToString());
        }
    }
    public void SetStaticSystemData()
    {
        tempWarmIn = "27";
        tempWarmOut = "35";

        tempBrineIn = "7";
        tempBrineOut = "12";

        warmLitPerMin = "11";
        brineLitPerMin = "15";
    }
    //-------------------------------

    //------Display Data-------------
    public void SetEfficiencyBar_Scenario1(float efficiencyVIA13, float efficiencyVIA14)
    {
        view.DisplayEfficiencyData_Scenario1(efficiencyVIA13, efficiencyVIA14);
    }
    public void SetEfficiencyBar_Scenario2(float efficiencyVIA14)
    {
        view.DisplayEfficiencyData_Scenario2(efficiencyVIA14);
    }
    public void BuildSoillayerDescription_Scenario2(string boreholeId)
    {
        foreach (Borehole borehole in bList.GetBoreholesList())
        {
            if (borehole._boreholeId.Equals(boreholeId))
            {
                Borehole tempBorehole = borehole;

                List<SoilLayer> slayers = tempBorehole.GetSoilLayersList();
                int i = 0;
                if (slayers != null)
                {
                    foreach (SoilLayer layer in slayers)
                    {
                        TextMesh txtLayer_i_height;
                        if (i == 0)
                        {
                            txtLayer_i_height = GameObject.Find("txtLayer_" + i + "_height").GetComponent<TextMesh>();
                            txtLayer_i_height.text = slayers[i]._height;

                            txtLayer_i_height.color = textColorBoreholeMode;
                            i++;
                        }
                        else
                        {
                            txtLayer_i_height = GameObject.Find("txtLayer_" + i + "_height").GetComponent<TextMesh>();
                            txtLayer_i_height.text = slayers[i]._height;
                            txtLayer_i_height.color = textColorBoreholeMode;

                            TextMesh txtLayer_i_description = GameObject.Find("txtLayer_" + i + "_description").GetComponent<TextMesh>();
                            txtLayer_i_description.text = slayers[i]._descriptionSoilLayer;
                            txtLayer_i_description.color = textColorBoreholeMode;

                            TextMesh txtLayer_i_name = GameObject.Find("txtLayer_" + i + "_name").GetComponent<TextMesh>();
                            txtLayer_i_name.text = slayers[i]._nameSoilLayer;
                            txtLayer_i_name.color = textColorBoreholeMode;
                            i++;
                        }
                    }
                }
                else
                {
                    Debug.Log("Borehole: " + boreholeId + " does not contain soillayers data");
                }
            }
            else
            {
                Debug.Log("No borehole: " + boreholeId + " in borehole list.");
            }
        }
    }

    public void DisplayRuntimeData_Scenario1()
    {
        if (tempBrineIn != null && tempWarmIn != null)
        {
            view.tempInScenario1Brine.text = tempBrineIn + " °C";
            view.tempOutScenario1Brine.text = tempBrineOut + " °C";
            view.waterFlowSpeedScenario1Brine.text = brineLitPerMin + " l/m";
            view.tempInScenario1Varme.text = tempWarmIn + " °C";
            view.tempOutScenario1Varme.text = tempWarmOut + " °C";
            view.waterFlowSpeedScenario1Varme.text = warmLitPerMin + " l/m";
        }
    }
    public void DisplayRuntimeData_ScenarioInfo()
    {
        if (tempBrineIn != null && tempWarmIn != null)
        {
            view.infopage_tempBrineIn.text = tempBrineIn + " °C";
            view.infopage_tempBrineOut.text = tempBrineOut + " °C";
            view.infopage_waterFlowSpeedBrine.text = brineLitPerMin + " l/m";
            view.infopage_tempVarmeIn.text = tempWarmIn + " °C";
            view.infopage_tempVarmeOut.text = tempWarmOut + " °C";
            view.infopage_waterFlowSpeedVarme.text = warmLitPerMin + " l/m";
        }
    }
    public void DisplayRuntimeData_Scenario2(string boreholeId)
    {
        foreach (Borehole borehole in bList.GetBoreholesList())
        {
            if (borehole._boreholeId.Equals(boreholeId) && tempWarmIn != null)
            {
                view.tempInScenario2.text = tempWarmIn + " °C";
                view.tempOutScenario2.text = tempWarmOut + " °C";
                view.waterFlowSpeedScenario2.text = warmLitPerMin + " l/m";
                //boreholeName.text = keyValuePair.Key;
                view.boreholeName.text = "VIA 14";
            }
            else
            {
                view.tempInScenario1Varme.text = "24 °C";
                view.tempOutScenario1Varme.text = "21 °C";
                view.waterFlowSpeedScenario1Varme.text = "19 l/m";
                view.boreholeName.text = "VIA 14";
            }
        }
    }
    //-------------------------------

    public static int ConverterValue(string arg)
    {
        double val = Convert.ToDouble(arg);
        byte[] inValue = new byte[8];
        byte[] outValue = new byte[8];

        inValue = BitConverter.GetBytes(val);

        outValue[0] = inValue[4];
        outValue[1] = inValue[5];
        outValue[2] = inValue[6];
        outValue[3] = inValue[7];
        outValue[4] = inValue[0];
        outValue[5] = inValue[1];
        outValue[6] = inValue[2];
        outValue[7] = inValue[3];
        double result = BitConverter.ToDouble(outValue, 0);
        int fResult = Convert.ToInt32(result);
        return fResult;
    }
    public IEnumerator PlayOnStartAppAnimation(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        view.onStartAppAnimation.SetActive(false);
    }
    public void PlaySoundOnButtonClick()
    {
        btnClick.Play();
    }
    public void PlaySoundOnToggleClick()
    {
        toggleClick.Play();
    }
    public void ExitApplication()
    {
        Application.Quit();
    }
}
