using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    [SerializeField] private float playerMoney;

    public float PlayerMoney { get => playerMoney; set => playerMoney = value; }    

    public Tile tilePrefab;     //griddeki tile larýn prefabý

    public GemEditor GemList;   //resources içindeki gem listesinin tutulduðu scriptable obje referansý
    
    void Start()
    {
        Application.targetFrameRate = 60;

        //normalde save sistemini json nodelarý ile yapýyorum, fakat sadece 2 deðer için performans kaybý olur diye düz playerprefs kullandým

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