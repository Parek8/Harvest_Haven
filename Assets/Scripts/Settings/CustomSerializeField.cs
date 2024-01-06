//// CREDITS: https://forum.unity.com/threads/draw-a-field-only-if-a-condition-is-met.448855/

//using UnityEngine;
//using System;
//using UnityEditor;
//using Unity.VisualScripting;

//[CustomPropertyDrawer(typeof(DrawIfAttribute))]
//internal class DrawIfPropertyDrawer : PropertyDrawer
//{
//    // Reference to the attribute on the property.
//    DrawIfAttribute drawIf;

//    // Field that is being compared.
//    SerializedProperty comparedField;

//    // Height of the property.
//    private float propertyHeight;

//    internal override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//        return propertyHeight;
//    }

//    internal override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        // Set the global variables.
//        drawIf = attribute as DrawIfAttribute;
//        comparedField = property.serializedObject.FindProperty(drawIf.comparedPropertyName);

//        // Get the value of the compared field.
//        object comparedFieldValue = comparedField.GetUnderlyingValue();

//        // References to the values as numeric types.
//        NumericType numericComparedFieldValue = null;
//        NumericType numericComparedValue = null;

//        try
//        {
//            // Try to set the numeric types.
//            numericComparedFieldValue = new NumericType(comparedFieldValue);
//            numericComparedValue = new NumericType(drawIf.comparedValue);
//        }
//        catch (NumericTypeExpectedException)
//        {
//            // This place will only be reached if the type is not a numeric one. If the comparison type is not valid for the compared field type, log an error.
//            if (drawIf.comparisonType != ComparisonType.Equals && drawIf.comparisonType != ComparisonType.NotEqual)
//            {
//                Debug.LogError("The only comparsion types available to type '" + comparedFieldValue.GetType() + "' are Equals and NotEqual. (On object '" + property.serializedObject.targetObject.name + "')");
//                return;
//            }
//        }

//        // Is the condition met? Should the field be drawn?
//        bool conditionMet = false;

//        // Compare the values to see if the condition is met.
//        switch (drawIf.comparisonType)
//        {
//            case ComparisonType.Equals:
//                if (comparedFieldValue.Equals(drawIf.comparedValue))
//                    conditionMet = true;
//                break;

//            case ComparisonType.NotEqual:
//                if (!comparedFieldValue.Equals(drawIf.comparedValue))
//                    conditionMet = true;
//                break;

//            case ComparisonType.GreaterThan:
//                if (numericComparedFieldValue > numericComparedValue)
//                    conditionMet = true;
//                break;

//            case ComparisonType.SmallerThan:
//                if (numericComparedFieldValue < numericComparedValue)
//                    conditionMet = true;
//                break;

//            case ComparisonType.SmallerOrEqual:
//                if (numericComparedFieldValue <= numericComparedValue)
//                    conditionMet = true;
//                break;

//            case ComparisonType.GreaterOrEqual:
//                if (numericComparedFieldValue >= numericComparedValue)
//                    conditionMet = true;
//                break;
//        }

//        // The height of the property should be defaulted to the default height.
//        propertyHeight = base.GetPropertyHeight(property, label);

//        // If the condition is met, simply draw the field. Else...
//        if (conditionMet)
//        {
//            EditorGUI.PropertyField(position, property);
//        }
//        else
//        {
//            //...check if the disabling type is read only. If it is, draw it disabled, else, set the height to zero.
//            if (drawIf.disablingType == DisablingType.ReadOnly)
//            {
//                GUI.enabled = false;
//                EditorGUI.PropertyField(position, property);
//                GUI.enabled = true;
//            }
//            else
//            {
//                propertyHeight = 0f;
//            }
//        }
//    }
//}

//internal class NumericType<T> : IEquatable<NumericType<T>>
//{
//    private T value;
//    private Type type;

//    internal NumericType(T obj)
//    {
//        if (!typeof(T).IsNumeric())
//        {
//            // Something bad happened.
//            throw new NumericTypeExpectedException("The type inputted into the NumericType generic must be a numeric type.");
//        }
//        type = typeof(T);
//        value = obj;
//    }

//    internal T GetValue()
//    {
//        return value;
//    }

//    internal object GetValueAsObject()
//    {
//        return value;
//    }

//    internal void SetValue(T newValue)
//    {
//        value = newValue;
//    }

