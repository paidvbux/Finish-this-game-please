using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotScript : MonoBehaviour
{
    #region Crop Variables/Settings
    [Header("Crop Settings")]
    public Crop plantedCrop;
    Crop previousCrop;
    CropScript summonedCrop;
    #endregion

    #region Hidden/Private Variables
    float timer;
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Update()
    {
        #region Crop Change
        //  Checks if there is a crop planted on the plot.
        if (plantedCrop != null)
        {
            //  Checks whether the crop has changed values
            if (plantedCrop != previousCrop)
            {
                //  Destroy the summonedCrop to properly reset the plot.
                Destroy(summonedCrop);
            
                //  Run a function to summon the crop.
                SummonCrop();

                //  Reset the timer
                timer = plantedCrop.cropGrowTime;

                //  Set the previousCrop to the current one.
                previousCrop = plantedCrop;
            }

            //  Run a function to increase the scale of the crop.
            GrowCrops();
        }
        #endregion
    }
    #endregion

    #region Custom Functions
    void GrowCrops()
    {
        timer -= timer >= 0 ? Time.deltaTime : 0;

        #region Update Scale & Hover
        if (timer <= 0) 
            summonedCrop.Hover();
        else
        {
            float lerpAmount = (plantedCrop.cropGrowTime - timer) / plantedCrop.cropGrowTime;
            summonedCrop.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, lerpAmount);
        }
        #endregion
    }

    void SummonCrop()
    {
        GameObject cropObject = Instantiate(plantedCrop.gameObject, transform.position, Quaternion.identity);

        #region Initialization
        cropObject.transform.localScale = Vector3.zero;
        cropObject.transform.SetParent(transform);

        summonedCrop = cropObject.GetComponent<CropScript>();
        GameManager.crops.Add(summonedCrop);

        summonedCrop.plotScript = this;
        #endregion
    }

    public void Harvest()
    {
        #region Instantiate Drops
        for (int i = 0; i < Random.Range(plantedCrop.minHarvestCount, plantedCrop.maxHarvestCount + 1); i++)
        {
            GameObject droppedCrop = Instantiate(plantedCrop.grabbableObject, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            
            droppedCrop.transform.SetParent(GameManager.singleton.transform);
        }
        #endregion

        #region Remove Crop
        GameManager.crops.Remove(summonedCrop);
        Destroy(summonedCrop.gameObject);

        summonedCrop = null;
        plantedCrop = null;
        #endregion
    }
    #endregion
}
