using UnityEngine;

public class SceneObjectDestroyer : MonoBehaviour
{
    public string objectName = "Audio"; // Name of the object to destroy
    public string objectName_2 = "Audio_Ambient";

    void Start()
    {
        GameObject obj = GameObject.Find(objectName);
        if (obj != null)
        {
            Destroy(obj);
            Debug.Log("Persistent object destroyed: " + objectName);
        }
        else
        {
            Debug.Log("Object not found: " + objectName);
        }

        GameObject obj2 = GameObject.Find(objectName_2);
        if (obj2 != null)
        {
            Destroy(obj2);
            Debug.Log("Persistent object destroyed: " + objectName_2);
        }
        else
        {
            Debug.Log("Object not found: " + objectName_2);
        }
    }

    private void Update()
    {
            
    }
}
