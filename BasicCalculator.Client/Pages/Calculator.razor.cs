using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace BasicCalculator.Client.Pages;

public partial class Calculator : ComponentBase
{
    private string _theme = "option-1";
    private readonly string[] _numbers = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
    private readonly string[] _operators = { "+", "-", "/", "x" };

    private string _pressedKey = "";
    private string _displayValue = "0";
    private string _previousValue = "";
    private string _operation = "";

    private void SetTheme(string theme)
    {
        _theme = theme;
    }

    private void HandleKeyPressed(string key)
    {
        _pressedKey = key;

        if (_numbers.Contains(key)) AppendNumber(key);
        else if (key == ".") AppendDecimal();
        else if (_operators.Contains(key)) SetOperation(key);
        else if (key == "del") DeleteLastDigit();
        else if (key == "=") Calculate();
        else if (key == "reset") ResetCalculator();
    }

    private void AppendNumber(string key)
    {
        _displayValue = _displayValue is "0" or "Error"
            ? key
            : $"{_displayValue}{key}";
    }

    private void AppendDecimal()
    {
        if (_displayValue == "Error")
            _displayValue = "0";

        if (!_displayValue.Contains('.'))
            _displayValue += '.';
    }

    private void SetOperation(string key)
    {
        if (string.IsNullOrEmpty(_operation))
        {
            _operation = key;
            _previousValue = _displayValue;
            _displayValue = "0";
        }
    }

    private void DeleteLastDigit()
    {
        if (_displayValue == "Error")
        {
            _displayValue = "0";
            return;
        }

        _displayValue = _displayValue.Length == 1 ? "0" : _displayValue[..^1];
    }

    private void ResetCalculator()
    {
        _displayValue = "0";
        _previousValue = "";
        _operation = "";
    }

    private void Calculate()
    {
        if (string.IsNullOrEmpty(_operation))
            return;

        if (!decimal.TryParse(_previousValue, CultureInfo.InvariantCulture, out var num1) ||
            !decimal.TryParse(_displayValue, CultureInfo.InvariantCulture, out var num2))
        {
            _displayValue = "Error";
            _previousValue = "";
            _operation = "";
            return;
        }

        decimal result;

        switch (_operation)
        {
            case "+":
                result = num1 + num2;
                break;
            case "-":
                result = num1 - num2;
                break;
            case "x":
                result = num1 * num2;
                break;
            case "/":
                if (num2 == 0)
                {
                    _displayValue = "Error";
                    _previousValue = "";
                    _operation = "";
                    return;
                }

                result = num1 / num2;
                break;
            default:
                return;
        }

        _displayValue = result.ToString(CultureInfo.InvariantCulture);
        _previousValue = "";
        _operation = "";
    }
}
