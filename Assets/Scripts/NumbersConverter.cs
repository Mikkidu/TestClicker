using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumbersConverter
{
    public static string ConvertNumber(int number)
    {
        string symbol = "";
        if (number >= 1000000000)
        {
            symbol = "B";
            number /= 1000000000;
        }
        else if (number >= 10000000)
        {
            symbol = "M";
            number /= 1000000;
        }else if (number >= 10000)
        {
            symbol = "K";
            number /= 1000;
        }

        return number.ToString() + symbol;
    }
}
