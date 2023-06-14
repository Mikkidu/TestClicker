using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelCreator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _panelPriceText;
    [SerializeField] private Button _createButton;
    [SerializeField] private Slider _cashToCreateSlider;

    [SerializeField] private Clicker _clickerPrefab;
    [SerializeField] private Transform _panelsParentTransform;

    [SerializeField] private int _panelPrice;
    [SerializeField] private int _panelCashPerSecond;
    [SerializeField] private int _cashRateIncrease = 5;
    [SerializeField] private int _priceRateIncrease = 10;

    [SerializeField] private float _minClickInterval = 0.5f;
    [SerializeField] private float _maxClickInterval = 5;

    private bool _isReadyToCreate;

    private GameController _gController;

    private void Start()
    {
        _gController = GameController.instance;
        _gController.OnScoreChangeEvent += UpdateCashToUpgrate;
        UpdateUI();
    }
    public void RefreshSelf()
    {
        _panelPrice *= _priceRateIncrease;
        _panelCashPerSecond *= _cashRateIncrease;
        
        _maxClickInterval++;
        transform.SetParent(_panelsParentTransform.parent);
        transform.SetParent(_panelsParentTransform);
        UpdateUI();
    }

    public void CreatePanel()
    {
        if(_gController.GetCash >= _panelPrice)
        {
            Clicker clickPanel = Instantiate(_clickerPrefab, _panelsParentTransform);
            float interval = Random.Range(_minClickInterval, _maxClickInterval);
            int cashAmount = (int)(_panelCashPerSecond * interval);
            int freezePrice = _panelPrice;
            clickPanel.Initialize(cashAmount, cashAmount * 5, interval);

            RefreshSelf();
            _gController.SubstractCash(freezePrice);
        }
    }

    private void UpdateUI()
    {
        _panelPriceText.text = "$" + ConvertNumber(_panelPrice);
        _cashToCreateSlider.maxValue = _panelPrice;
    }

    private void UpdateCashToUpgrate()
    {
        if (!_gController.IsCashEnough(_panelPrice))
        {
            _cashToCreateSlider.value = _gController.GetCash;
        }
        if (_isReadyToCreate != _gController.IsCashEnough(_panelPrice))
        {
            SwitchButton(!_isReadyToCreate);
            _cashToCreateSlider.value = _gController.GetCash;
        }
    }

    private void SwitchButton(bool isReady)
    {
        _createButton.interactable = isReady;
        _cashToCreateSlider.gameObject.SetActive(!isReady);
        _isReadyToCreate = isReady;
    }

    private string ConvertNumber(int number)
    {
        return NumbersConverter.ConvertNumber(number);
    }
}
