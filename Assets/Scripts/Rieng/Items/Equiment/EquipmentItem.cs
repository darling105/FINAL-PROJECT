using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentItem : Item
{
    
    [Header("Defense Bonus")]
    public float physicalDefense;
    public float fireDefense;

    [Header("Resistances")]
    public float poisonResistance;
}
