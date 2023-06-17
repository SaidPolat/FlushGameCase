using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    [SerializeField] private float playerMoney;

    public float PlayerMoney { get => playerMoney; set => playerMoney = value; }    

    public Tile tilePrefab;     //griddeki tile lar�n prefab�

    public GemEditor GemList;   //resources i�indeki gem listesinin tutuldu�u scriptable obje referans�
    
    void Start()
    {
        Application.targetFrameRate = 60;

        //normalde save sistemini json nodelar� ile yap�yorum, fakat sadece 2 de�er i�in performans kayb� olur diye d�z playerprefs kulland�m

        GemList.GemModels.ForEach(g =>
        {
            g.collectedGemCount = PlayerPrefs.GetInt(g.gemName, 0);
        });

        PlayerMoney = PlayerPrefs.GetFloat("playerMoney", 0);
        
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus) Save();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) Save();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("playerMoney", PlayerMoney);

        GemList.GemModels.ForEach(g =>
        {
            PlayerPrefs.SetInt(g.gemName, g.collectedGemCount);
        });
    }
}

public partial class GameManager
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance) return _instance;
            var objs = FindObjectsOfType(typeof(GameManager)) as GameManager[];
            if (objs?.Length > 0) _instance = objs[0];
            if (_instance) return _instance;
            var obj = new GameObject { hideFlags = HideFlags.HideAndDontSave };
            _instance = obj.AddComponent<GameManager>();
            return _instance;
        }
    }
}