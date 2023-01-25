using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaylightCycle : MonoBehaviour
{
    public bool isDay;

    public int day;
    public int ticks;

    public int ticksPerDay = 1440;

    float ticks_float;

    void Start()
    {
        ticks_float = 270;
    }

    void Update()
    {
        ticks_float += Time.deltaTime;
        ticks = Mathf.FloorToInt(ticks_float);

        isDay = ticks > 270 || ticks < 1170;
    }
}
