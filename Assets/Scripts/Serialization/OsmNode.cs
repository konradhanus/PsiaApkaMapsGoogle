using System.Xml;
using UnityEngine;

/// <summary>
/// OSM node.
/// </summary>
class OsmNode : BaseOsm
{
    /// <summary>
    /// Node ID.
    /// </summary>
    public ulong ID { get; private set; }

    /// <summary>
    /// Latitude position of the node.
    /// </summary>
    public float Latitude { get; private set; }

    /// <summary>
    /// Longitude position of the node.
    /// </summary>
    public float Longitude { get; private set; }

    /// <summary>
    /// Unity unit X-co-ordinate.
    /// </summary>
    public float X { get; private set; }

    /// <summary>
    /// Unity unit Y-co-ordinate.
    /// </summary>
    public float Y { get; private set; }

    /// <summary>
    /// Implicit conversion between OsmNode and Vector3.
    /// </summary>
    /// <param name="node">OsmNode instance</param>
    public static implicit operator Vector3 (OsmNode node)
    {
        return new Vector3(node.X, 0, node.Y);
    }
    
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="node">Xml node</param>
    public OsmNode(XmlNode node)
    {
        // Get the attribute values
        ID = GetAttribute<ulong>("id", node.Attributes);
        Latitude = GetAttribute<float>("lat", node.Attributes);
        Longitude = GetAttribute<float>("lon", node.Attributes);

        // Calculate the position in Unity units
        X = (float)MercatorProjection.lonToX(Longitude);
        Y = (float)MercatorProjection.latToY(Latitude);
    }
}

