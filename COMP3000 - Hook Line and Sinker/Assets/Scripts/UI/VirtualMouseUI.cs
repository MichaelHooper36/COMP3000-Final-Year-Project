using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;

public class VirtualMouseUI : MonoBehaviour
{
    [SerializeField] private RectTransform canvasTransform;
    private VirtualMouseInput virtualMouseInput;

    void Awake()
    {
        virtualMouseInput = GetComponent<VirtualMouseInput>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Ensures a consistent scale for the virtual mouse UI regardless of the canvas scale.
        transform.localScale = Vector3.one * (1 / canvasTransform.localScale.x);
        transform.SetAsLastSibling();
    }

    private void LateUpdate()
    {
        // Clamps the virtual mouse position to the screen bounds to prevent it from going off-screen.
        Vector2 virtualMousePosition = virtualMouseInput.virtualMouse.position.value;
        virtualMousePosition.x = Mathf.Clamp(virtualMousePosition.x, 0, Screen.width);
        virtualMousePosition.y = Mathf.Clamp(virtualMousePosition.y, 0, Screen.height);
        InputState.Change(virtualMouseInput.virtualMouse.position, virtualMousePosition);
    }
}
