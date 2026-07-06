using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace BasicCalculator.Client.Pages;

public abstract partial class Calculator : ComponentBase
{
    private void HandleKeyPressed(string key)
    {
        _pressedKey = key;

        if (_numbers.Contains(key))
            _displayValue = _displayValue == "0"
                ? key
                : $"{_displayValue}{key}";

        if (key == "." && !_displayValue.Contains("."))
            _displayValue += key;

        if (key == "del")
            _displayValue = _displayValue.Length == 1 ? "0" : _displayValue[..^1];

        if (_operators.Contains(key) && string.IsNullOrEmpty(_operation))
        {
            _operation = key;
            _previousValue = _displayValue;
            _displayValue = "0";
        }

        if (key == "=")
        {
            if (!string.IsNullOrEmpty(_operation))
            {
                decimal num1 = decimal.Parse(_previousValue);
                decimal num2 = decimal.Parse(_displayValue);
                decimal result = 0;

                switch (_operation)
                {
                    case "+": result = num1 + num2; break;
                    case "-": result = num1 - num2; break;
                    case "x": result = num1 * num2; break;
                    case "/":
                        if (num2 != 0) result = num1 / num2;
                        else { _displayValue = "Error"; }
                        break;
                }

                _displayValue = result.ToString(CultureInfo.InvariantCulture);
                _previousValue = "";
                _operation = "";
            }
        }

        if (key == "reset")
        {
            _displayValue = "0";
            _previousValue = "";
            _operation = "";
        }
    }
}