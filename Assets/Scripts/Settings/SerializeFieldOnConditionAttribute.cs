using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

internal enum ComparisonType
{
    Equals = 1,
    NotEqual = 2,
    GreaterThan = 3,
    SmallerThan = 4,
    SmallerOrEqual = 5,
    GreaterOrEqual = 6
}
#if UNITY_EDITOR
internal sealed class SerializeFieldOnConditionAttribute : PropertyAttribute
{
    internal string comparedPropertyName { get; private set; }
    internal object comparedValue { get; private set; }
    internal ComparisonType comparisonType { get; private set; }
    internal SerializeFieldOnConditionAttribute(string comparedPropertyName, object comparedValue, ComparisonType comparisonType)
    {
        this.comparedPropertyName = comparedPropertyName;
        this.comparedValue = comparedValue;
        this.comparisonType = comparisonType;
    }
    internal bool IsConditionMet(SerializedProperty property)
    {
        bool conditionMet = false;
        if (property.serializedObject.FindProperty(comparedPropertyName) != null)
        {
            SerializedProperty comparedProperty = property.serializedObject.FindProperty(comparedPropertyName);

            if (comparisonType == ComparisonType.Equals)
                conditionMet = (GetSerializedPropertyValue(comparedProperty)).Equals(comparedValue);
            else if (comparisonType == ComparisonType.NotEqual)
                conditionMet = !(GetSerializedPropertyValue(comparedProperty)).Equals(comparedValue);
            else
                conditionMet = IsConditionMetNumeric(comparedProperty);
        }
        else
        {
            Debug.LogError($"Property '{comparedPropertyName}' not found in serializedObject.");
        }
        return conditionMet;
    }

    private bool IsConditionMetNumeric(SerializedProperty comparedProperty)
    {
        if (comparedProperty.numericType != SerializedPropertyNumericType.Unknown && comparedValue.GetType().IsNumeric())
        {
            // Use GetNumericValue to get the numeric value of properties
            var comparedPropertyValue = GetNumericValue(comparedProperty, comparedProperty.numericType);

            // Perform the comparison based on the specified ComparisonType
            switch (comparisonType)
            {
                case ComparisonType.GreaterThan:
                    return comparedPropertyValue > (double)comparedValue;
                case ComparisonType.SmallerThan:
                    return comparedPropertyValue < (double)comparedValue;
                case ComparisonType.GreaterOrEqual:
                    return comparedPropertyValue >= (double)comparedValue;
                case ComparisonType.SmallerOrEqual:
                    return comparedPropertyValue <= (double)comparedValue;
                default:
                    Debug.LogError($"ComparisonType '{comparisonType}' is not supported.");
                    return false;
            }
        }
        else
        {
            if (comparedProperty.numericType != SerializedPropertyNumericType.Unknown)
                Debug.LogError($"Property '{comparedProperty.name}' of type '{comparedProperty.propertyType}' is not of type Numeric, so it can't be compared with this ComparisonType!");
            if (!comparedValue.GetType().IsNumeric())
                Debug.LogError($"Property '{comparedValue}' is not of type Numeric, so it can't be compared with this ComparisonType!");
            return false;
        }
    }

    private double GetNumericValue(SerializedProperty property, SerializedPropertyNumericType type)
    {
        switch (type)
        {
            // Adjust cases based on propertyType
            //case SerializedPropertyNumericType.Byte:
            //    return (byte)property.intValue;
            //case SerializedPropertyNumericType.SByte:
            //    return (sbyte)property.intValue;
            case SerializedPropertyNumericType.UInt16:
                return (ushort)property.intValue;
            case SerializedPropertyNumericType.UInt32:
                return (uint)property.longValue;
            case SerializedPropertyNumericType.UInt64:
                return (ulong)property.longValue;
            case SerializedPropertyNumericType.Int16:
                return (short)property.intValue;
            case SerializedPropertyNumericType.Int32:
                return property.intValue;
            case SerializedPropertyNumericType.Int64:
                return property.longValue;
            //case SerializedPropertyNumericType.Decimal:
            //    return (double)(decimal)property.doubleValue;
            case SerializedPropertyNumericType.Double:
                return property.doubleValue;
            case SerializedPropertyNumericType.Float:
                return property.floatValue;
            default:
                throw new NotSupportedException($"Type {type} is not a supported numeric type.");
        }
    }
    internal object GetSerializedPropertyValue(SerializedProperty property)
    {
        switch (property.propertyType)
        {
            case SerializedPropertyType.Integer:
                return property.intValue;
            case SerializedPropertyType.Boolean:
                return property.boolValue;
            case SerializedPropertyType.Float:
                return property.floatValue;
            case SerializedPropertyType.String:
                return property.stringValue;
            case SerializedPropertyType.Enum:
                return property.enumValueIndex;
            case SerializedPropertyType.ObjectReference:
                return property.objectReferenceValue;
            case SerializedPropertyType.Color:
                return property.colorValue;
            case SerializedPropertyType.LayerMask:
                return property.intValue; // LayerMask is internally stored as an int
            case SerializedPropertyType.Vector2:
                return property.vector2Value;
            case SerializedPropertyType.Vector3:
                return property.vector3Value;
            case SerializedPropertyType.Vector4:
                return property.vector4Value;
            case SerializedPropertyType.Rect:
                return property.rectValue;
            case SerializedPropertyType.ArraySize:
                return property.arraySize;
            default:
                Debug.LogError($"Unsupported propertyType: {property.propertyType}");
                return null;
        }
    }
}

[CustomPropertyDrawer(typeof(SerializeFieldOnConditionAttribute))]
internal sealed class SerializeFieldOnConditionPropertyDrawer : PropertyDrawer
{
    SerializeFieldOnConditionAttribute attr;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        attr = attribute as SerializeFieldOnConditionAttribute;

        EditorGUI.BeginProperty(position, label, property);
        int indentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        if (attr.IsConditionMet(property))
            EditorGUI.PropertyField(position, property);
        else
            position.height = 0;

        EditorGUI.indentLevel = indentLevel;
        EditorGUI.EndProperty();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (attr == null)
            return 0;
        if (!attr.IsConditionMet(property))
            return 0f;

        return base.GetPropertyHeight(property, label);
    }
}
#endif