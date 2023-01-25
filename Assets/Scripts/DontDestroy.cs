using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public string ObjectName;
    static HashSet<string> objectsLoaded;

    void Awake()
    {
        if (objectsLoaded == null)
            objectsLoaded = new HashSet<string>();

        if (ObjectName == "")
        {
            Debug.LogError("Dont destroy object has no ObjectName");
            return;
        }

        if (!objectsLoaded.Contains(ObjectName))
        {
            objectsLoaded.Add(ObjectName);
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
}
