using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtilities
{
    public static bool Save<T>(T objectToSave, string path)
    {
        try
        {
            string json = JsonUtility.ToJson(objectToSave, true);
            File.WriteAllText(path, json);

            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            return false;
        }
    }

    public static T Load<T>(string path)
    {
        try
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<T>(json);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            return default;
        }
    }
}