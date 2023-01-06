using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotScript : MonoBehaviour
{
    public Crop plantedCrop;
    Crop previousCrop;

    public List<GameObject> summonedCrops;
    public bool readyToHarvest;

    float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (plantedCrop == null) return;

        if (plantedCrop != previousCrop)
        {
            foreach (GameObject cropObject in summonedCrops)
            {
                Destroy(cropObject);
            }
            summonedCrops.Clear();
            SummonCrops();
            timer = plantedCrop.cropGrowTime;

            previousCrop = plantedCrop;
        }

        GrowCrops();
    }

    void GrowCrops()
    {
        timer -= timer >= 0 ? Time.deltaTime : 0;
        if (timer < 0) readyToHarvest = true;

        foreach (GameObject crop in summonedCrops)
        {
            float lerpAmount = (plantedCrop.cropGrowTime - timer) / plantedCrop.cropGrowTime;
            crop.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, lerpAmount);
        }
    }

    void SummonCrops()
    {
        float spacingX;
        float spacingZ;
        if (plantedCrop.cropDimensions.x == 1) spacingX = 0;
        else spacingX = (3f - 2 * plantedCrop.cropMargins.x) / (plantedCrop.cropDimensions.x - 1);
        if (plantedCrop.cropDimensions.y == 1) spacingZ = 0;
        else spacingZ = (3f - 2 * plantedCrop.cropMargins.y) / (plantedCrop.cropDimensions.y - 1);

        for (int x = 0; x < plantedCrop.cropDimensions.x; x++)
        {
            for (int z = 0; z < plantedCrop.cropDimensions.y; z++)
            {
                float pos_x = x * spacingX;
                float pos_z = z * spacingZ;

                float margin_x = plantedCrop.cropDimensions.y == 1 ? plantedCrop.cropMargins.x : 1.5f;
                float margin_z = plantedCrop.cropDimensions.x == 1 ? plantedCrop.cropMargins.y : 1.5f;

                Vector3 position = new Vector3(pos_x - (1.5f) + margin_x, 0, pos_z - (1.5f) + margin_z) + transform.position;

                SummonCrop(position);
            }
        }

    }

    void SummonCrop(Vector3 localPosition)
    {
        GameObject cropObject = Instantiate(plantedCrop.cropObject, localPosition, Quaternion.identity);
        cropObject.transform.localScale = Vector3.zero;
        summonedCrops.Add(cropObject);
    }
}
