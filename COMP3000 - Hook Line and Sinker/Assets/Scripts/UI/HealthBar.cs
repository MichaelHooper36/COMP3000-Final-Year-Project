using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public int GetMaxHealth()
    {
        return (int)slider.maxValue;
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.minValue = 0;
        slider.value = health;
    }

    public int GetCurrentHealth()
    {
        return (int)slider.value;
    }

    public void SetCurrentHealth(int health)
    {
        Debug.Log("HealthBar setting current health to: " + health);
        slider.value = health;
        Canvas.ForceUpdateCanvases();
        Debug.Log("HealthBar current health is now: " + slider.value);
    }
}
