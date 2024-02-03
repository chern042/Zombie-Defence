using UnityEngine;
using System.Collections.Generic;
using System;
using System.ComponentModel;

public static class SceneHelper
{
    public static void SetLayerAllChildren(Transform root, int layer)
    {
        var children = root.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            child.gameObject.layer = layer;
        }
    }


}
