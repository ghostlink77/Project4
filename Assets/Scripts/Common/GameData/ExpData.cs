using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExpData : IGameData
{
    private Dictionary<float, float> experienceData = new Dictionary<float, float>();
    private readonly string FILENAME = "ExpData";

    public void SetData()
    {
        string path = Application.dataPath + "/Resources/DataTable/" + FILENAME + ".csv";

        StreamReader reader = new StreamReader(path);
        if (reader == null)
        {
            Debug.Log("ExpData.csv doesn't exist.");
        }

        reader.ReadLine();

        bool isFinish = false;
        while (isFinish == false)
        {
            string data = reader.ReadLine();
            if (data == null)
            {
                isFinish = true;
                break;
            }

            var splitData = data.Split(',');
            if (float.TryParse(splitData[0], out float result1) && float.TryParse(splitData[1], out float result2))
            {

                experienceData.Add(result1, result2);
            }
            else
            {
                Debug.Log("LevelUpdata Error.");
            }

        }
    }
    public float GetExpData(int currentLevel)
    {
        return experienceData[currentLevel];
    }
}
