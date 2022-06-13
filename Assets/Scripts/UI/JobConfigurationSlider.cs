using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobConfigurationSlider : MonoBehaviour
{
    [SerializeField] string _name;
    [SerializeField] float _baseValue;
    [SerializeField] int _priority;
    [SerializeField] int _framePeriodOptions;

    [Header("Components")]
    [SerializeField] Text _nameText;
    [SerializeField] Text _valueText;
    [SerializeField] Text _priorityText;
    [SerializeField] Slider _valueSlider;
    [SerializeField] Dropdown _framePeriodDropdown;

    void Start()
    {
        _valueSlider.value = _baseValue;
        _nameText.text = _name;
        _priorityText.text = "Priority " + _priority.ToString();

        _framePeriodDropdown.ClearOptions();
        for(int i=0; i<_framePeriodOptions; i++)
        {
            _framePeriodDropdown.AddOptions(new List<Dropdown.OptionData> {
                new Dropdown.OptionData { text = "Run every " + (i+1).ToString() + " frames" }
            });
        }
    }

    public int GetDurationValue()
    {
        return Mathf.RoundToInt(_valueSlider.value);
    }

    public int GetPeriodValue()
    {
        return _framePeriodDropdown.value + 1;
    }

    private void Update()
    {
        _valueText.text = GetDurationValue().ToString();
    }
}
