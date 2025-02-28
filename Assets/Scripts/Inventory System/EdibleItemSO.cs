using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    [CreateAssetMenu]
    public class EdibleItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        // [SerializeField]
        // private List<ModifierData> modifiersData = new List<ModifierData>();
        public float HealthRestore;
        public string ActionName => "Consume";

        public bool PerformAction(GameObject character/*, List<ItemParameter> itemState = null*/)
        {
            Debug.Log("health restore is :" +HealthRestore);
            return true;
        }
    }

    public interface IDestroyableItem
    {

    }

    public interface IItemAction
    {
        public string ActionName { get; }
        bool PerformAction(GameObject character);
    }

    [Serializable]
    public class ModifierData
    {
        //public CharacterStatModifierSO statModifier;
        public float value;
    }
