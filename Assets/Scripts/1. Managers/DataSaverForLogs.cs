using UnityEngine;

public class DataSaverForLogs : MonoBehaviour
{
    //Timers ans Times 
    public float totalTime = -1f;
    public float firstKeyTime = -1f;
    public float jeraTime = -1f;
    public float othillaTime = -1f;
    //Objects and Used Objects
    public int takedPotions;
    public int usedPotions;

    //************************************************************************
    // Times and Timers here.
    //************************************************************************
    // Total time of the game
    public void SetTotalTime(float time)
    {
        totalTime = time;
    }

    public float GetTotalTime()
    {
        return totalTime;
    }


    //Time for the first key.
    public void SetFirstKeyTime(float firstKeyTime)
    {
        if (this.firstKeyTime < 0f)
        {
            this.firstKeyTime = firstKeyTime;
            Debug.Log(this.firstKeyTime);
        }
    }

    public float GetFirstKeyTime()
    {
        return firstKeyTime;
    }

    // Time for the runes
    // Jera
    public void SetJeraTime(float time)
    {
        if (jeraTime <= 0f)
        {
            jeraTime = time;
        }
        Debug.Log("Tiempo en coger Jera: " + jeraTime);
    }

    public float GetJeraTime()
    {
        return jeraTime;
    }

    //Othilla
    public void SetOthillaTime(float time)
    {
        if (othillaTime <= 0f)
        {
            othillaTime = time;
        }
        Debug.Log("Tiempo en coger Othilla: " + othillaTime);
    }

    public float GetOthillaTime()
    {
        return othillaTime;
    }


    //************************************************************************
    // Objects and Used Objects here.
    //************************************************************************

    // Taked Potions.
    public void SetTakedPotions()
    {
        takedPotions++;
        Debug.Log("Taked potions: " + this.takedPotions);
    }
    public int GetTakedPotions()
    {
        return takedPotions;
    }

    //Used Potions.
    public void SetUsedPotions()
    {
        usedPotions++;
        Debug.Log("Used Potions" + this.usedPotions);
    }

    public int GetUsedPotions()
    {
        return usedPotions;
    }

}
