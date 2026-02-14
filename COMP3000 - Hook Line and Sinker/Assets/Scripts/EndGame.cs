using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            float finalTime = PlayerPrefs.GetFloat("Level 1 timer");
            float personalBest = PlayerPrefs.GetFloat("Level 1 PB");
            Debug.Log("Previous PB: " + personalBest);
            Debug.Log("New time: " + finalTime);
            if ((finalTime < personalBest && finalTime != 0 && personalBest != 0) || (finalTime != 0 && personalBest == 0))
            {
                PlayerPrefs.SetFloat("Level 1 PB", finalTime);
            }
            Debug.Log("New PB: " + PlayerPrefs.GetFloat("Level 1 PB"));

            PlayerPrefs.DeleteKey("Level 1 timer");
            PlayerPrefs.DeleteKey("RespawnX");
            PlayerPrefs.DeleteKey("RespawnY");
            SceneManager.LoadScene("Title Screen");
        }
    }
}
