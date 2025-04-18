using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UniversalHelper
{
    public static string generateUniqueID(GameObject gameObject) {
        return $"{gameObject.scene.name}_{gameObject.transform.position.x}_{gameObject.transform.position.y}";
    }
}
