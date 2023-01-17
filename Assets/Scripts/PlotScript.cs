using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotScript : MonoBehaviour
{
    public Crop plantedCrop;
    Crop previousCrop;
    CropScript summonedCrop;

    float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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

    }

    void GrowCrops()
    {
        timer -= timer >= 0 ? Time.deltaTime : 0;
        if (timer < 0) summonedCrop.Hover();
        else
        {
            float lerpAmount = (plantedCrop.cropGrowTime - timer) / plantedCrop.cropGrowTime;
            summonedCrop.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, lerpAmount);
        }
    }

    void SummonCrop()
    {
        GameObject cropObject = Instantiate(plantedCrop.cropObject, transform.position, Quaternion.identity);
        cropObject.transform.localScale = Vector3.zero;
        cropObject.transform.SetParent(transform);
        summonedCrop = cropObject.GetComponent<CropScript>();
        summonedCrop.plotScript = this;
        GameManager.crops.Add(summonedCrop);
    }

    public void Harvest()
    {
        for (int i = 0; i < Random.Range(plantedCrop.minHarvestCount, plantedCrop.maxHarvestCount + 1); i++)
        {
            GameObject droppedCrop = Instantiate(plantedCrop.grabbableCropObject, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            droppedCrop.transform.SetParent(GameManager.singleton.transform);
        }
        GameManager.crops.Remove(summonedCrop);
        Destroy(summonedCrop.gameObject);
        summonedCrop = null;
        plantedCrop = null;
    }
}
