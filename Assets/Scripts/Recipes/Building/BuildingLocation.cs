using UnityEngine;

/// <summary>
/// int BuildingID, Vector3 Position, Quaternion Rotation. <br/>
/// TODO - Using public fields instead of properties to get them to serialize. Is it necessary? 
/// </summary>
[System.Serializable]
public class BuildingLocation
{
    public int BuildingID; /*{ get; }*/
    public Vector3 Position; /*{ get; }*/
    public Quaternion Rotation; /*{ get; }*/

    public BuildingLocation(int buildingID, Vector3 position, Quaternion rotation)
    {
        BuildingID = buildingID;
        Position = position;
        Rotation = rotation;
    }
}