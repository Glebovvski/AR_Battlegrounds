using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(Light))]
public class AmbientLightEstimation : MonoBehaviour
{
    private Light lightComponent;
    public ARCameraManager manager;
    // Start is called before the first frame update
    

    private void OnEnable()
    {
        lightComponent = GetComponent<Light>();
        manager = GetComponent<ARCameraManager>();
        manager.frameReceived += OnCameraFrameReceived;
    }

    void OnCameraFrameReceived(ARCameraFrameEventArgs camFrameEvent)
    {
        ARLightEstimationData led = camFrameEvent.lightEstimation;

        if (led.averageBrightness.HasValue) lightComponent.intensity = led.averageBrightness.Value;
        if (led.averageColorTemperature.HasValue) lightComponent.colorTemperature = led.averageColorTemperature.Value;
    }

    private void OnDisable()
    {
        manager.frameReceived -= OnCameraFrameReceived;
    }
}
