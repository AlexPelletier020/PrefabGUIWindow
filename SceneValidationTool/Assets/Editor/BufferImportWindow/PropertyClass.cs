using System;

public class PropertyClass//<T>
{
    public string name;
    public string description;
    public int type;
    public int minValue;
    public int maxValue;
    public object value;
    //public PropertyValue<T> m_value;

    public PropertyClass()
    {
        name = "";
        description = "";
        type = 0;
        minValue = 0;
        maxValue = 1;
    }

    public void SetValue(object propValue)
    {
        value = propValue;
    }

    //public void SetProperlyValue(PropertyValue<T> property)
    //{
    //    m_value = property;
    //}
}

public enum PROPERTY_TYPE
{
    StringField = 1,
    FloatField,
    IntegerField,
    MaterialField,
    LightField,
    BooleanField,
    FloatSlider,
    IntegerSlider
}

public struct PropertyValue<T>
{ 
    public T Value;
    public PROPERTY_TYPE type;

    public void SetValue(T newValue, PROPERTY_TYPE newType)
    {
        Value = newValue;
        type = newType;
    }
}

