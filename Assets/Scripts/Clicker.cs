using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Clicker : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _cashMultiplierText;
    [SerializeField] private TextMeshProUGUI _cashAmountText;
    [SerializeField] private TextMeshProUGUI _priceMultiplierText;
    [SerializeField] private TextMeshProUGUI _multiplierText;
    [SerializeField] private Slider _timeToClickSlider;
    [SerializeField] private Slider _cashToUpdateSlider;
    [SerializeField] private Button _clickButton;
    [SerializeField] private Button _upgrateButton;

    [SerializeField] private int _baseCashAmount;
    private int _cashAmount;
    private int _cashMultiplier = 1;
    [SerializeField]private int _priceMultiplierUpgrate;

    [SerializeField] private float _clickInterval;
    private float _clickTimer;

    private bool _isReadyToClick = true;
    private bool _isReadyToUpgrate;

    private GameController _gController;

    private void Awake()
    {
        _gController = GameController.instance;
    }

    public void Initialize(int baseCash, int priceUpgrate, float clickInterval)
    {
        _baseCashAmount = baseCash;
        _priceMultiplierUpgrate = priceUpgrate;
        _clickInterval = clickInterval;
    }

    private void Start()
    {
        _cashAmount = _baseCashAmount;
        _cashAmountText.text = "$" + _cashAmount.ToString();

        _timeToClickSlider.maxValue = _clickInterval;
        _timeToClickSlider.value = _clickTimer;

        UpdateMultiplierUI();

        TimeCounter.OnFrameUpdateIvent += UpdateTimeToClick;
        _gController.OnScoreChangeEvent += UpdateCashToUpgrate;

    }

    public void GetCash()
    {
        _gController.AddCash(_cashAmount);
        _clickButton.interactable = false;
        _timeToClickSlider.gameObject.SetActive(true);
        _isReadyToClick = false;
        _clickTimer = 0;
    }

    public void UpgrateCashAmount()
    {
        if (_gController.GetCash >= _priceMultiplierUpgrate)
        {
            _cashMultiplier++;
            _cashAmount = _baseCashAmount * _cashMultiplier;
            _gController.SubstractCash(_priceMultiplierUpgrate);
            _priceMultiplierUpgrate *= 3;

            UpdateMultiplierUI();

            if (_gController.GetCash < _priceMultiplierUpgrate)
            {
                _upgrateButton.interactable = false;
                _isReadyToUpgrate = false;
                UpdateCashToUpgrate();
            }
        }
    }

    private void UpdateMultiplierUI()
    {
        _priceMultiplierText.text = "$" + _priceMultiplierUpgrate.ToString();
        _multiplierText.text = "x" + _cashMultiplier.ToString();
        _cashToUpdateSlider.maxValue = _priceMultiplierUpgrate;
    }

    public void UpdateCashToUpgrate()
    {
        if (!_isReadyToUpgrate)
        {
            if (_gController.GetCash >= _priceMultiplierUpgrate)
            {
                _isReadyToUpgrate = true;
                _upgrateButton.interactable = true;
            }
            _cashToUpdateSlider.value = _gController.GetCash;
        }
    }

    public void UpdateTimeToClick(float deltaTime)
    {
        if (!_isReadyToClick)
        {
            _clickTimer += deltaTime;
            if (_clickTimer < _clickInterval)
            {
                _timeToClickSlider.value = _clickTimer;
            }
            else
            {
                _timeToClickSlider.gameObject.SetActive(false);
                _isReadyToClick = true;
                _clickButton.interactable = true;
            }
        }
    }



}
