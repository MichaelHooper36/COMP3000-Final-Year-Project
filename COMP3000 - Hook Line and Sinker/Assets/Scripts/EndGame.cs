using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndGame : MonoBehaviour
{
    public Scene scene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            if (scene.name == "levelOne")
            {
                Debug.Log("Level One Complete!");
                float finalTime = GameControl.gameControl.levelOneTimer;
                float personalBest = GameControl.gameControl.levelOneBest;
                Debug.Log("Previous PB: " + personalBest);
                Debug.Log("New time: " + finalTime);
                if ((finalTime < personalBest && finalTime != 0 && personalBest != 0) || (finalTime != 0 && personalBest == 0))
                {
                    GameControl.gameControl.levelOneBest = finalTime;
                }
                Debug.Log("New PB: " + GameControl.gameControl.levelOneBest);

                GameControl.gameControl.levelOneTimer = 0;
                GameControl.gameControl.levelOneRespawnX = -16;
                GameControl.gameControl.levelOneRespawnY = 3;
                GameControl.gameControl.Save();
                GameControl.gameControl.Load();
                Debug.Log(GameControl.gameControl.levelOneRespawnX);
                Debug.Log(GameControl.gameControl.levelOneRespawnY);
            }
            else if (scene.name == "levelTwo")
            {
                float finalTime = GameControl.gameControl.levelTwoTimer;
                float personalBest = GameControl.gameControl.levelTwoBest;
                Debug.Log("Previous PB: " + personalBest);
                Debug.Log("New time: " + finalTime);
                if ((finalTime < personalBest && finalTime != 0 && personalBest != 0) || (finalTime != 0 && personalBest == 0))
                {
                    GameControl.gameControl.levelTwoBest = finalTime;
                }
                Debug.Log("New PB: " + GameControl.gameControl.levelTwoBest);

                GameControl.gameControl.levelTwoTimer = 0;
                GameControl.gameControl.levelTwoRespawnX = -16;
                GameControl.gameControl.levelTwoRespawnY = 3;
                GameControl.gameControl.Save();
                GameControl.gameControl.Load();
            }
            else if (scene.name == "levelThree")
            {
                float finalTime = GameControl.gameControl.levelThreeTimer;
                float personalBest = GameControl.gameControl.levelThreeBest;
                Debug.Log("Previous PB: " + personalBest);
                Debug.Log("New time: " + finalTime);
                if ((finalTime < personalBest && finalTime != 0 && personalBest != 0) || (finalTime != 0 && personalBest == 0))
                {
                    GameControl.gameControl.levelThreeBest = finalTime;
                }
                Debug.Log("New PB: " + GameControl.gameControl.levelThreeBest);

                GameControl.gameControl.levelThreeTimer = 0;
                GameControl.gameControl.levelThreeRespawnX = -16;
                GameControl.gameControl.levelThreeRespawnY = 3;
                GameControl.gameControl.Save();
                GameControl.gameControl.Load();
            }

            SceneManager.LoadScene("Title Screen");
        }
    }
}
