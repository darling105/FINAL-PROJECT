using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSaveSystem : MonoBehaviour
{
    [SerializeField ] private InventorySO inventoryData;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)){
            Save();
        }
    }
    private void Save(){
        
        Debug.Log("Saving");
    }
}
