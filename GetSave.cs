using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;

public class GetSave : MonoBehaviour {
	
	private GameObject GoldController;
    private GameObject Monster;
    private GameObject Upgrade;
    private string _savePath; //путь с сохранением
    private SavingInfo _infoFromSavedFile; // класс с переменными для сохраняемой/получаемой инфы

	//
	private GameObject Upgrade1;
	//

	void Start () {
        try
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        _savePath = Path.Combine(Application.persistentDataPath, "arpgsave.json"); // внешняя память -> Android -> Data -> название бандла проекта -> files
#else
            _savePath = Path.Combine(Application.dataPath, "arpgsave.json");
#endif
            if (File.Exists(_savePath))
            {
				GoldController = GameObject.Find("GoldController");
                Monster = GameObject.Find("Chan1");
                Upgrade = GameObject.Find("DamageController");

                _infoFromSavedFile = JsonUtility.FromJson<SavingInfo>(File.ReadAllText(_savePath)); //пишем инфу из файла в _infoFromSavedFile

                GoldController.GetComponent<GoldController>().Set_Player_Gold(_infoFromSavedFile.Gold); // присваиваем playerGold значение экземпляра класса _infoFromSavedFile
                //GoldController.GetComponent<GoldController>().update_PlayerGold();

                Monster.GetComponent<Monster>().CurrentHP = _infoFromSavedFile.CurrentHP;
                Monster.GetComponent<Monster>().MaxHP = _infoFromSavedFile.MaxHP;
                Monster.GetComponent<HPController>().Update_HP_Bar_and_Text();
				Monster.GetComponent<SpriteRenderer>().sprite = _infoFromSavedFile.Sprite;
				Upgrade.GetComponent<DamageController>()._totalBoost = _infoFromSavedFile.boost;

					Upgrade1 = GameObject.Find("Boost_0");
					Upgrade1.GetComponent<OnClick_Buy_Upgrade>().Set_BoostCount(Upgrade.GetComponent<DamageController>()._totalBoost[0]);
					Debug.Log("Boost_0" + ": Значение= " + Upgrade.GetComponent<DamageController>()._totalBoost[0] + "  загружено из файла");
				Upgrade.GetComponent<DamageController>().Refresh_ClickDamage();

					Upgrade1 = GameObject.Find("Boost_1");
					Upgrade1.GetComponent<OnClick_Buy_Upgrade>().Set_BoostCount(Upgrade.GetComponent<DamageController>()._totalBoost[1]);
					Debug.Log("Boost_1" + ": Значение= " + Upgrade.GetComponent<DamageController>()._totalBoost[1] + "  загружено из файла");
				Upgrade.GetComponent<DamageController>().Refresh_AutoDamage();
			}
        }
        catch(Exception ex)
        {
            Debug.Log("ERROR " + ex);
        }
	}
}