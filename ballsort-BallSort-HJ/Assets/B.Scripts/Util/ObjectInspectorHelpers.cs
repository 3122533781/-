using UnityEngine;
using System.Collections;
using System;


namespace Prime31
{

	/// <summary>
	/// implement this empty interface to tell the ObjectInspector that your class should be inspected
	/// </summary>
	public interface IObjectInspectable
	{}


	/// <summary>
	/// makes any method have a button in the inspector to call it
	/// </summary>
	[AttributeUsageAttribute( AttributeTargets.Method )]
	public class MakeButtonAttribute : Attribute
	{}


	/// <summary>
	/// adds a vector3 editor to the scene GUI if this attribute is on any serialized Vector3, List<Vector3>
	/// or Vector3[] fields.
	/// </summary>
	[AttributeUsageAttribute( AttributeTargets.Field )]
	public class Vector3Inspectable : Attribute
	{}


    public class CategoryFieldAttribute : Attribute
    {
        public string ID;

        public CategoryFieldAttribute(string id)
        {
            ID = id;
        }
    }

    public class IfContentAttribute : Attribute
    {
        public string ConditionName;

        public IfContentAttribute(string condition)
        {
            ConditionName = condition;
        }
    }
}