using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    GameObject tilesGem;

    void Start()
    {
        InitializeGem();
    }

    public void InitializeGem()
    {
        //yeni random gem oluþtur

        Gem gem = new Gem();

        //listeden random bir gem çekip özelliklerini cloneluyoruz
        gem = (Gem)GameManager.Instance.GemList.GemModels[Random.Range(0, GameManager.Instance.GemList.GemModels.Count)].Clone();
        
        tilesGem = Instantiate(gem.gemPrefab, transform, false);
        tilesGem.transform.localScale = Vector3.zero;
        tilesGem.transform.localPosition = new Vector3(0, 1/0.39f, 0);

        //controller oluþturup listeden aldýðýmýz özellikler daha kolay eriþilmesi için ona aktarýlýr
        GemController tilesGemController = tilesGem.GetComponent<GemController>();

        tilesGemController.CurrentTile = this;
        tilesGemController.GemName = gem.gemName;
        tilesGemController.GemStartPrice = gem.gemStartPrice;
        tilesGemController.GemColor = gem.gemColor;

        GrowGem();
    }

    public void GrowGem()
    {
        if (tilesGem == null) return;

        tilesGem.transform.DOScale(1 / 0.39f, 5);
    }

    public void SpawnNewGem()
    {
        tilesGem = null;

        InitializeGem();
    }
}
