using UnityEngine;

public class ParseJSONData
{
    public static string strCity = "";

    public static void accessData(JSONObject obj)
    {

        switch (obj.type)
        {
            case JSONObject.Type.OBJECT:
                for (int i = 0; i < obj.list.Count; i++)
                {
                    string key = (string)obj.keys[i];
                    JSONObject j = (JSONObject)obj.list[i];
                    if (key.Contains("city"))
                    {
                        strCity = j.ToString();
                    }
                    accessData(j);
                }
                break;
            case JSONObject.Type.ARRAY:
                foreach (JSONObject j in obj.list)
                {

                    accessData(j);
                }
                break;
            case JSONObject.Type.STRING:
                //if(obj.str)
                // str = obj.str;
                //Debug.Log(str);
                //c++;
                break;
            case JSONObject.Type.NUMBER:
                // Debug.Log(obj.n);
                break;
            case JSONObject.Type.BOOL:
                // Debug.Log(obj.b);
                break;
            case JSONObject.Type.NULL:
                Debug.Log("NULL");
                break;

        }

    }
}