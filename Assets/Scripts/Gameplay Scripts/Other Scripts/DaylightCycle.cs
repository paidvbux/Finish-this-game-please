using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaylightCycle : MonoBehaviour
{
    #region General Variables/Settings
    [Header("General Settings")]
    public Light sun;
    public AnimationCurve brightnessCurve;
    #endregion

    #region Time Variables/Settings
    [Header("Time Settings")]
    public bool isDay;
    public int ticksPerDay = 1440;
    public int day;
    public int ticks;
    #endregion

    #region Hidden Variables
    float ticks_float;
    float sunRotationSpeed;
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Start()
    {
        ticks_float = ticksPerDay * PercentageOfDay(7, 30);
        sun.transform.eulerAngles = new Vector3(0, sun.transform.eulerAngles.y, sun.transform.eulerAngles.z);
        sun.transform.Rotate(0, -CalculateRotationFromTime(7, 30), 0, Space.Self);
        sunRotationSpeed = 360 / ticksPerDay;
    }

    void Update()
    {
        UpdateTicks();
        UpdateBrightness();
        UpdateRotation();
    }
    #endregion

    #region Custom Functions
    float PercentageOfDay(float hours, float minutes)
    {
        return (hours * 60 + minutes) / (float) 1440;
    }

    float CalculateRotationFromTime(float hours, float minutes)
    {
        return PercentageOfDay(hours, minutes) * 360 - 90;
    }
    
    void UpdateDay()
    {
        if (ticks_float > ticksPerDay)
        {
            foreach (QuestBoardScript questBoard in QuestLoader.singleton.questBoards) {   
                if (questBoard.acceptedQuest)
                    QuestLoader.acceptedQuests.Remove(questBoard.selectedQuest);
            }

            QuestLoader.singleton.GenerateDailyQuests();
            ticks_float = 0;
            day++;
        }
    }

    void UpdateTicks()
    {
        ticks_float += Time.deltaTime;
        ticks = Mathf.FloorToInt(ticks_float);

        isDay = ticks > 270 || ticks < 1170;

        UpdateDay();
    }

    void UpdateBrightness()
    {
        sun.intensity = brightnessCurve.Evaluate(ticks_float / (float) ticksPerDay);
    }

    void UpdateRotation()
    {
        sun.transform.Rotate(0, -sunRotationSpeed * Time.deltaTime, 0, Space.Self);
    }
    #endregion
}
