using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CashBooster : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _boosterPriceText;
    [SerializeField] private TextMeshProUGUI _boosterMultiplierText;
    [SerializeField] private Button _enableBoosterButton;
    [SerializeField] private Slider _cashToBoosterSlider;
    [SerializeField] private Slider _reloadBoosterSlider;

    [SerializeField] private int _boosterPrice;
    [SerializeField] private int _boosterMultiplier = 2;

    [SerializeField] private float _boosterWorkTime = 5;
    [SerializeField] private float _boosterReloadTime = 30;

    private bool _canBuyBoost;
    private bool _isReloading;
    private bool _isBoosterReady = true;
    private float _boosterTimer;

    private GameController _gController;

    private void Start()
    {
        _gController = GameController.instance;
        _gController.OnScoreChangeEvent += UpdateCashToBoost;
        UpdateUI();
    }

    private void UpdateUI()
    {
        _boosterPriceText.text = "$" + ConvertNumber(_boosterPrice);
        _boosterMultiplierText.text = "X" + ConvertNumber(_boosterMultiplier);
        _cashToBoosterSlider.maxValue = _boosterPrice;
    }

    private void UpdateCashToBoost()
    {
        if (!_gController.IsCashEnough(_boosterPrice))
        {
            _cashToBoosterSlider.value = _gController.GetCash;
        }
        if (_canBuyBoost != _gController.IsCashEnough(_boosterPrice))
        {
            if (_isBoosterReady)
                _enableBoosterButton.interactable = !_canBuyBoost;
            _cashToBoosterSlider.value = _gController.GetCash;
            _canBuyBoost = !_canBuyBoost;
        }
    }

    public void SwitchOnBoost()
    {
        if (_gController.IsCashEnough(_boosterPrice))
        {
            _gController.SubstractCash(_boosterPrice);
            _gController.AddMultiplier(_boosterMultiplier);

            TimeCounter.OnFrameUpdateIvent += BoosterCounter;

            _enableBoosterButton.interactable = false;
            _reloadBoosterSlider.maxValue = _boosterWorkTime;
            _reloadBoosterSlider.gameObject.SetActive(true);
            
            _isBoosterReady = false;
        }
    }

    private void BoosterCounter(float deltaTime)
    {
        if (_isReloading)
        {
            _boosterTimer -= deltaTime;
            _reloadBoosterSlider.value = _boosterTimer;
            if (_boosterTimer <= 0)
            {
                TimeCounter.OnFrameUpdateIvent -= BoosterCounter;
                _isBoosterReady = true;
                _boosterTimer = 0;
                _isReloading = false;
                if (_canBuyBoost)
                    _enableBoosterButton.interactable = _canBuyBoost;
            }
        }
        else
        {
            _boosterTimer += deltaTime;
            if (_boosterTimer >= _boosterWorkTime)
            {
                _isReloading = true;
                _gController.RemoveMultiplier(_boosterMultiplier);
                _boosterTimer = _boosterReloadTime;
                _reloadBoosterSlider.value = _boosterTimer;
                _reloadBoosterSlider.maxValue = _boosterReloadTime;
            }
            _reloadBoosterSlider.value = _boosterTimer;
        }
    }

    private string ConvertNumber(int number)
    {
        return NumbersConverter.ConvertNumber(number);
    }
}
