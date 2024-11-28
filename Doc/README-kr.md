# CategorizedDB
CategorizedDB는 유니티에서 사용 가능한 데이터 저장소입니다. 해당 데이터베이스의 가장 큰 특징은 각각의 데이터를 윈도우나 여타 운영체제들과 비슷하게 폴더 구조로 관리하며 데이터를 손쉽게 선택하고 이동할 수 있도록 설계되었다는 점 입니다.
또한 데이터 구성 요소를 만들고 해당 구성요소를 사용하는 데이터 베이스를 제작하는 일련의 과정들을 간소화 시키는 것을 통해 유니티를 활용한 컨텐츠 개발을 더욱 손쉽게 만들어줍니다.

---
## Features
- 간단한 데이터 클래스, DB, EditorWindow, Selector 구현 지원
- GUID 기반 분류를 통한 데이터 무결성
## 목차
- [CategorizedDB](#categorizeddb)
  - [Features](#features)
  - [목차](#목차)
  - [Simple Usage](#simple-usage)
  - [Getting Started](#getting-started)
    - [Example Category Element](#example-category-element)
    - [Example DB](#example-db)
    - [Example EditorWindow](#example-editorwindow)
    - [Example Selector Attribute](#example-selector-attribute)
    - [Example Usage](#example-usage)
      - [데이터 생성 방법](#데이터-생성-방법)
      - [데이터가 여럿일 경우](#데이터가-여럿일-경우)
      - [데이터의 수정](#데이터의-수정)
      - [데이터의 선택](#데이터의-선택)
  - [설치 방법](#설치-방법)
    - [Git URL](#git-url)
    - [마무리](#마무리)
## Simple Usage
- `CategoryElement`를 상속하여 카테고리 기반의 데이터 클래스를 생성
- `GenericCategorizedDB<T>`를 위에서 구현한 `Elememt` 데이터 클래스를 제너릭으로 상속하여 DB 클래스를 생성
- `CategoryDBEditor<T>`을 상속받아 `EditorWindow` 생성
  - `OdinInspector` 지원(`OdinInspector` 사용시 `OdinCategorizeDBEditor<T>`를 상속)
- `CategoryElementSelectorAttribute`를 상속받는 `Attribute`를 구현하여 에디터 상에서 손쉽게 데이터를 탐색
## Getting Started
[유니티 패키지 매니저](#설치-방법)를 통한 설치가 가능합니다. 아래 코드들은 데이터베이스를 실제 사용할 때의 예시입니다.

### Example Category Element
``` c#
using Postive.CategorizedDB.Runtime.Categories;
using UnityEngine;
namespace Runtime
{
    // Inherit from CategoryElement
    public class TestData : CategoryElement
    {
        // Test data for the element
        [SerializeField] private string _testContent = "Empty";
        [SerializeField] private int _testInt = 0;
        [SerializeField] private float _testFloat = 0.0f;
    }
}
```
- `CategoryElement`를 상속받는 데이터 클래스를 생성합니다.
- 기본적인 사용법은 유니티의 `ScriptableObject`와 유사합니다.
### Example DB
``` c#
using Postive.CategorizedDB.Runtime.Categories;
using UnityEngine;
namespace Runtime
{
    // This class is a simple data class that inherits from CategoryElement
    // Create an asset menu for this class
    [CreateAssetMenu(fileName = "TestDB", menuName = "TestDB")]
    // Input the element class into the generic categorised database
    public class TestDB : GenericCategorisedDB<TestData> { }
}
```
- `GenericCategorisedDB<T>`를 상속받는 데이터베이스 클래스를 생성합니다.
- 이때 `T`는 위에서 생성한 데이터 클래스를 입력합니다.
- `DB`의 경우 어떤 경로로든 접근할 방법이 필요합니다. 해당 패키지는 그 부분에 대한 부분은 사용자의 자유도를 위해 제공하지 않습니다. 때문에 사용자가 직접 데이터 접근 경로를 구축해야 합니다.
- ex) `Resources.Load<TestDB>("TestDB")`, `Singleton` 등등
### Example EditorWindow
``` c#
using Postive.CategorizedDB.Editor.CustomEditors.Native.CategorizedDBEditor;
using Postive.CategorizedDB.Runtime.Categories;
using Runtime;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
namespace Editor
{
    // The NativeTestDBEditor class for the TestDB database
    // Inherits from CategorizeDBEditor and uses the TestData class as the element type
    public class NativeTestDBEditor : CategorizeDBEditor<TestData>
    {
        // You must override CurrentDB to return the current database
        protected override CategorisedElementDB CurrentDB => Resources.Load<TestDB>("TestDB");
        // You need to create a way to open the editor window
        // For Example, Use [OnOpenAsset] to open the editor window when the asset is selected
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line) {
            if(Selection.activeObject is TestDB) {
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

``` c#
using Postive.CategorizedDB.Editor.CustomEditors.Odin.CategorizedDBEditor;
using Postive.CategorizedDB.Runtime.Categories;
using Runtime;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
namespace Editor
{
    // The TestOdinDBEditor class for the TestDB database
    // Inherits from OdinCategorizeDBEditor and uses the TestData class as the element type
    public class TestOdinDBEditor : OdinCategorizeDBEditor<TestData>
    {
        // You must override CurrentDB to return the current database
        protected override GenericCategorisedDB<TestData> CurrentDB => Resources.Load<TestDB>("TestDB");
        // You need to create a way to open the editor window
        // For Example, Use [OnOpenAsset] to open the editor window when the asset is selected
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line) {
            if(Selection.activeObject is TestDB) {
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

- `CategorizeDBEditor<T>`를 상속받는 에디터 윈도우 클래스를 생성합니다.
- 이때 `T`는 위에서 생성한 데이터 클래스를 입력합니다.
- `CurrentDB` 메소드를 오버라이드하여 현재 데이터베이스를 반환합니다.
- 이후 다양한 방법으로 에디터 윈도우를 열 수 있는 경로를 설정합니다.
- 예시에서는 [OnOpenAsset]를 사용하여 에셋이 선택되었을 때 에디터 윈도우를 열도록 설정하였습니다.
- OdinInspector를 사용할 경우 `OdinCategorizeDBEditor<T>`를 상속받아야 합니다.

### Example Selector Attribute
``` c#
using Postive.CategorizedDB.Runtime.Categories.Interfaces;
using UnityEngine;
namespace Runtime.Attributes
{
    // The TestDBSelectorAttribute class for the TestDB database
    public class TestDBSelectorAttribute : CategoryElementSelectorAttribute
    {
        // You must override ElementFinder to return the current database
        public override ICategoryElementFinder ElementFinder => Resources.Load<TestDB>("TestDB");
    }
}
```
- `CategoryElementSelectorAttribute`를 상속받는 `Attribute` 클래스를 생성합니다.
- `ElementFinder` 메소드를 오버라이드하여 현재 데이터베이스를 반환합니다.
- 이후 해당 `Attribute`를 사용한 string 변수는 데이터베이스의 데이터를 선택할 수 있는 에디터 상의 드롭다운을 생성합니다.
### Example Usage
![image](https://github.com/user-attachments/assets/39978c7a-a8fe-4352-b62d-9643aa4b5299)
데이터베이스를 생성하고 해당 데이터베이스를 활용해 데이터를 열어보면 위와 같은 에디터 창이 나타납니다.

좌측에 위치한 것이 데이터베이스의 데이터들이며 트리뷰로 나타납니다. 

우측에 위치한 것이 데이터의 세부 정보를 표시하며 수정 가능한 인스펙터입니다.
#### 데이터 생성 방법

![image](https://github.com/user-attachments/assets/61dd1909-0ade-461b-9f89-4c4458135662)

![image](https://github.com/user-attachments/assets/1b6711ac-693b-46b1-89ec-a963ff3e2791)

데이터 베이스의 좌측 트리뷰에 우클릭 할 경우 메뉴가 나타납니다. 해당 메뉴에서 `Category/Element`를 선택하면 데이터를 생성할 수 있습니다.

메뉴 상단에는 현재 선택되어진 데이터베이스의 범례를 표시해줍니다. 카테고리를 선택할 경우 해당 카테고리의 이름이 표시되며 아무 카테고리도 선택되어 있지 않을 경우 Root로 표시되고 이후 생성되는 데이터들의 경우 Root 카테고리에 속하게 됩니다.

이 상태에서 데이터나 카테고리를 생성할 경우 해당 데이터나 카테고리는 현재 선택되어진 카테고리에 포함되게 됩니다.

특정 카테고리를 선택한 이후 다시 Root 카테고리를 선택하고 싶을 때는 TreeView를 두 번 클릭하면 Root 카테고리로 돌아올 수 있습니다.

#### 데이터가 여럿일 경우
![image](https://github.com/user-attachments/assets/1e0a5ed3-bcee-43ab-bc11-098b96a13c90)

데이터가 여러 개일 경우 `Add Element` 메뉴에 하위 메뉴들이 추가됩니다. 이 하위 메뉴들은 데이터베이스에 제너릭으로 입력된 데이터 클래스를 상속받는 추상클래스가 아닌 클래스들입니다.

이 하위 메뉴들은 해당 클래스의 이름의 명칭을 이용하여 생성되며 해당 메뉴를 선택할 경우 해당 클래스의 데이터가 생성됩니다.

#### 데이터의 수정

기본적인 데이터 수정 방법은 유니티의 `SerializeObject`를 인스펙터에서 수정하는 방법과 동일합니다.

인스펙터의 상단에는 현재 선택되어 있는 부모 카테고리가 표시되며 해당 카테고리는 언제든지 변경이 가능합니다. 카테고리를 변경한다고 하더라도 해당 데이터를 참조가 훼손되지 않기 때문에 안전하게 변경이 가능합니다.

`Path` 변수는 현재 데이터의 경로를 표시해줍니다. 이 경로는 데이터베이스의 루트부터 해당 데이터까지의 경로를 나타냅니다.

`Name` 변수는 현재 데이터의 이름을 표시해줍니다. 만약 이름이 존재핮지 않을 경우 해당 데이터의 GUID가 포함된 대체 이름이 표기되며 이는 데이터의 무결성을 보장하기 위한 것입니다.

이후 Content 라벨 하단에 사용자가 추가적으로 생성한 변수들이 표시됩니다. 이 변수들은 데이터 클래스에 선언된 변수들이며 이 변수들을 통해 데이터를 수정할 수 있습니다.

#### 데이터의 선택

기본적으로 string 변수에 직접 구현한 Selector Attribute를 사용할 경우 해당 변수를 클릭하면 데이터베이스의 데이터를 선택할 수 있는 드롭다운이 나타납니다.

![image](https://github.com/user-attachments/assets/649f0d55-95bb-4c50-a03f-84b56b7ebabc)

이때 string 변수에 저장되는 것은 해당 데이터의 GUID이며 GUID를 이용하여 데이터를 찾아올 수 있습니다. 아래는 데이터를 선택하는 예시입니다.

``` c#
using Runtime.Attributes;
using UnityEngine;
namespace Runtime
{
    public class TestScript : MonoBehaviour
    {
        // The TestDBSelector attribute for the TestDB database
        [TestDBSelector]
        [SerializeField] private string _testString;
        private void Awake() {
            // Load the database
            TestDB db = Resources.Load<TestDB>("TestDB");
            // Get the data from the database
            TestData data = db.Get(_testString);
        }
    }
}
```

위처럼 DB를 불러온 뒤 Get(string GUID) 함수를 활용하여 데이터를 불러올 수 있습니다. 해당 GUID에 해당하는 데이터가 없을 경우 null을 반환합니다.

## 설치 방법
### Git URL
유니티 2019.3.4f1 이전 버전에서틑 테스트 되지 않았습니다. 때문에 되도록이면 그 이상의 버전을 사용해주세요.
유니티 패키지 매니저에서 `https://github.com/Postive-ToolKit/CategorizedDB.git` URL을 이용하여 설치가 가능합니다.

![image](https://github.com/user-attachments/assets/c1a97d72-5be2-429f-89ac-0198418abf2d)
![image](https://github.com/user-attachments/assets/f5167944-ac9c-4efe-b48a-acd2da20df46)

---

### 마무리
위의 일련의 과정을 통해 데이터베이스를 생성하고 데이터를 관리하는 일련의 과정을 간소화 시킬 수 있습니다. 이를 통해 유니티를 활용한 컨텐츠 개발을 더욱 손쉽게 만들어줍니다.