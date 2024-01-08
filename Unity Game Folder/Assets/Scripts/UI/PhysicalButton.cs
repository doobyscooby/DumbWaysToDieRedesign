using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class PhysicalButton : MonoBehaviour, ISerializationCallbackReceiver
{
    // Button
    [SerializeField]
    private GameObject onClickObject;

    [SerializeField][HideInInspector]
    private Component currentComponent;

    public static List<string> tempList;
    [HideInInspector]
    public List<string> popupList;

    public static List<string> tempList2;
    [HideInInspector]
    public List<string> popupList2;

    // Popup List 1
    [ListToPopup(typeof(PhysicalButton), "tempList")]
    public string PopupComponents;

    // Popup List 2
    [ListToPopup(typeof(PhysicalButton), "tempList2")]
    public string PopupOfMethods;
    public List<string> GetAllComponents()
    {
        Component[] listOfComponents = onClickObject.GetComponents(typeof(Component));
        List<string> listOfComponentsString = new List<string>();

        for (int i = 0; i < listOfComponents.Length; i++)
        {
            listOfComponentsString.Add(listOfComponents[i].ToString());
        }
        return listOfComponentsString;
    }
    public List<string> GetAllMethods()
    {
        // List of Methods
        var methods = new List<MethodInfo>();
        Component componentNeeded = GetCurrentComponent();
        if (componentNeeded == null)
            return null;
        // Add methods from component
        methods.AddRange(componentNeeded.GetType().GetTypeInfo().DeclaredMethods);
        List<string> listOfMethodsString = new List<string>();

        var methodsArray = methods.ToArray();
        for (int i = 0; i < methods.Count; i++)
        {
            listOfMethodsString.Add(methodsArray[i].ToString());
        }
        return listOfMethodsString;
    }

    public void OnAfterDeserialize(){}

    public void OnBeforeSerialize()
    {
        popupList = GetAllComponents();
        tempList = popupList;
        popupList2 = GetAllMethods();
        tempList2 = popupList2;
    }
    Component GetCurrentComponent()
    {
        // List of Methods
        var methods = new List<MethodInfo>();
        Component componentNeeded = null;
        // Temp Component
        var tempComponents = onClickObject.GetComponents<Component>();
        foreach (var component in tempComponents)
        {
            if (component.ToString() == PopupComponents)
            {
                currentComponent = component;
                componentNeeded = currentComponent;
                return componentNeeded;
            }
        }
        return null;
    }
    MethodInfo GetCurrentMethod()
    {
        // List of Methods
        var methods = new List<MethodInfo>();

        methods.AddRange(currentComponent.GetType().GetTypeInfo().DeclaredMethods);
        List<string> listOfMethodsString = new List<string>();

        var methodsArray = methods.ToArray();
        for (int i = 0; i < methods.Count; i++)
        {
            if (PopupOfMethods == methodsArray[i].ToString())
            {
                return methods[i];
            }
        }
        return null;
    }
    private void OnMouseDown()
    {

        MethodInfo methodToCall = GetCurrentMethod();
        if (methodToCall != null)
        {
            Object[] parameters = null;
            methodToCall.Invoke(currentComponent, parameters);
        }
    }
}