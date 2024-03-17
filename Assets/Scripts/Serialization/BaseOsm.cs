using System;
using System.Xml;
using UnityEngine;


public class BaseOsm
{
    protected static T GetAttribute<T>(string attrName, XmlAttributeCollection attributes)
    {
        XmlAttribute attribute = attributes[attrName];
        if (attribute != null)
        {
            try
            {
                return (T)Convert.ChangeType(attribute.Value, typeof(T), System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                Debug.LogError($"Error converting attribute {attrName} to type {typeof(T)}");
            }
        }
        return default(T);
    }
}



