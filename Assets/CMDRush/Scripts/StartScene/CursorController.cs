using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;

public class CursorController : MonoBehaviour
{
    public int playerNumber;
    public Gamepad gamepad;
    [SerializeField] float speed;
    [SerializeField] Canvas cursorCanvas;

    float horizontal;
    float vertical;
    bool enterFlag;

    RectTransform rectTransform;
    Image image;
    Camera uiCamera;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        image.enabled = false;

        if (cursorCanvas == null)
        {
            cursorCanvas = GetComponentInParent<Canvas>();
        }

        // CanvasのRenderModeがOverlay以外（Camera / WorldSpace）の場合はカメラを取得
        if (cursorCanvas != null && cursorCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            uiCamera = cursorCanvas.worldCamera;
        }
    }

    void Update()
    {
        CursorControl();
        Enter();
    }

    void CursorControl()
    {
        Vector3 move = new Vector2(horizontal * speed, vertical * speed) * Time.deltaTime;
        Vector3 posi = transform.GetComponent<RectTransform>().localPosition;

        if (!image.enabled && move != Vector3.zero) image.enabled = true;
        if (posi.x + move.x <= -960f || posi.x + move.x >= 960f) move.x = 0f;
        if (posi.y + move.y <= -540f || posi.y + move.y >= 540f) move.y = 0f;
        transform.GetComponent<RectTransform>().localPosition += move;
    }

    void Enter()
    {
        if (enterFlag)
        {
            ExecuteEventTriggerAtCursor();
            enterFlag = false;
        }
    }

    void ExecuteEventTriggerAtCursor()
    {
        if (EventSystem.current == null) return;

        // 1. カーソルのワールド座標をスクリーン座標（画面上のピクセル座標）に変換
        // ※別Canvasであってもスクリーン座標を基準にすれば正確に重なりを判定できる
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(uiCamera, rectTransform.position);

        // 2. その座標にあるすべてのUI要素をレイキャストで自動取得
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPos
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        // 3. 手前にあるUIから順に調べ、EventTrigger（またはクリック対象）があれば実行
        foreach (RaycastResult result in results)
        {
            // 自分自身（カーソル画像）にRaycastTargetが残っていた場合はスキップ
            if (result.gameObject == gameObject) continue;

            // 親オブジェクトにEventTriggerが付いている場合も考慮して取得
            EventTrigger trigger = result.gameObject.GetComponentInParent<EventTrigger>();
            if (trigger != null)
            {
                // EventTriggerの「PointerClick」を発火させる
                ExecuteEvents.Execute(trigger.gameObject, pointerData, ExecuteEvents.pointerClickHandler);
                break; // 一番手前の1つだけ実行して終了
            }
        }
    }

    public void CursorMove(InputAction.CallbackContext context)
    {
        if (Gamepad.all.Count == 0)
        {
            if (context.control.path == "/Keyboard/w" ||
                context.control.path == "/Keyboard/a" ||
                context.control.path == "/Keyboard/s" ||
                context.control.path == "/Keyboard/d")
            {
                horizontal = context.ReadValue<Vector2>().x;
                vertical = context.ReadValue<Vector2>().y;
            }
            else if (context.control.path == "/Keyboard/upArrow" ||
                context.control.path == "/Keyboard/leftArrow" ||
                context.control.path == "/Keyboard/downArrow" ||
                context.control.path == "/Keyboard/rightArrow")
            {
                horizontal = context.ReadValue<Vector2>().x;
                vertical = context.ReadValue<Vector2>().y;
            }
        }
        else if (Gamepad.all.Count == 1)
        {
            var inputDevice = context.control.device;
            if (playerNumber == 0 && gamepad.deviceId == inputDevice.deviceId)
            {
                horizontal = context.ReadValue<Vector2>().x;
                vertical = context.ReadValue<Vector2>().y;
            }
            else if (context.control.path == "/Keyboard/upArrow" || 
                context.control.path == "/Keyboard/leftArrow" || 
                context.control.path == "/Keyboard/downArrow" || 
                context.control.path == "/Keyboard/rightArrow")
            {
                horizontal = context.ReadValue<Vector2>().x;
                vertical = context.ReadValue<Vector2>().y;
            }
        }
        else
        {
            var inputDevice = context.control.device;
            if (gamepad.deviceId == inputDevice.deviceId)
            {
                horizontal = context.ReadValue<Vector2>().x;
                vertical = context.ReadValue<Vector2>().y;
            }
        }       
    }

    public void CursorEnter(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (Gamepad.all.Count == 0)
            {
                if (context.control.path == "/Keyboard/e")
                {
                    enterFlag = true;
                }
                else if (context.control.path == "/Keyboard/rightShift")
                {
                    enterFlag = true;
                }
            }
            else if (Gamepad.all.Count == 1)
            {
                var inputDevice = context.control.device;
                if (playerNumber == 0 && gamepad.deviceId == inputDevice.deviceId)
                {
                    enterFlag = true;
                }
                else if (context.control.path == "/Keyboard/rightShift")
                {
                    enterFlag = true;
                }
            }
            else
            {
                var inputDevice = context.control.device;
                if (gamepad.deviceId == inputDevice.deviceId)
                {
                    enterFlag = true;
                }
            }
        }        
    }
}
