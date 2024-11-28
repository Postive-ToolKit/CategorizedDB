# CategorizedDB

CategorizedDB is a data storage system available for Unity. Its primary feature is the ability to manage data in a folder structure similar to Windows or other operating systems, making it easy to select and move data.

Additionally, it simplifies the process of creating data components and using them to build databases, streamlining Unity-based content development.
## Other Language
- Korean 🇰🇷 [한국어](Doc/README-kr.md)
---
## Features
- Support for simple implementation of data classes, databases, EditorWindows, and selectors
- Data integrity ensured through GUID-based categorization

## Table of Contents
- [CategorizedDB](#categorizeddb)
  - [Other Language](#other-language)
  - [Features](#features)
  - [Table of Contents](#table-of-contents)
  - [Simple Usage](#simple-usage)
  - [Getting Started](#getting-started)
    - [Example Category Element](#example-category-element)
    - [Example DB](#example-db)
    - [Example EditorWindow](#example-editorwindow)
    - [Example Selector Attribute](#example-selector-attribute)
    - [Example Usage](#example-usage)
      - [Creating Data](#creating-data)
      - [Handling Multiple Data](#handling-multiple-data)
      - [Editing Data](#editing-data)
      - [Selecting Data](#selecting-data)
  - [Installation](#installation)
    - [Git URL](#git-url)
  - [Conclusion](#conclusion)

## Simple Usage
1. Create a category-based data class by inheriting from `CategoryElement`.
2. Create a database class by inheriting from `GenericCategorizedDB<T>` and passing the previously created data class as the generic type.
3. Create an EditorWindow by inheriting from `CategoryDBEditor<T>`.
   - Supports OdinInspector (use `OdinCategorizedDBEditor<T>` when utilizing OdinInspector).
4. Implement an attribute by inheriting from `CategoryElementSelectorAttribute` for easy data exploration in the editor.

## Getting Started
The package can be installed through the [Unity Package Manager](#installation). Below are examples of how to use the database.

### Example Category Element
```csharp
using Postive.CategorizedDB.Runtime.Categories;
using UnityEngine;

namespace Runtime
{
    public class TestData : CategoryElement
    {
        [SerializeField] private string _testContent = "Empty";
        [SerializeField] private int _testInt = 0;
        [SerializeField] private float _testFloat = 0.0f;
    }
}
```
- Create a data class inheriting from `CategoryElement`.
- Its usage is similar to Unity's `ScriptableObject`.

### Example DB
```csharp
using Postive.CategorizedDB.Runtime.Categories;
using UnityEngine;

namespace Runtime
{
    [CreateAssetMenu(fileName = "TestDB", menuName = "TestDB")]
    public class TestDB : GenericCategorizedDB<TestData> { }
}
```
- Create a database class inheriting from `GenericCategorizedDB<T>`, where `T` is the data class created earlier.
- The package does not enforce a specific method to access the database. You can define your own access method, such as `Resources.Load<T>("Path")` or using singletons.

### Example EditorWindow
```csharp
using Postive.CategorizedDB.Editor.CustomEditors.Native.CategorizedDBEditor;
using Postive.CategorizedDB.Runtime.Categories;
using Runtime;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Editor
{
    public class NativeTestDBEditor : CategorizeDBEditor<TestData>
    {
        protected override CategorizedElementDB CurrentDB => Resources.Load<TestDB>("TestDB");

        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            if (Selection.activeObject is TestDB)
            {
                var window = GetWindow<NativeTestDBEditor>();
                window.titleContent = new GUIContent("TestDB Editor");
                return true;
            }
            return false;
        }
    }
}
```

<details><summary style="color: yellow;">if you use OdinInspector</summary>

```csharp
using Postive.CategorizedDB.Editor.CustomEditors.Odin.CategorizedDBEditor;
using Postive.CategorizedDB.Runtime.Categories;
using Runtime;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Editor
{
    public class TestOdinDBEditor : OdinCategorizedDBEditor<TestData>
    {
        protected override GenericCategorizedDB<TestData> CurrentDB => Resources.Load<TestDB>("TestDB");

        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            if (Selection.activeObject is TestDB)
            {
                var window = GetWindow<TestOdinDBEditor>();
                window.titleContent = new GUIContent("TestDB Editor");
                return true;
            }
            return false;
        }
    }
}
```
</details>

### Example Selector Attribute
```csharp
using Postive.CategorizedDB.Runtime.Categories.Interfaces;
using UnityEngine;

namespace Runtime.Attributes
{
    public class TestDBSelectorAttribute : CategoryElementSelectorAttribute
    {
        public override ICategoryElementFinder ElementFinder => Resources.Load<TestDB>("TestDB");
    }
}
```
- Create an attribute by inheriting from `CategoryElementSelectorAttribute`.
- Override `ElementFinder` to return the current database.
- When used with a `string` field, it generates a dropdown for selecting data in the editor.

### Example Usage

![image](https://github.com/user-attachments/assets/39978c7a-a8fe-4352-b62d-9643aa4b5299)

A generated database editor shows data as a tree view on the left and detailed properties on the right, allowing easy creation, modification, and selection of data.

#### Creating Data

![image](https://github.com/user-attachments/assets/61dd1909-0ade-461b-9f89-4c4458135662)

![image](https://github.com/user-attachments/assets/1b6711ac-693b-46b1-89ec-a963ff3e2791)

Right-click the tree view to access a menu for creating new categories or elements.

#### Handling Multiple Data
![image](https://github.com/user-attachments/assets/1e0a5ed3-bcee-43ab-bc11-098b96a13c90)

When the database has multiple data types, the "Add Element" menu dynamically populates with submenus based on the types.

#### Editing Data
Data can be edited in the inspector, similar to Unity's default `SerializedObject` workflow.

#### Selecting Data
Use a custom attribute to enable dropdown-based data selection in the editor. Selected data is referenced by its GUID.

![image](https://github.com/user-attachments/assets/649f0d55-95bb-4c50-a03f-84b56b7ebabc)

## Installation

### Git URL
Tested on Unity versions 2019.3.4f1 and later. Install the package via Unity Package Manager using the following URL:  
`https://github.com/Postive-ToolKit/CategorizedDB.git`


![image](https://github.com/user-attachments/assets/c1a97d72-5be2-429f-89ac-0198418abf2d)
![image](https://github.com/user-attachments/assets/f5167944-ac9c-4efe-b48a-acd2da20df46)

---

## Conclusion
The process outlined above simplifies database creation and management, enhancing efficiency in Unity-based content development.
