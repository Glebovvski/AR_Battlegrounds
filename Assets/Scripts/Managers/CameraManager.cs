using System;
using Models;
using UnityEngine;
using ViewModels;
using Zenject;

public class CameraManager : MonoBehaviour
{
    private GameTimeModel GameTimeModel { get; set; }
    private GameModeModel GameModeModel { get; set; }
    private DefensesViewModel DefensesViewModel { get; set; }


    [SerializeField] private Camera camera;
    [SerializeField] private Camera arCamera;

    [Inject]
    private void Construct(GameTimeModel gameTimeModel, GameModeModel gameModeModel, DefensesViewModel defensesViewModel)
    {
        GameTimeModel = gameTimeModel;
        GameModeModel = gameModeModel;
        DefensesViewModel = defensesViewModel;
    }

    private Vector3 touchStart;
    private float dragSpeed = 20;

    private float zoomSpeed = 0.5f;
    private int minZoom = 2;
    private int maxZoom = 20;

    private void Start()
    {
        SelectCamera();

        GameModeModel.OnChangeMode += SelectCamera;
        DefensesViewModel.OnResetCameraClick += Reset;
    }

    private void SelectCamera()
    {
        bool IsARModeSelected = GameModeModel.IsARModeSelected;

        TurnOffCameras();

        camera.gameObject.SetActive(!IsARModeSelected);
        arCamera.gameObject.SetActive(IsARModeSelected);
    }

    private void TurnOffCameras()
    {
        camera.gameObject.SetActive(false);
        arCamera.gameObject.SetActive(false);
    }

    private void Reset()
    {
        Camera.main.fieldOfView = 90;
        Camera.main.transform.position = new Vector3(0, 7, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameModeModel.IsARModeSelected) return;

#if UNITY_EDITOR
        if (Input.GetMouseButton(1))
        {
            float speed = dragSpeed * (Time.deltaTime / Time.timeScale);
            Camera.main.transform.position -= new Vector3(Input.GetAxis("Mouse X") * speed, 0, Input.GetAxis("Mouse Y") * speed);
        }

#elif PLATFORM_IOS || PLATFORM_ANDROID
        if (Input.touchCount == 1)
        {
            //MOVING
            var touch = Input.GetTouch(0);
            float speed = (Time.deltaTime / Time.timeScale);
            Camera.main.transform.position -= new Vector3(touch.deltaPosition.x * speed, 0, touch.deltaPosition.y * speed);

        }
        else if (Input.touchCount == 2)
        {
            //ZOOMING
            var touch0 = Input.GetTouch(0);
            var touch1 = Input.GetTouch(1);

            Vector2 touch0_prev = touch0.position - touch0.deltaPosition;
            Vector2 touch1_prev = touch1.position - touch1.deltaPosition;

            float prevMagnitude = (touch0_prev - touch1_prev).sqrMagnitude;
            float currMagnitude = (touch0.position - touch1.position).sqrMagnitude;

            float diff = currMagnitude - prevMagnitude;

            Zoom(diff * 0.01f);

        }
#endif
    }

    private void Zoom(float v)
    {
#if PLATFORM_ANDROID
        Camera.main.fieldOfView += v * zoomSpeed;
#elif PLATFORM_IOS
        Camera.main.fieldOfView += v * zoomSpeed;
#endif
        // Clamp the field of view to make sure it's between 0 and 180.
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 0.1f, 179.9f);
    }

    private void OnDestroy()
    {
        GameModeModel.OnChangeMode -= SelectCamera;
    }
}
