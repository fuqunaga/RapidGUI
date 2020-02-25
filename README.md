# RapidGUI
[![openupm](https://img.shields.io/npm/v/ga.fuquna.rapidgui?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/ga.fuquna.rapidgui/)

Unity IMGUI extensions for Rapid prototyping/development.

![rapidgui](Documentation~/rapidgui.png)

## Installation

### Install via OpenUPM

The package is available on the [openupm registry](https://openupm.com). It's recommended to install it via [openupm-cli](https://github.com/openupm/openupm-cli).

```
openupm add ga.fuquna.rapidgui
```

### Install via git URL

Add following lines to the `dependencies` section of the `Packages/manifest.json`.
```
"ga.fuquna.rapidgui": "https://github.com/fuqunaga/RapidGUI.git"
```

### Install via traditional `.unitypackage`

Download a `.unitypackage` file from the [Release page](https://github.com/fuqunaga/RapidGUI/releases).

## Getting Started
- Open and checkout the **RapidGUI/Example/RapidGUIExample.unity**
- see also the usage from the script below [**RapidGUI/Example/Scripts/**](Example/Scripts/)

## Functions
### RGUI.Field()
![field](Documentation~/field.gif)

```csharp
value = RGUI.Field(value, label);
```

- Display standard GUI according to type of value
- Right-drag label to edit numbers
- Color picker
- Array/List has a right-click menu like inspector
- Supports custom class

**CustomClass**

![fieldCustomClass](Documentation~/FieldCustomClass.png)

```csharp
public class CustomClass
{
    public int publicField;

    [SerializeField]
    protected int serializeField;

    [NonSerialized]
    public int nonSerializedField;

    [Range(0f, 10f)]
    public float rangeVal;

    public string longNameFieldWillBeMultiLine;
}

```
```csharp
customClass = RGUI.Field(customClass, nameof(customClass));
```


### RGUI.Slider()
![Slider](Documentation~/Slider.png)
```csharp
value = RGUI.Slider(value, min, max, label);
```
- Display slider GUI according to type of numbers


### RGUI.MinMaxSlider()
![MinMaxSlider](Documentation~/MinMaxSlider.png)
```csharp
RGUI.MinMaxSlider(minMaxVal, minMaxRange, label);
RGUI.MinMaxSlider(ref floatMin, ref floatMax, rangeMin, rangeMax, label);
```
- Display min max slider GUI according to type of numbers
- RapidGUI defines some basic MinMax type(`MinMaxInt`,`MinMaxFloat`,`MinMaxVector2`...)
- You can also create your own MinMax type by inheriting `MinMax<T>`


### RGUI.SelectionPopup()
![fold](Documentation~/selectionPopup.gif)
```csharp
selectionPopupIdx = RGUI.SelectionPopup(selectionPopupIdx, new[] { "One", "Two", "Three" });
selectionPopupStr = RGUI.SelectionPopup(selectionPopupStr, new[] { "One", "Two", "Three" });
```

### RapidGUI.Fold / Folds
![fold](Documentation~/fold.gif)

```csharp
// Initialize
fold = new Fold("Fold");
fold.Add(() => GUILayout.Label("Added function"));
```

```csharp
fold.DoGUI();
```

### RapidGUI.WindowLauncher / WindowLaunchers
![windowLauncher](Documentation~/windowLauncher.gif)
```csharp
// Initialize
launcher = new WindowLauncher("WindowLauncher");
launcher.Add(() => GUILayout.Label("Added function"));
```

```csharp
launcher.DoGUI();
```
- Toggle open/close window
- Resizable
- Has a close button

![windowLaunchers](Documentation~/windowLaunchers.gif)
- WindowLaunchers automatically adjusts the layout when opening a window

### And more!!!
Please check the usage from the script below [**RapidGUI/Example/Scripts/**](Example/Scripts/)

## Tips
### A "RapidGUI" object appears in the hierarchy
![RapidGUIBehaviour](Documentation~/RapidGUIBehaviour.png)
the object is a RapidGUI settings and update hooks.
If not in the scene, it will be generated automatically.

## Save/Load parameters
See [PrefsGUI](https://github.com/fuqunaga/PrefsGUI)


## Reference
- **unity-immediate-color-picker**
https://github.com/mattatz/unity-immediate-color-picker
- **PrefsGUI**
https://github.com/fuqunaga/PrefsGUI
