using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotScript : MonoBehaviour
{
    #region Crop Variables/Settings
    [Header("Crop Settings")]
    public Crop plantedCrop;
    #endregion

    #region Hidden/Private Variables
    Crop previousCrop;
    CropScript summonedCrop;
    float timer;
    #endregion

    /*******************************************************************/

    #region Unity Runtime Functions
    void Update()
    {
        #region Crop Change
        if (plantedCrop != null)
        {
            if (plantedCrop != previousCrop)
            {
                Destroy(summonedCrop);
            
                SummonCrop();

                timer = plantedCrop.cropGrowTime;

                previousCrop = plantedCrop;
            }

            GrowCrops();
        }
        #endregion
    }
    #endregion

    #region Custom Functions
    #region Public Functions
    public void PlantSeed(Crop crop)
    {
        plantedCrop = crop;

        SummonCrop();

        timer = plantedCrop.cropGrowTime;

        previousCrop = plantedCrop;
    }
    #endregion

    void GrowCrops()
    {
        timer -= timer >= 0 ? Time.deltaTime : 0;

        #region Update Scale & Hover
        if (timer <= 0) 
            summonedCrop.Hover();
        else
        {
            float lerpAmount = (plantedCrop.cropGrowTime - timer) / plantedCrop.cropGrowTime;
            summonedCrop.transform.localScale = Vector3.Lerp(Vector3.one * 0.001f, Vector3.one, lerpAmount);
        }
        #endregion
    }

    void SummonCrop()
    {
        summonedCrop = Instantiate(plantedCrop.cropGameObject, transform.position, Quaternion.identity).GetComponent<CropScript>();

        #region Initialization
        summonedCrop.transform.localScale = Vector3.zero;
        summonedCrop.transform.SetParent(transform);

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
