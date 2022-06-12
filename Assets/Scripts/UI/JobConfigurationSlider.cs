using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobConfigurationSlider : MonoBehaviour
{
    [SerializeField] string _name;
    [SerializeField] float _baseValue;

    [Header("Components")]
    [SerializeField] Text _nameText;
    [SerializeField] Text _valueText;
    [SerializeField] Slider _valueSlider;

    void Start()
    {
        _valueSlider.value = _baseValue;
        _nameText.text = _name;
    }

    public int GetValue()
    {
        return Mathf.RoundToInt(_valueSlider.value);
    }

    private void Update()
    {
        _valueText.text = GetValue().ToString();
    }
}
