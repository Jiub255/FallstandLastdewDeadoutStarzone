using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBInstantiator : MonoBehaviour
{
	[SerializeField]
	private List<SOObjData> _objDataSOs;
  

    private void Awake()
    {
        List<GameObject> instances = new List<GameObject>();
        
        foreach (SOObjData objDataSO in _objDataSOs)
        {
            instances.Add(Instantiate(objDataSO.Prefab));

            foreach (SOComponent soComp in objDataSO.SOComponents)
            {
                soComp.GOInstances.AddRange(instances);
                soComp.MBAwake();
            }
        }

    }

    /// <summary>
    /// Seems a bit sloppy. Use linq? Or just give up because it's a stupid idea? <br/>
    /// Need to call start on each SOComponent, but only once each. 
    /// </summary>
    private void Start()
    {
        List<SOComponent> eachCompOnce = new();
        foreach (SOObjData objDataSO in _objDataSOs)
        {
            foreach (SOComponent soComp in objDataSO.SOComponents)
            {
                if (!eachCompOnce.Contains(soComp))
                    eachCompOnce.Add(soComp);
            }
        }

        foreach (SOComponent soComp in eachCompOnce)
        {
            soComp.Start();
        }
    }
}