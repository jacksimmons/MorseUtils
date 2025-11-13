using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class TransformTools
{
    public static void KillAllChildren(this Transform transform, Transform[] exclude = null)
    {
        List<Transform> children = new();
        foreach (Transform child in transform)
            children.Add(child);

        foreach (Transform child in children)
        {
            if (exclude != null && exclude.Contains(child))
            {
                continue;
            }
            GameObject.Destroy(child.gameObject);
        }
    }


    public static T GetRandomComponent<T>(this Transform transform)
    {
        var components = transform.GetComponents<T>();
        return components[Random.Range(0, components.Length)];
    }
}