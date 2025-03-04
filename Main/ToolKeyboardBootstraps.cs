using UnityEngine;

public class ToolKeyboardBootstraps : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void GenerateGameBootstrapComponent()
    {
        // Tìm thử instance
        var existing = Object.FindObjectOfType<KeyboardPluginCaller>();
        if (existing == null)
        {
            var prefab = Resources.Load<GameObject>("KeyboardPluginCaller");
            if (prefab != null)
            {
                var obj = Object.Instantiate(prefab);
                Object.DontDestroyOnLoad(obj);
            }
        }
        else
        {
            // Đã có rồi thì không tạo mới
        }
    }
}