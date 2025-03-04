# Unity Soft Keyboard State Handler

A lightweight Unity plugin for detecting and handling the Android soft keyboard state (show/hide) and height in real time. This plugin is especially useful for adjusting UI elements when the keyboard appears or disappears on Android devices.

# General

- **Purpose**:  
  - Detect when the soft keyboard is shown or hidden in Unity on Android.  
  - Get the keyboard height in pixels.  
  - Provide easy-to-use C# events (or a single event with parameters) to respond to keyboard changes.

- **Key Features**:  
  - Uses a native Android plugin to listen for keyboard visibility changes.  
  - Dispatches events into Unity via reflection (no direct dependency on `UnityPlayer` class).  
  - Singleton C# class in Unity that remains alive across scene loads (`DontDestroyOnLoad`).  
  - Simple registration/unregistration pattern for event callbacks.

- **Supported Platforms**:  
  - Primarily tested on **Android**.  
  - (Optional) On iOS, you can still compile, but you may need to adapt for iOS keyboard events or rely on `TouchScreenKeyboard.area`.

# Integration (Newest Version)

### 1. Via Unity Package Manager (UPM)

1. **Add from Git**  
   - In Unity, open **Window > Package Manager**.  
   - Click the **+** button → **Add package from git URL...**  
   - Enter:
     ```
     https://github.com/ngtien137/UnitySoftKeyboardStateHandler.git
     ```
     Optionally, specify a tag or branch (e.g., `#main`, `#v1.0.0`, etc.).

2. **Confirm Installation**  
   - Unity will download and install the package.  
   - You should see **UnitySoftKeyboardStateHandler** in the Package Manager list.

### 2. Via `.unitypackage`

1. **Download** the `.unitypackage` file
2. **Import** into your Unity project by selecting **Assets > Import Package > Custom Package...** and choosing the `.unitypackage` file.

### 3. Via Source (Copying Files)

1. Clone or download the repo:

git clone https://github.com/ngtien137/UnitySoftKeyboardStateHandler.git

2. Copy the content inside folder into your own Unity project, ensuring the folder structure remains consistent.  
3. Unity will import scripts automatically.

> **Note**: The recommended approach is **Package Manager** (Method 1) so you can easily update or remove the plugin.

# Guide

Below is a step-by-step overview of how to use the plugin in your Unity project once integrated.

### 1. Scene Setup

- After importing the package, a singleton script (e.g., `KeyboardPluginCaller`) is available.  
- It will automatically initialize on `Start()` when the scene is loaded. You do not need to manually create a GameObject unless you want to customize the script’s location.

### 2. How the Plugin Works

- **Native Android Plugin**:  
- Uses an Android `OnGlobalLayoutListener` to detect when the soft keyboard is shown or hidden.  
- Calculates keyboard height by comparing visible area vs. screen height.  
- Calls `UnitySendMessage` (via reflection) to notify Unity about the new keyboard state.

- **Unity Side**:  
- `KeyboardPluginCaller` is a singleton that does not get destroyed across scene loads.  
- When the keyboard changes state, the method `OnKeyboardHeightChanged(string heightValue)` is called.  
- This triggers an event (e.g., `OnKeyboardStateChanged`) or calls delegates you registered.

### 3. Registering for Keyboard Events

Inside `KeyboardPluginCaller`, there are static methods `Register` and `Unregister` that let you easily subscribe/unsubscribe to keyboard events:

```csharp
void OnEnable()
{
 // Subscribe to keyboard state changes
 KeyboardPluginCaller.Register(OnKeyboardStateChanged);
}

void OnDisable()
{
 // Unsubscribe to avoid memory leaks or double-calls
 KeyboardPluginCaller.Unregister(OnKeyboardStateChanged);
}

// The callback signature is UnityAction<bool, int>:
//  - bool:  true = keyboard shown, false = hidden
//  - int:   keyboard height (in pixels), or 0 if hidden
private void OnKeyboardStateChanged(bool isVisible, int height)
{
 if (isVisible)
 {
     Debug.Log($"Keyboard shown with height: {height}");
     // Adjust your UI accordingly
 }
 else
 {
     Debug.Log("Keyboard hidden");
     // Restore UI if needed
 }
}

```
> **Tip**: You can place this code in any MonoBehaviour that needs to react to keyboard changes. 

### 4. Adjusting UI or Layouts
When the keyboard is shown, you typically move certain UI elements above the keyboard or resize them to remain visible.
Use the height parameter to calculate offsets in screen pixels.
If you’re using RectTransform-based UI, you might need to convert screen pixels to anchored positions or adapt your layout group.
### 5. Logging and Debugging
The plugin has a method setLogEnabled(bool enabled) (or similar) that can be called via reflection or directly in the plugin code.
This controls whether the plugin prints detailed logs (Log.d(...), etc.) in Logcat on Android.
### 6. iOS Considerations (Optional)
On iOS, you may rely on TouchScreenKeyboard.area.height from Unity’s built-in API, or create a separate iOS native plugin.
Currently, this plugin focuses on Android. If you need iOS support, you can adapt or fork the repository.
Contributing
Fork the repo
Create a new branch (feat/my-feature or fix/bug-xyz)
Commit changes with clear messages
Submit a pull request describing your changes
License
This project is licensed under the MIT License. See the LICENSE file for details.

Support
If you encounter issues or have suggestions, feel free to open an Issue on GitHub or contact the maintainer.

Thank you for using UnitySoftKeyboardStateHandler!
We hope this plugin makes it easier to manage Android soft keyboard interactions in your Unity projects. If you find it useful, please give the repository a star on GitHub!
