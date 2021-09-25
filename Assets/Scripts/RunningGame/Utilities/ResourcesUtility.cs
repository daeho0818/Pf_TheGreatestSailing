using UnityEngine;

public static class ResourcesUtility
{
    public static T Load<T>(string prefabPath) where T : Component
    {
        var prefab = Resources.Load<GameObject>(prefabPath);
        return prefab.GetComponentInChildren<T>(includeInactive: true);
    }

    public static T Instantiate<T>(string prefabPath, Transform parent = null) where T : Component
    {
        var prefab = Load<T>(prefabPath);
        return GameObject.Instantiate(prefab, parent);
    }

    public static T Instantiate<T>(string prefabPath, Vector3 position, Quaternion rotation) where T : Component
    {
        var prefab = Load<T>(prefabPath);
        return GameObject.Instantiate(prefab, position, rotation);
    }
}
