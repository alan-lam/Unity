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

    GameStatus theGameStatus;
    Ball theBall;

    private void Start()
    {
        theGameStatus = FindObjectOfType<GameStatus>();
        theBall = FindObjectOfType<Ball>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 paddlePos = new Vector2(transform.position.x, transform.position.y);
        paddlePos.x = Mathf.Clamp(GetXPos(), minX, maxX);
        transform.position = paddlePos;
    }

    private float GetXPos()
    {
        if (theGameStatus.IsAutoPlayEnabled())
        {
            return theBall.transform.position.x;
        }
        else
        {
            return Input.mousePosition.x / Screen.width * screenWidthInUnits;
        }
    }
}
