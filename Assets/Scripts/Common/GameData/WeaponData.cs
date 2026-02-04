using System.IO;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public struct WeaponDataStruct
{
    public string WeaponName;
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
    private List<WeaponDataStruct> _weaponData = new List<WeaponDataStruct>();
    public void SetData()
    {
        string path = Application.dataPath + "/Resources/DataTable/" + FILENAME + ".csv";
        Debug.Log(path);
        StreamReader reader = new StreamReader(path);
        if (reader == null)
        {
            Debug.Log("WeaponData.csv doesn't exist.");
        }

        reader.ReadLine();

        bool isFinish = false;
        while (isFinish == false)
        {
            string data = reader.ReadLine();
            if (data == null)
            {
                isFinish = true;
                Debug.Log("No WeaponData.");
                break;
            }
            
            WeaponDataStruct weaponData = new WeaponDataStruct();
            var splitData = data.Split(',');
            weaponData.WeaponName = splitData[0];
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

            _weaponData.Add(weaponData);
            Debug.Log($"weaponData Added -> {data}");

        }
    }
    public WeaponDataStruct GetWeaponData(string weaponName)
    {
        foreach(var weaponData in _weaponData)
        {
            if (weaponData.WeaponName == weaponName)
                return weaponData;
        }
        Debug.Log($"weapon : {weaponName} doesn't exist. Return the initial weapon.");
        return _weaponData[0];
    }

    public WeaponDataStruct GetRandomWeaponData()
    {
        /*int totalWeight = 0;
        foreach(var weaponData in _weaponData)
        {
            totalWeight += weaponData.weight;
        }

        int pivot = Random.Range(0, totalWeight);

        int currentWeight = 0;
        foreach(var weaponData in _weaponData)
        {
            currentWeight += weaponData_weight;
            if (pivot < currentWeight) return weaponData;
        }
        return _weaponData[0];*/

        int index = Random.Range(0, _weaponData.Count-1);
        return _weaponData[index];
    }
}
