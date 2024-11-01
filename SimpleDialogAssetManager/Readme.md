# SimpleDialogAssetManager - SDAM

The dialog system created by postive

## How to use

### 1. Create a dialog plot graph asset

1. Download the package from the release page and import it into your project.

2. Create a new dialog plot graph asset by right-clicking in the project window and selecting "Create -> Dialog System -> Dialog Plot Graph".

3. Double-click the asset to open the graph editor window.

4. Create a new node by right-clicking on the graph editor window and selecting "Dialog System -> Nodes -> Dialog".

5. Add data to the node.

### 2. Create a dialog set asset

1. The dialog set asset is a collection of dialog plot graph assets.

2. Create a new dialog set asset by right-clicking in the project window and selecting "Create -> Dialog System -> Dialog Set".
   1.1. Be careful, The dialog set asset must be in the "Resources/Dialogs" folder.

3. Set the name of the dialog set as scene name.
   2.1. It will be automatically loaded when scene is opened But If you don't set the name of the dialog set as scene name, the dialog set will not be loaded.

4. Add the dialog plot graph asset to the dialog set asset.

5. Set the dialog plot graph plot id.

6. Set the Start dialog plot graph id.
   5.1. You can use dialog set without start dialog plot graph id. But if you set the start dialog plot graph id, the dialog set will start with the start dialog plot.

It's done. You can use the dialog system now.

### When using with Unity Localization Package

Essential Conditions:

- You must install the Unity Localization package
- You must create a localization setting asset
- You must create at least one localization table

If you install the Unity Localization package, The SDAM will automatically replace the text area with the localized string.
Then you can choose the localization phrase from the localized string editor.
