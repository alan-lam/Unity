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
