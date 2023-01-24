using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CustomFunctions
{
    /*******************************************************************/

    #region Custom Functions
    public static IEnumerator LerpText(string a, string b, float t, Action<string> callback)
    {
        while (a != b)
        {
            string calculatedString;

            int length = a.Length + (a.Length < b.Length ? 1 : 0);
            if (b[length - 1] == '<')
            {
                int index = b.IndexOf('>', length);
                length = index + ((index == b.Length - 1) ? 1 : 2);
            }

            calculatedString = b.Substring(0, length);

            a = calculatedString;

            callback(calculatedString);
            yield return new WaitForSeconds(t);
        }
    }
    #endregion
}
