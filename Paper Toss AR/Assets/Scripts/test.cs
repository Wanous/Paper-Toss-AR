using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))] // Forces Unity to add it
public class test : MonoBehaviour
{
    void Start()
    {
        // This will add ARRaycastManager if missing
        if (GetComponent<ARRaycastManager>() == null)
        {
            gameObject.AddComponent<ARRaycastManager>();
            Debug.Log("ARRaycastManager added manually!");
        }
    }
}
