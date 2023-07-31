using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/SOTool", fileName = "New Tool SO")]
public class SOTool : SOItem
{
    /// <summary>
    /// What to do here? Anything? Could have a tool info popup. <br/>
    /// For now, do nothing since there's no inventory to display them anyway. Just used as a recipe requirement for now. 
    /// </summary>
    public override void OnClickInventory()
    {

    }
}