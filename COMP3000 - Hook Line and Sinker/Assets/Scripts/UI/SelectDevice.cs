using UnityEngine;
using TMPro;

public class SelectDevice : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown device_dropdown;
    [SerializeField] private GameObject cursorImage;

    public void Start()
    {
        if (GameControl.gameControl == null || device_dropdown == null)
        {
            return;
        }

        int selectedIndex = GameControl.gameControl.device == GameControl.Device.Controller ? 1 : 0;
        device_dropdown.SetValueWithoutNotify(selectedIndex);
        device_dropdown.RefreshShownValue();
    }

    public void SelectActiveDevice(int index)
    {
        if (index == 1)
        {
            GameControl.gameControl.device = GameControl.Device.Controller;
            cursorImage.SetActive(true);
            Cursor.visible = false;
        }
        else
        {
            GameControl.gameControl.device = GameControl.Device.Keyboard;
            cursorImage.SetActive(false);
            Cursor.visible = true;
        }
        GameControl.gameControl.Save();
    }
}
