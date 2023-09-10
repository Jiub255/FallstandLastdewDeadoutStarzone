using UnityEngine;

[System.Serializable]
public class BuildingLocation
{
    public SOBuilding BuildingSO { get; set; }
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }

    public BuildingLocation(SOBuilding buildingSO, Vector3 position, Quaternion rotation)
    {
        BuildingSO = buildingSO;
        Position = position;
        Rotation = rotation;
    }
}