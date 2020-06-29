# Unity

## Text101

### Update Text Component
Use `[SerializeField]` to access Game Object from script (AdventureGame.cs)

1. Create new GameObject (named Game)
2. Add script (AdventureGame.cs) as component

```
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

Change the class so that it inherits from `ScriptableObject` instead of `MonoBehaviour`

### Managing States
1. Create ScriptableObject called StartingState
2. In AdventureGame.cs, add `[SerializeField] State startingState;`
3. In Unity, add StartingState to field (in Game object)
4. In State.cs, add `[SerializeField] State[] nextStates;`
5. In Unity, add next states to StartingState's Next States field

```
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

### Loading Scenes With Button Clicks
1. Create GameObject called Scene Loader
2. Add SceneLoader.cs as component
3. In Unity, File -> Build Settings and drag scenes
4. Add Scene Loader to Button's On Click() and select LoadNextScene()

```
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

### Creating Fonts
1. Download from [dafont.com](https://dafont.com)
2. Window -> TextMeshPro -> Font Asset Creator

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
Moving the Paddle:

```
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

Moving the Ball When the Mouse Is Clicked:

```
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
using UnityEngine;

public class Block : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        // to destroy the object hitting the block
        // Destroy(collision.gameObject);
    }
}
```

### Prefabs
A prefab is a template for creating objects

Create prefab by dragging GameObject into Assets

### Playing Audio on Object Collision

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

### Showing Sprites on Collision

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

### Changing Sprites on Collision

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

## Laser Defender

Setup:
- Enemy Spawner GameObject has an Enemy Spawner (script) component
    - It has an array of WaveConfigs
- WaveConfigs have information about what type of Enemy spawns and what path they take (waypoints)
- Enemy GameObjects have an Enemy Pathing (script) component
    - Enemy Pathing has information about the current WaveConfig
    - Responsible for moving 1 Enemy GameObject

Execution:
- Enemy Spawner instantiates Enemy GameObjects and attaches the current WaveConfig to it's Enemy Pathing component
- Enemy Pathing uses the info from the WaveConfig and moves the Enemy GameObject

### Moving Objects With Keyboard

To see input options (e.g. `Input.GetAxis()`, `Input.GetButtonDown()`), Edit -> Project Settings

```
private void Move()
{
    // multiplying by Time.deltaTime makes movement same across all computers (frame-rate independent)
    var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
    var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

    var newXPos = transform.position.x + deltaX;
    var newYPos = transform.position.y + deltaY;
    transform.position = new Vector2(newXPos, newYPos);
}
```

Constraining Movement Within Camera:

```
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        // multiplying by Time.deltaTime makes movement same across all computers (frame-rate independent)
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }
}
```

### Shooting Laser Beams

```
private void Fire()
{
    if (Input.GetButtonDown("Fire1"))
    {
        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
    }
}
```

### Coroutines
Methods that can pause execution and continue at a later time

```
IEnumerator NameOfCoroutine()
{
    // code before pausing
    yield return new WaitForSeconds(3); // pause for 3 seconds
    // code after pausing
}

// to call the coroutine
StartCoroutine(NameOfCoroutine());
```

### Shooting Lasers Continuously

```
public class Player : MonoBehaviour
{
    [SerializeField] float projectileFiringPeriod = 0.1f;

    Coroutine firingCoroutine;
    
    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }
    
    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }
}
```

### Moving Objects Along a Path
Create GameObjects and use them to make a path

```
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    [SerializeField] List<Transform> waypoints;
    [SerializeField] float moveSpeed = 2f;
    int waypointIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = waypoints[waypointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (waypointIndex <= waypoints.Count - 1)
        {
            var targetPosition = waypoints[waypointIndex].transform.position;
            var movementThisFrame = moveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);
            if (transform.position == targetPosition)
            {
                waypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
```

### Layer Collision Matrix
Edit -> Project Settings -> Physics2D

Determines what layers can collide with which layers

### Create Scrolling Background
1. Change Sprite Texture Type to Default and Wrap Mode to Repeat
2. Create Quad (3D Object)
3. Drag background Sprite onto Quad
4. Change Quad's Shader to Unlit/Texture

```
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] float backgroundScrollSpeed = 0.2f;
    Material myMaterial;
    Vector2 offSet;

    // Start is called before the first frame update
    void Start()
    {
        myMaterial = GetComponent<Renderer>().material;
        offSet = new Vector2(0f, backgroundScrollSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        myMaterial.mainTextureOffset += offSet * Time.deltaTime;
    }
}
```

### Creating Particle Effects
1. Create Material
2. Change Albedo to Sprite
3. Change Shader to Mobile/Particles/Alpha Blended
4. Create Particle System (Right click in Hierarchy -> Effects -> Particle System)
5. Change Material under Renderer

### Singleton

```
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
```

## Glitch Garden

### Slicing Spritesheets
1. Change Sprite Mode to Multiple and click Apply
2. Go to Sprite Editor and click Slice

### Setting Up Animator Controller
1. Add Animator Component to GameObject
2. Create Animator Controller (Right click -> Create -> Animator Controller)
3. Select all frames (from sliced up spritesheet) and create Animation (Right click -> Create -> Animation)
4. Change speed of animation by changing number of samples (in Animation window)
    - Make it loop by checking Loop Time in Inspector
5. Drag Animation into Animation Controller (in Animator window)
    - Transition to another animation by right-clicking on transition and Make Transition
6. Assign GameObject's Animator Controller

1. Click on GameObject to animate
2. In Animation window, click Create
3. Click Add Property
4. Sprite Renderer -> Sprite
5. Drag all frames into Dopesheet

### Move Objects Using transform.Translate

```
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [Range(0f,5f)] [SerializeField] float walkSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * Time.deltaTime * walkSpeed);
    }
}
```

### Animation Events
Animation events call a method when the animation reaches a certain frame

### Creating GameObjects on Mouse Click

```
using UnityEngine;

public class DefenderSpawner : MonoBehaviour
{
    [SerializeField] GameObject defender;

    private void OnMouseDown()
    {
        SpawnDefender(GetSquareClicked());
    }

    private Vector2 GetSquareClicked()
    {
        Vector2 clickPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(clickPos);
        return worldPos;
    }

    private void SpawnDefender(Vector2 worldPos)
    {
        GameObject newDefender = Instantiate(defender, worldPos, Quaternion.identity) as GameObject;
    }
}
```

### Animation Parent-Child Setup
Have a Body GameObject as a Child of the main Parent GameObject

Put the Animator Controller on the Parent
- Allows us to manipulate the parent and all of its children

Put the Sprite Renderer on the Body

Animators can only access methods that are on the same level
- An Animator on the Parent can only access methods from the script on the Parent

### Instantiating Objects as a Child

```
childObject.transform.parent = transform;
```

### Changing Animation State
1. Add a Parameter in the Animator window
2. Click on transition line and add a Condition
3. Has Exit Time will play the entire animation until the end and then transition

```
animator.SetBool("NameOfParameter", true);
```
