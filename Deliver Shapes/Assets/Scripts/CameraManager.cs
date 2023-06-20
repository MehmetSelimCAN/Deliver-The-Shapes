using UnityEngine;

public class CameraManager : MonoBehaviour {

    private Camera mainCamera;

    private Vector3 origin;
    private Vector3 difference;

    private bool isDragging;
    private const int CAMERA_MIN_X = -25;
    private const int CAMERA_MAX_X = 25;
    private const int CAMERA_MIN_Y = -50;
    private const int CAMERA_MAX_Y = 50;

    private float zoomMagnitude = 12f;

    private void Awake() {
        mainCamera = Camera.main;
    }

    private void Update() {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f) {
            mainCamera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * zoomMagnitude;
            mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, 3, 25);
        }
    }

    private void LateUpdate() {
        if (Input.GetMouseButton(0)) {
            difference = GetMousePosition() - mainCamera.transform.position;

            if (!isDragging) {
                isDragging = true;
                origin = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            isDragging = false;
        }

        if (isDragging) {
            mainCamera.transform.position = origin - difference;

            mainCamera.transform.position = new Vector3(
              Mathf.Clamp(mainCamera.transform.position.x, CAMERA_MIN_X, CAMERA_MAX_X),
              Mathf.Clamp(mainCamera.transform.position.y, CAMERA_MIN_Y, CAMERA_MAX_Y),
              -10f);
        }
    }

    private Vector3 GetMousePosition() {
        return (mainCamera.ScreenToWorldPoint(Input.mousePosition));
    }
}
