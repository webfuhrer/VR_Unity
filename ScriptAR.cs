using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ScriptAR : MonoBehaviour
{
    private ARTrackedImageManager _trackedImagesManager;
    // Start is called before the first frame update
    //Los prefabs se 
    public GameObject[] ArPrefabs;
    private readonly Dictionary<string, GameObject> _instantiatedPrefabs=new Dictionary<string, GameObject>();
    //Esto es cuando se inicia:
    void Awake()
    {
        _trackedImagesManager=GetComponent<ARTrackedImageManager>();
    }
    void OnEnable()
    {
        //Attach event handler
        _trackedImagesManager.trackedImagesChanged+=OnTrackedImagesChanged;
    }
     void OnDisable()
    {//dettach event handler
        _trackedImagesManager.trackedImagesChanged-=OnTrackedImagesChanged;
    }
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach(var trackedImage in eventArgs.added)
            {
                var imageName=trackedImage.referenceImage.name;
                foreach(var curPrefab in ArPrefabs)
                {
                    if(string.Compare(curPrefab.name, imageName,StringComparison.OrdinalIgnoreCase )==0 && !_instantiatedPrefabs.ContainsKey(imageName))
                        {
                            var newPrefab=Instantiate(curPrefab, trackedImage.transform);
                            _instantiatedPrefabs[imageName]=newPrefab;
                        }
                }
            }
    foreach(var trackedImage in eventArgs.updated)
        {
            _instantiatedPrefabs[trackedImage.referenceImage.name].SetActive(trackedImage.trackingState==TrackingState.Tracking);
        }
    foreach(var trackedImage in eventArgs.removed){
        Destroy(_instantiatedPrefabs[trackedImage.referenceImage.name]);
        _instantiatedPrefabs.Remove(trackedImage.referenceImage.name);
    }

    }
  
}
