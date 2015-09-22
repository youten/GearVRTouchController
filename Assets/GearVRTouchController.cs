using UnityEngine;
using System.Collections;

// TouchPad Controller for Gear VR
// Usage: Attach this script to Camera's parent.
// 
// - MOVE MODE
// -- drug front-back, MainCamera move front-back.
// -- drug up-down, MainCamera move up-down.
// - ROTATE MODE (when toggle enable)
// -- drug front-back, MainCamera rotate Y-Axis.
// -- drug up-down, MainCamera rotate X-Axis.
//
public class GearVRTouchController : MonoBehaviour {
	// If true, you can toggle MOVE MODE/ROTATE MODE by double tap.
    public bool toggleRotateMode = false;
	// If set, rotateModeObject activated only when in ROTATE MODE.
    public GameObject rotateModeObject;

    // for move mode
    private const float MOVE_RATE = 0.02f;
	private bool isDrugging = false;
	private float mouseStartX;
	private float mouseStartY;
	private float mouseMoveX;
	private float mouseMoveY;
    private Vector3 startPosition;
    private Vector3 startEulerAngles;

    // for rotate mode
    private bool isRotateMode = false;
    private const float DOUBLE_CLICK_SECOND = 0.35f;
    private float privClickTime;

    // Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown (0)) {
            if (toggleRotateMode) {
                float nowTime = Time.realtimeSinceStartup;
				// when double tap
                if ((nowTime - privClickTime) < DOUBLE_CLICK_SECOND) {
                    // toggle mode
                    isRotateMode = !isRotateMode;
                    if (rotateModeObject) {
                        rotateModeObject.SetActive(isRotateMode);
                    }
                }
                privClickTime = nowTime;
            }

            if (!isDrugging) {
                // start drugging
                isDrugging = true;
                mouseStartX = Input.mousePosition.x;
                mouseStartY = Input.mousePosition.y;
                mouseMoveX = 0;
                mouseMoveY = 0;
                startPosition = transform.position;
                startEulerAngles = transform.eulerAngles;
            }
        } else if (Input.GetMouseButtonUp (0)) {
            isDrugging = false;
        }

        if (isDrugging) {
            float deltaY = Input.mousePosition.y - mouseStartY - mouseMoveY;
            if ((deltaY < -0.1f) || (0.1f < deltaY)) {
                mouseMoveY += deltaY;
            }
            float deltaX = Input.mousePosition.x - mouseStartX - mouseMoveX;
            if ((deltaX < -0.1f) || (0.1f < deltaX)) {
                mouseMoveX += deltaX;
            }
            if (isRotateMode) {
                transform.rotation = Quaternion.Euler(new Vector3(
                    startEulerAngles.x - mouseMoveY,
                    startEulerAngles.y + mouseMoveX,
                    startEulerAngles.z));
            } else {
                Vector3 pos = new Vector3(
                    startPosition.x,
                    startPosition.y + mouseMoveY * MOVE_RATE, 
                    startPosition.z + mouseMoveX * MOVE_RATE);
                transform.position = pos;
            }
        }
	}
}
