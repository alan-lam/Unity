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
- If element is anchored to top-right corner of screen, then it will stay in the top-right corner when the screen is resized
- The spacing between the element and the anchor point stays the same when the screen is resized

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

## Block Breaker

### Making Objects Move
Objects with a Rigidbody component are affected by physics
- Dynamic: objects are affected by gravity and forces (ball)
- Kinematic: objects are moved by user (paddle)

### Making Objects Solid
Add a Collider 2D component

### Making Objects Bounce
1. Add a Physics Material 2D
2. Change Bounciness to 1
    - Higher numbers make it bounce higher
3. Add the material to the Circle Collider's Material

### Triggering Events
1. Create a GameObject and add a collider to it
2. Make Is Trigger true
    - Objects will pass through the collider
3. Create script and add it as a component

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene("Game Over");
    }
}
```

### Moving Objects With Mouse
Moving the paddle:

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    /* the camera size is 6, so the total height is 12
     * the aspect ratio is 4:3, which makes the total width 16
     */
    [SerializeField] float screenWidthInUnits = 16f;

    /* paddle takes up 2 units (the paddle is 256x64 and the pixels per unit is 128)
     * paddle's position is based on it's pivot, which is center
     * so when the paddle is at position 1, the left edge of the paddle will be at position 0
     */
    [SerializeField] float minX = 1;
    [SerializeField] float maxX = 15;

    // Update is called once per frame
    void Update()
    {
        float mousePosInUnits = Input.mousePosition.x / Screen.width * screenWidthInUnits;
        Vector2 paddlePos = new Vector2(transform.position.x, transform.position.y);
        paddlePos.x = Mathf.Clamp(mousePosInUnits, minX, maxX);
        transform.position = paddlePos;
    }
}
```

Moving the ball when the mouse is clicked:

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Paddle paddle1;
    [SerializeField] float xPush = 2f;
    [SerializeField] float yPush = 15f;

    Vector2 paddleToBallVector;
    bool hasStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        paddleToBallVector = transform.position - paddle1.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted)
        {
            LockBallToPaddle();
            LaunchBallOnMouseClick();
        }
    }

    private void LockBallToPaddle()
    {
        Vector2 paddlePos = new Vector2(paddle1.transform.position.x, paddle1.transform.position.y);
        transform.position = paddlePos + paddleToBallVector;
    }

    private void LaunchBallOnMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hasStarted = true;
            GetComponent<Rigidbody2D>().velocity = new Vector2(xPush, yPush);
        }
    }
}
```

### Destroying Objects

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
```

### Prefabs
A prefab is a template for creating objects

Create prefab by dragging GameObject into Assets

### Playing Audio On Object Collision

```
private void OnCollisionEnter2D(Collision2D collision)
{
    // to play a sound specified in Unity
    GetComponent<AudioSource>().Play();
    
    // to play a sound to the end (doesn't interrupt if another sound plays while current sound is playing)
    // GetComponent<AudioSource>().PlayOneShot(audioClip);
}
```

### Playing Audio When Object Is Destroyed
The above method works when the ball hits the block

However, when the block is destroyed, it doesn't exist anymore so it doesn't have any components (namely audio)

This method creates a temporary GameObject that plays the sound

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] AudioClip breakSound;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position);
        Destroy(gameObject);
    }
}
```

### Showing Sprites On Collision

```
public class Block : MonoBehaviour
{
    private void TriggerSparklesVFX()
    {
        GameObject sparkles = Instantiate(blockSparklesVFX, transform.position, transform.rotation);
        Destroy(sparkles, 1f);
    }
}
```

### Changing Sprites On Collision

```
public class Block : MonoBehaviour
{
    [SerializeField] Sprite[] hitSprites;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tag == "Breakable")
        {
            HandleHit();
        }
    }
    
    private void HandleHit()
    {
        timesHit++;
        if (timesHit >= maxHits)
        {
            DestroyBlock();
        }
        else
        {
            ShowNextHitSprite();
        }
    }
    
    private void ShowNextHitSprite()
    {
        int spriteIndex = timesHit - 1;
        GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
    }
}
```
