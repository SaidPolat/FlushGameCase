using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.UIElements;

public partial class UIManager : MonoBehaviour
{
    public TextMeshProUGUI cashText;

    [Header("CollectedGemUI")]
    [SerializeField]private GameObject allGemsButton;
    [SerializeField] private GameObject collectedGemPanel;
    [SerializeField] private GameObject collectibleUIPrefab;
    [SerializeField] private GameObject content;

    [HideInInspector] public List<CollectibleUIController> collectibleUIControllersList;

    void Start()
    {
        UpdateCashText(PlayerPrefs.GetFloat("playerMoney", 0));

        InitializeCollectedGemUI();
    }


    public void CollectedGemPanelStatus(bool status)
    {
        if (status)
        {
            allGemsButton.SetActive(false);
            collectedGemPanel.transform.DOScale(new Vector3(.92f, 1, 1), .5f).SetEase(Ease.OutBack);
        }
        else
        {
            collectedGemPanel.transform.DOScale(0, .4f).SetEase(Ease.InBack).OnComplete(() => allGemsButton.SetActive(true));
        }
    }

    public void InitializeCollectedGemUI()
    {
        GameManager.Instance.GemList.GemModels.ForEach(g =>
        {
            var collectibleUIGem = Instantiate(collectibleUIPrefab, content.transform);

            CollectibleUIController controller = collectibleUIGem.GetComponent<CollectibleUIController>();

            controller.icon.sprite = g.gemSprite;
            controller.gemNameText.text = "Gem Type: " + g.gemName;
            controller.gemCountText.text = "Collected Count: " + g.collectedGemCount.ToString();

            collectibleUIControllersList.Add(controller);
        });
    }

    public void UpdateCollectedGemUI(int type, int amount)
    {
        collectibleUIControllersList[type].gemCountText.text = "Collected Count: " + amount.ToString();   
    }

    public void UpdateCashText(float amount)
    {
        string textToWrite;

        if (amount > 1000000000) textToWrite = (amount / 10000).ToString("F1") + "B";
        else if (amount > 1000000) textToWrite = (amount / 10000).ToString("F1") + "M";
        else if (amount > 1000) textToWrite = (amount / 1000).ToString("F1") + "K";
        else textToWrite = amount.ToString("F1");

        cashText.text = "$" + textToWrite;
    }
}

public partial class UIManager
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance) return _instance;
            var objs = FindObjectsOfType(typeof(UIManager)) as UIManager[];
            if (objs?.Length > 0) _instance = objs[0];
            if (_instance) return _instance;
            var obj = new GameObject { hideFlags = HideFlags.HideAndDontSave };
            _instance = obj.AddComponent<UIManager>();
            return _instance;
        }
    }
}
