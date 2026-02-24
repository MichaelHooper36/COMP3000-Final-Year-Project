using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameControl : MonoBehaviour
{
    public static GameControl gameControl;

    public List<int> projectiles = new List<int> { 0 };
    public int projectileIndex;

    public float levelOneRespawnX = -16;
    public float levelOneRespawnY = 3;
    public float levelTwoRespawnX;
    public float levelTwoRespawnY;
    public float levelThreeRespawnX;
    public float levelThreeRespawnY;

    public float levelOneTimer;
    public float levelTwoTimer;
    public float levelThreeTimer;

    public float levelOneBest;
    public float levelTwoBest;
    public float levelThreeBest;

    public float timePlayed;
    public float totalKills;
    public float totalDeaths;

    void Awake()
    {
        if (gameControl == null)
        {
            DoNotDestroyOnLoad(gameObject);
            gameControl = this;
        }
        else if (gameControl != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Load();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoNotDestroyOnLoad(GameObject gameObject)
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream file = File.Create(Application.persistentDataPath + "/gameInfo.dat"))
        {
            GameData data = new GameData();
            data.projectiles = projectiles;
            data.projectileIndex = projectileIndex;

            data.levelOneRespawnX = levelOneRespawnX;
            data.levelOneRespawnY = levelOneRespawnY;
            data.levelTwoRespawnX = levelTwoRespawnX;
            data.levelTwoRespawnY = levelTwoRespawnY;
            data.levelThreeRespawnX = levelThreeRespawnX;
            data.levelThreeRespawnY = levelThreeRespawnY;

            data.levelOneTimer = levelOneTimer;
            data.levelTwoTimer = levelTwoTimer;
            data.levelThreeTimer = levelThreeTimer;

            data.levelOneBest = levelOneBest;
            data.levelTwoBest = levelTwoBest;
            data.levelThreeBest = levelThreeBest;

            data.timePlayed = timePlayed;
            data.totalKills = totalKills;
            data.totalDeaths = totalDeaths;

            bf.Serialize(file, data);
            file.Close();
        }
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/gameInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameInfo.dat", FileMode.Open);

            GameData data = (GameData)bf.Deserialize(file);
            file.Close();

            projectiles = data.projectiles;
            projectileIndex = data.projectileIndex;

            levelOneRespawnX = data.levelOneRespawnX;
            levelOneRespawnY = data.levelOneRespawnY;
            levelTwoRespawnX = data.levelTwoRespawnX;
            levelTwoRespawnY = data.levelTwoRespawnY;
            levelThreeRespawnX = data.levelThreeRespawnX;
            levelThreeRespawnY = data.levelThreeRespawnY;
            
            levelOneTimer = data.levelOneTimer;
            levelTwoTimer = data.levelTwoTimer;
            levelThreeTimer = data.levelThreeTimer;
            
            levelOneBest = data.levelOneBest;
            levelTwoBest = data.levelTwoBest;
            levelThreeBest = data.levelThreeBest;
            
            timePlayed = data.timePlayed;
            totalKills = data.totalKills;
            totalDeaths = data.totalDeaths;
        }
    }
}

[Serializable]
class GameData
{
    public List<int> projectiles;
    public int projectileIndex;

    public float levelOneRespawnX;
    public float levelOneRespawnY;
    public float levelTwoRespawnX;
    public float levelTwoRespawnY;
    public float levelThreeRespawnX;
    public float levelThreeRespawnY;

    public float levelOneTimer;
    public float levelTwoTimer;
    public float levelThreeTimer;

    public float levelOneBest;
    public float levelTwoBest;
    public float levelThreeBest;

    public float timePlayed;
    public float totalKills;
    public float totalDeaths;
}
