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
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("Title Screen");
        }
    }
}
