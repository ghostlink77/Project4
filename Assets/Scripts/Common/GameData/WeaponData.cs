using System.IO;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public struct WeaponDataStruct
{
    public string WeaponType;
    public float WeaponDamage;
    public float CritRate;
    public float CritMultiplier;
    public float EffectRate;
    public float ProjAmount;
    public float ProjSpeed;
    public float AtkSpeed;
    public float AtkRange;
}




public class WeaponData : IGameData
{
    private readonly string FILENAME = "WeaponData";
    private Dictionary<string, WeaponDataStruct> _weaponData = new Dictionary<string, WeaponDataStruct>();
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
            
            WeaponDataStruct weaponData = new WeaponDataStruct();
            var splitData = data.Split(',');
            weaponData.WeaponType = splitData[1];
            if (float.TryParse(splitData[2], out float result2)) weaponData.WeaponDamage = result2;
            else break;
            if (float.TryParse(splitData[3], out float result3)) weaponData.CritRate = result3;
            else break;
            if (float.TryParse(splitData[4], out float result4)) weaponData.CritMultiplier = result4;
            else break;
            if (float.TryParse(splitData[5], out float result5)) weaponData.EffectRate = result5;
            else break;
            if (float.TryParse(splitData[6], out float result6)) weaponData.ProjAmount = result6;
            else break;
            if (float.TryParse(splitData[7], out float result7)) weaponData.ProjSpeed = result7;
            else break;
            if (float.TryParse(splitData[8], out float result8)) weaponData.AtkSpeed = result8;
            else break;
            if (float.TryParse(splitData[9], out float result9)) weaponData.AtkRange = result9;
            else break;


        }
    }
    public WeaponDataStruct GetExpData(string weaponName)
    {
        return _weaponData[weaponName];
    }
}
