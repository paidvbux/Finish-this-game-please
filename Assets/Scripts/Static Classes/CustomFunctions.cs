using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomFunctions
{
    public static IEnumerator LerpText(string a, string b, float t, Action<string> callback)
    {
        while (a != b)
        {
            string calculatedString;
            int length = a.Length + (a.Length < b.Length ? 1 : 0);

            calculatedString = b.Substring(0, length);

            a = calculatedString;

            callback(calculatedString);
            yield return new WaitForSeconds(t);
        }
    }


}
