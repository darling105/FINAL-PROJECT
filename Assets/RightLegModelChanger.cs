using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightLegModelChanger : MonoBehaviour
{

    public List<GameObject> legModels;

    private void Awake()
    {
        GetAllRightLegModels();
    }

    private void GetAllRightLegModels()
    {
        int childrenGameObjects = transform.childCount;

        for (int i = 0; i < childrenGameObjects; i++)
        {
            legModels.Add(transform.GetChild(i).gameObject);
        }
    }

    public void UnEquipAllRightLegModels()
    {
        foreach (GameObject legModel in legModels)
        {
            legModel.SetActive(false);
        }
    }

    public void EquipRightLegModelByName(string legName)
    {
        for (int i = 0; i < legModels.Count; i++)
        {
            if (legModels[i].name == legName)
            {
                legModels[i].SetActive(true);
            }
            else
            {
                legModels[i].SetActive(false);
            }
        }
    }
}
