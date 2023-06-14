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
        _cashText.text = "$" + ConvertNumber(_cash);
        _multiplierText.text = "x" + ConvertNumber(_multiplier);
    }

    public void AddCash(int amount)
    {
        _cash += amount * _multiplier;
        _cashText.text = "$" + ConvertNumber(_cash);
        if (OnScoreChangeEvent != null)
            OnScoreChangeEvent.Invoke();
    }

    public void SubstractCash(int amount)
    {
        _cash -= amount;
        _cashText.text = "$" + ConvertNumber(_cash);
        if (OnScoreChangeEvent != null)
            OnScoreChangeEvent.Invoke();
    }

    public void AddMultiplier(float amount)
    {
        _multiplier = (int)(_multiplier * amount);
        if (_multiplier < 1)
            _multiplier = 1;
        _multiplierText.text = "X" + ConvertNumber(_multiplier);
    }

    public void RemoveMultiplier(int amount)
    {
        AddMultiplier(1f / amount);
    }

    public bool IsCashEnough(int price)
    {
        return _cash >= price;
    }

    private string ConvertNumber(int number)
    {
        return NumbersConverter.ConvertNumber(number);
    }

}
