using UnityEngine;

public class UICanvas : MonoBehaviour
{
    private static UICanvas _instance;
    public static UICanvas Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = ResourcesUtility.Instantiate<UICanvas>("RunningGame/Prefabs/UI/UICanvas");
                DontDestroyOnLoad(_instance);
            }

            return _instance;
        }
    }
}