//    internal bool Equals(NumericType<T> other)
//    {
//        return this == other;
//    }

//    internal override bool Equals(object obj)
//    {
//        if (obj != null && !(obj is NumericType<T>))
//            return false;

//        return Equals(obj);
//    }

//    internal override int GetHashCode()
//    {
//        return GetValue().GetHashCode();
//    }

//    internal override string ToString()
//    {
//        return GetValue().ToString();
//    }

//    /// <summary>
//    /// Checks if the value of left is smaller than the value of right.
//    /// </summary>
//    internal static bool operator <(NumericType<T> left, NumericType<T> right)
//    {
//        object leftValue = left.GetValueAsObject();
//        object rightValue = right.GetValueAsObject();

//        switch (Type.GetTypeCode(left.type))
//        {
//            case TypeCode.Byte:
//                return (byte)leftValue < (byte)rightValue;

//            case TypeCode.SByte:
//                return (sbyte)leftValue < (sbyte)rightValue;

//            case TypeCode.UInt16:
//                return (ushort)leftValue < (ushort)rightValue;

//            case TypeCode.UInt32:
//                return (uint)leftValue < (uint)rightValue;

//            case TypeCode.UInt64:
//                return (ulong)leftValue < (ulong)rightValue;

//            case TypeCode.Int16:
//                return (short)leftValue < (short)rightValue;

//            case TypeCode.Int32:
//                return (int)leftValue < (int)rightValue;

//            case TypeCode.Int64:
//                return (long)leftValue < (long)rightValue;

//            case TypeCode.Decimal:
//                return (decimal)leftValue < (decimal)rightValue;

//            case TypeCode.Double:
//                return (double)leftValue < (double)rightValue;

//            case TypeCode.Single:
//                return (float)leftValue < (float)rightValue;
//        }
//        throw new NumericTypeExpectedException("Please compare valid numeric types with numeric generics.");
//    }

//    /// <summary>
//    /// Checks if the value of left is greater than the value of right.
//    /// </summary>
//    internal static bool operator >(NumericType<T> left, NumericType<T> right)
//    {
//        object leftValue = left.GetValueAsObject();
//        object rightValue = right.GetValueAsObject();

//        switch (Type.GetTypeCode(left.type))
//        {
//            case TypeCode.Byte:
//                return (byte)leftValue > (byte)rightValue;

//            case TypeCode.SByte:
//                return (sbyte)leftValue > (sbyte)rightValue;

//            case TypeCode.UInt16:
//                return (ushort)leftValue > (ushort)rightValue;

//            case TypeCode.UInt32:
//                return (uint)leftValue > (uint)rightValue;

//            case TypeCode.UInt64:
//                return (ulong)leftValue > (ulong)rightValue;

//            case TypeCode.Int16:
//                return (short)leftValue > (short)rightValue;

//            case TypeCode.Int32:
//                return (int)leftValue > (int)rightValue;

//            case TypeCode.Int64:
//                return (long)leftValue > (long)rightValue;

//            case TypeCode.Decimal:
//                return (decimal)leftValue > (decimal)rightValue;

//            case TypeCode.Double:
//                return (double)leftValue > (double)rightValue;

//            case TypeCode.Single:
//                return (float)leftValue > (float)rightValue;
//        }
//        throw new NumericTypeExpectedException("Please compare valid numeric types.");
//    }

//    /// <summary>
//    /// Checks if the value of left is the same as the value of right.
//    /// </summary>
//    internal static bool operator ==(NumericType<T> left, NumericType<T> right)
//    {
//        return !(left > right) && !(left < right);
//    }

//    /// <summary>
//    /// Checks if the value of left is not the same as the value of right.
//    /// </summary>
//    internal static bool operator !=(NumericType<T> left, NumericType<T> right)
//    {
//        return !(left > right) || !(left < right);
//    }

//    /// <summary>
//    /// Checks if left is either equal or smaller than right.
//    /// </summary>
//    internal static bool operator <=(NumericType<T> left, NumericType<T> right)
//    {
//        return left == right || left < right;
//    }

//    /// <summary>
//    /// Checks if left is either equal or greater than right.
//    /// </summary>
//    internal static bool operator >=(NumericType<T> left, NumericType<T> right)
//    {
//        return left == right || left > right;
//    }
//}