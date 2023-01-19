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
   /*
    *   Decrement the timer and lerps the 
    *   scale from zero to full depending 
    *   on the timer.
    */
    void GrowCrops()
    {
        //  Decreases the timer if the timer is more or equal to zero.
        timer -= timer >= 0 ? Time.deltaTime : 0;

        #region Update Scale & Hover
        /*
         *   If the timer is less or equal to 0 then 
         *   allow for the hover outline to be shown.
         *   If not, change the scale of the object.
         */
        if (timer <= 0) summonedCrop.Hover();
        else
        {
            float lerpAmount = (plantedCrop.cropGrowTime - timer) / plantedCrop.cropGrowTime;
            summonedCrop.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, lerpAmount);
        }
        #endregion
    }

    /*
     *   Instantiates the crop and sets up 
     *   some variables for later use.
     */
    void SummonCrop()
    {
        //  Instantiate the crop object.
        GameObject cropObject = Instantiate(plantedCrop.cropObject, transform.position, Quaternion.identity);

        #region Initialization
        //  Start the scale of the object at zero and set the parent to this. (Parented to remove clutter from the hierarchy)
        cropObject.transform.localScale = Vector3.zero;
        cropObject.transform.SetParent(transform);

        //  Adds the instantiated crop to the list of all crops in the game.
        summonedCrop = cropObject.GetComponent<CropScript>();
        GameManager.crops.Add(summonedCrop);

        //  Sets a variable for later use.
        summonedCrop.plotScript = this;
        #endregion
    }

   /*
    *   Harvests the plot. Destroys the crop
    *   and removes it from the global list.
    *   Instantiates the drops on top.
    */
    public void Harvest()
    {
        #region Instantiate Drops
        //  Instantiates the drops.
        for (int i = 0; i < Random.Range(plantedCrop.minHarvestCount, plantedCrop.maxHarvestCount + 1); i++)
        {
            GameObject droppedCrop = Instantiate(plantedCrop.grabbableCropObject, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            
            //  Reduce clutter in the hierarchy
            droppedCrop.transform.SetParent(GameManager.singleton.transform);
        }
        #endregion

        #region Remove Crop
        //  Removes the summonedCrop from the global list and destroys it.
        GameManager.crops.Remove(summonedCrop);
        Destroy(summonedCrop.gameObject);

        //  Reset some values.
        summonedCrop = null;
        plantedCrop = null;
        #endregion
    }
    #endregion
}
