using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;

public class Save : MonoBehaviour {

	private GameObject GoldController;
    private GameObject Monster;
    private GameObject Upgrade;
    private SavingInfo _savingInfo = new SavingInfo();
    private string _savingPath;
    private string json;

#if UNITY_ANDROID && !UNITY_EDITOR // проверка сохраняем с андроида или нет
    private void OnApplicationPause(bool pause)
    {
        SaveMethod();
        if (pause) File.WriteAllText(_savingPath, json);
    }
#endif
    private void OnApplicationQuit()
    {
        SaveMethod();
        File.WriteAllText(_savingPath, json);
        Debug.Log("APP QUIT gold:" + _savingInfo.Gold + " Current HP:" + _savingInfo.CurrentHP 
            + " Max HP:" + _savingInfo.MaxHP);
        
    }

    private void SaveMethod()
    {
        try
        {
			// Здесь пытаемся присваивоить переменные с UI экземплярам класса
			//_savingInfo.Gold = Convert.ToInt32(playerGold.text);
			GoldController = GameObject.Find("GoldController");
            Monster = GameObject.Find("Chan1");
            Upgrade = GameObject.Find("DamageController");
            
            _savingInfo.Gold = GoldController.GetComponent<GoldController>().Get_Player_Gold();
            _savingInfo.CurrentHP = Monster.GetComponent<Monster>().CurrentHP;
            _savingInfo.MaxHP = Monster.GetComponent<Monster>().MaxHP;
            _savingInfo.boost = Upgrade.GetComponent<DamageController>()._totalBoost;
			_savingInfo.Sprite = Monster.GetComponent<SpriteRenderer>().sprite;
			json = JsonUtility.ToJson(_savingInfo); // преобразуем класс в json
        }
        catch
        {
            Debug.Log("FAILED TO WRITE INFO");
            return;
        }
#if UNITY_ANDROID && !UNITY_EDITOR
        _savingPath = Path.Combine(Application.persistentDataPath, "arpgsave.json"); // внешняя память -> Android -> Data -> название бандла проекта -> files
#else
		_savingPath = Path.Combine(Application.dataPath, "arpgsave.json");
#endif
    }
}

[Serializable]
public class SavingInfo // класс для хранения сохраняемой инфы
{
    public int Gold; // бесплатные монетки
    public double CurrentHP; // хп монстра на момент выхода с приложения, решил сделать одним массивом вместо двух переменных
    public double MaxHP;
    public int[] boost; // индекс - номер буста, значение по индексу - количество буста
	public Sprite Sprite;
}
