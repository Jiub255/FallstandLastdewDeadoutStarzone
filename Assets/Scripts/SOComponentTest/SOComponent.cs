using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for scriptable object components. This way you can just have an object data SO with <br/>
/// a list of SOs that derive from this. They act just like monobehaviour components but you can apply them to data SOs <br/>
/// instead of GOs. 
/// </summary>
public abstract class SOComponent : ScriptableObject
{
	/// <summary>
	/// Do list instead? So it can be attached to multiple GOs? yes. 
	/// </summary>
	[field: SerializeField]
	public List<GameObject> GOInstances { get; set; }

	/// <summary>
	/// Called by the MB that instantiates/manages the GO that holds this SO. <br/>
	/// Must be called after instantiating GO in Awake! 
	/// </summary>
	public virtual void MBAwake() { }
	/// <summary>
	/// Called by the MB that instantiates/manages the GO that holds this SO. 
	/// </summary>
	public virtual void MBOnEnable() {}
	/// <summary>
	/// Called by the MB that instantiates/manages the GO that holds this SO. 
	/// </summary>
	public virtual void MBOnDisable() { }
	/// <summary>
	/// Called by the MB that instantiates/manages the GO that holds this SO. 
	/// </summary>
	public virtual void Start() { }
	/// <summary>
	/// Called by the MB that instantiates/manages the GO that holds this SO. 
	/// </summary>
	public virtual void Update() { }
	/// <summary>
	/// Called by the MB that instantiates/manages the GO that holds this SO. 
	/// </summary>
	public virtual void FixedUpdate() {}
}