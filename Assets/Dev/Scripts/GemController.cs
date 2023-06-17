using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemController : MonoBehaviour
{
    private Colors.Color _gemColor;
    private string _gemName;
    private float _gemStartPrice;

    private float _scaleBeforeCollect;
    private Tile _currentTile;

    public Colors.Color GemColor { get { return _gemColor; } set {  _gemColor = value; } }
    public string GemName { get { return _gemName; } set { _gemName = value; } }
    public float GemStartPrice { get { return _gemStartPrice; } set {  _gemStartPrice = value; } }
    public float ScaleBeforeCollect { get { return _scaleBeforeCollect; } set {  _scaleBeforeCollect = value; } }
    public Tile CurrentTile { get { return _currentTile; } set {  _currentTile = value; } }

    public void SpawnNewGemForTile()
    {
        CurrentTile.SpawnNewGem();
    }

    public void EarnMoneyFromSale()
    {
        GameManager.Instance.PlayerMoney += GemStartPrice + (ScaleBeforeCollect * 100);

        UIManager.Instance.UpdateCashText(GameManager.Instance.PlayerMoney);
    }
}
