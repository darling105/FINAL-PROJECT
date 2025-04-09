using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftLegModelChanger : MonoBehaviour
{
    public List<GameObject> legModels;

    private void Awake()
    {
        GetAllLeftLegModels();
    }

    private void GetAllLeftLegModels()
    {
        int childrenGameObjects = transform.childCount;

        for (int i = 0; i < childrenGameObjects; i++)
        {
            legModels.Add(transform.GetChild(i).gameObject);
        }
    }

    public void UnEquipAllLeftLegModels()
    {
        foreach (GameObject legModel in legModels)
        {
            legModel.SetActive(false);
        }
    }

    public void EquipLeftLegModelByName(string legName)
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
