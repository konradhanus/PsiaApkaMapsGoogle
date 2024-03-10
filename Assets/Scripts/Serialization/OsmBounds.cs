using System.Xml;
using UnityEngine;

// Poprawiony konstruktor klasy OsmBounds
// Poprawiony konstruktor klasy OsmBounds
public class OsmBounds : BaseOsm
{
    public double MinLat { get; private set; }
    public double MinLon { get; private set; }
    public double MaxLat { get; private set; }
    public double MaxLon { get; private set; }
    public Vector3 Centre { get; private set; }

    public OsmBounds(XmlNode node)
    {
        MinLat = GetAttribute<double>("minlat", node.Attributes);
        MinLon = GetAttribute<double>("minlon", node.Attributes);
        MaxLat = GetAttribute<double>("maxlat", node.Attributes);
        MaxLon = GetAttribute<double>("maxlon", node.Attributes);

        // Oblicz środek granic mapy
        float x = (float)((MercatorProjection.lonToX(MaxLon) + MercatorProjection.lonToX(MinLon)) / 2.0);
        float y = 0;  // Ustaw y na stałą wartość lub oblicz na podstawie innych danych
        float z = (float)((MercatorProjection.latToY(MaxLat) + MercatorProjection.latToY(MinLat)) / 2.0);

        Centre = new Vector3(x, y, z);
    }
}
