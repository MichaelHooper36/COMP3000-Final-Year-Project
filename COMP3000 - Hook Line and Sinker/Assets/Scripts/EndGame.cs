using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndGame : MonoBehaviour
{
    public GameControl gameControl;
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
                float finalTime = gameControl.levelOneTimer;
                float personalBest = gameControl.levelOneBest;
                Debug.Log("Previous PB: " + personalBest);
                Debug.Log("New time: " + finalTime);
                if ((finalTime < personalBest && finalTime != 0 && personalBest != 0) || (finalTime != 0 && personalBest == 0))
                {
                    gameControl.levelOneBest = finalTime;
                }
                Debug.Log("New PB: " + gameControl.levelOneBest);

                gameControl.levelOneTimer = 0;
                gameControl.levelOneRespawnX = -16;
                gameControl.levelOneRespawnY = 3;
                gameControl.Save();
            }
            else if (scene.name == "levelTwo")
            {
                float finalTime = gameControl.levelTwoTimer;
                float personalBest = gameControl.levelTwoBest;
                Debug.Log("Previous PB: " + personalBest);
                Debug.Log("New time: " + finalTime);
                if ((finalTime < personalBest && finalTime != 0 && personalBest != 0) || (finalTime != 0 && personalBest == 0))
                {
                    gameControl.levelTwoBest = finalTime;
                }
                Debug.Log("New PB: " + gameControl.levelTwoBest);

                gameControl.levelTwoTimer = 0;
                gameControl.levelTwoRespawnX = -16;
                gameControl.levelTwoRespawnY = 3;
                gameControl.Save();
            }
            if (scene.name == "levelThree")
            {
                float finalTime = gameControl.levelThreeTimer;
                float personalBest = gameControl.levelThreeBest;
                Debug.Log("Previous PB: " + personalBest);
                Debug.Log("New time: " + finalTime);
                if ((finalTime < personalBest && finalTime != 0 && personalBest != 0) || (finalTime != 0 && personalBest == 0))
                {
                    gameControl.levelThreeBest = finalTime;
                }
                Debug.Log("New PB: " + gameControl.levelThreeBest);

                gameControl.levelThreeTimer = 0;
                gameControl.levelThreeRespawnX = -16;
                gameControl.levelThreeRespawnY = 3;
                gameControl.Save();
            }

            SceneManager.LoadScene("Title Screen");
        }
    }
}
