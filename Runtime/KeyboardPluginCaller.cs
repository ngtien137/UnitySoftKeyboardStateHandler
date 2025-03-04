using System;
using UnityEngine;
using UnityEngine.Events;


namespace Venaluza
{
    public class KeyboardPluginCaller : MonoBehaviour
    {
        private static KeyboardPluginCaller instance;

        public static KeyboardPluginCaller Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<KeyboardPluginCaller>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject("KeyboardPluginCaller");
                        instance = obj.AddComponent<KeyboardPluginCaller>();
                    }
                }

                return instance;
            }
        }

        // Unity event kết hợp: (true, height) khi bàn phím hiển thị, (false, 0) khi bàn phím ẩn
        [SerializeField] private UnityEvent<bool, int> OnKeyboardStateChanged = new();

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                //DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }


#if UNITY_ANDROID && !UNITY_EDITOR
    void Start()
    {
        // Lấy Activity hiện tại từ Unity
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        // Lấy lớp plugin của bạn
        AndroidJavaClass keyboardPlugin = new AndroidJavaClass("com.venaluza.keyboardplugin.KeyboardHeightPlugin");

        // Bật log (có thể điều chỉnh qua setLogEnabled nếu cần)
        keyboardPlugin.CallStatic("setLogEnabled", true);

        // Khởi tạo listener bàn phím, đảm bảo GameObject truyền vào trùng với tên của đối tượng này.
        keyboardPlugin.CallStatic("initKeyboardListener", currentActivity, gameObject.name);
    }
#endif

        // Phương thức nhận thông điệp từ plugin Android
        public void OnKeyboardHeightChanged(string heightValue)
        {
            Debug.Log("Keyboard height changed: " + heightValue);
            if (int.TryParse(heightValue, out int keyboardHeight))
            {
                if (keyboardHeight > 0)
                {
                    // Bàn phím hiển thị: Invoke event với trạng thái true và chiều cao bàn phím
                    OnKeyboardStateChanged?.Invoke(true, keyboardHeight);
                }
                else
                {
                    // Bàn phím ẩn: Invoke event với trạng thái false và chiều cao 0
                    OnKeyboardStateChanged?.Invoke(false, 0);
                }
            }
            else
            {
                Debug.LogError("Failed to parse keyboard height: " + heightValue);
            }
        }

        /// <summary>
        /// Đăng ký callback để nhận sự kiện thay đổi trạng thái bàn phím.
        /// </summary>
        /// <param name="callback">Hàm callback có kiểu UnityAction<bool, int> (true, height) khi bàn phím hiển thị, (false, 0) khi ẩn.</param>
        public static void Register(UnityAction<bool, int> callback)
        {
            Instance.OnKeyboardStateChanged.AddListener(callback);
        }

        /// <summary>
        /// Hủy đăng ký callback đã đăng ký trước đó.
        /// </summary>
        /// <param name="callback">Callback cần hủy đăng ký.</param>
        public static void Unregister(UnityAction<bool, int> callback)
        {
            Instance.OnKeyboardStateChanged.RemoveListener(callback);
        }
    }
}