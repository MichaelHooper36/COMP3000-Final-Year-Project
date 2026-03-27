using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;

public class VirtualMouseUI : MonoBehaviour
{
    [SerializeField] private RectTransform canvasTransform;
    private VirtualMouseInput virtualMouseInput;
    private GameObject mouseImage;

    void Awake()
    {
        virtualMouseInput = GetComponent<VirtualMouseInput>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mouseImage = transform.GetChild(0).gameObject;
         if (GameControl.gameControl.device == GameControl.Device.Controller)
         {
             mouseImage.SetActive(true);
             Cursor.visible = false;
         }
         else
         {
             mouseImage.SetActive(false);
             Cursor.visible = true;
         }
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.one * (1 / canvasTransform.localScale.x);
        transform.SetAsLastSibling();

        if (GameControl.gameControl.device == GameControl.Device.Controller)
        {
            mouseImage.SetActive(true);
            Cursor.visible = false;
        }
        else
        {
            mouseImage.SetActive(false);
            Cursor.visible = true;
        }
    }

    private void LateUpdate()
    {
        Vector2 virtualMousePosition = virtualMouseInput.virtualMouse.position.value;
        virtualMousePosition.x = Mathf.Clamp(virtualMousePosition.x, 0, Screen.width);
        virtualMousePosition.y = Mathf.Clamp(virtualMousePosition.y, 0, Screen.height);
        InputState.Change(virtualMouseInput.virtualMouse.position, virtualMousePosition);
    }
}
