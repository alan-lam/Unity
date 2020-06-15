# Unity

## Text101

### Update Text Component
Use `[SerializeField]` to access Game Object from script (AdventureGame.cs)

1. Create new GameObject (named Game)
2. Add script (AdventureGame.cs) as component

```
using System.Collections;  
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdventureGame : MonoBehaviour
{
    [SerializeField] Text textComponent;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = "Hi";
    }
}
```

### Scriptable Objects
`[CreateAssetMenu(menuName = "State")]` allows us to create a ScriptableObject in Unity (right click in Assets area -> Create -> State)

### Managing States
1. Create ScriptableObject called StartingState
2. In AdventureGame.cs, add `[SerializeField] State startingState;`
3. In Unity, add StartingState to field (in Game object)
4. In State.cs, add `[SerializeField] State[] nextStates;`
5. In Unity, add next states to StartingState's Next States field

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State")]
public class State : ScriptableObject
{
    [TextArea(10,14)] [SerializeField] string storyText;
    [SerializeField] State[] nextStates;

    public string GetStateStory()
    {
        return storyText;
    }

    public State[] GetNextStates()
    {
        return nextStates;
    }
}
```

## Number Wizard UI

### UI Anchors
Elements keep relative distance to anchor point when screen is resized
- if element is anchored to top-right corner of screen, then it will stay in the top-right corner when the screen is resized
- the spacing between the element and the anchor point stays the same when the screen is resized

### Loading Scenes with Button Clicks
1. Create GameObject called Scene Loader
2. Add SceneLoader.cs as component
3. In Unity, File -> Build Settings and drag scenes
4. Add Scene Loader to Button's On Click() and select LoadNextScene()

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene(0);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
```
