using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    [SerializeField] private int _cash;
    [SerializeField] private TextMeshProUGUI _cashText;
    [SerializeField] private TextMeshProUGUI _multiplierText;

    private int _multiplier = 1;

    public int GetCash => _cash;

    public delegate void OnScoreChange();
    public OnScoreChange OnScoreChangeEvent;

    public void Start()
    {
        _cashText.text = "$" + _cash.ToString();
        _multiplierText.text = "x" + _multiplier.ToString();
    }

    public void AddCash(int amount)
    {
        _cash += amount;
        _cashText.text = "$" + _cash.ToString();
        if (OnScoreChangeEvent != null)
            OnScoreChangeEvent.Invoke();
    }

    public void SubstractCash(int amount)
    {
        AddCash(-amount);
    }

    public void AddMultiplier(float amount)
    {
        _multiplier = (int)(_multiplier / amount);
        if (_multiplier < 1)
            _multiplier = 1;
    }

    public void RemoveMultiplier(int amount)
    {
        AddMultiplier(1 / amount);
    }

    public bool IsCashEnough(int price)
    {
        return _cash >= price;
    }

}
