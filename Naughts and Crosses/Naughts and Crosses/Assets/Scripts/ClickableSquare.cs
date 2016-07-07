using UnityEngine;
using System.Collections;

public class ClickableSquare : MonoBehaviour {

    public int squareNum = 0;

    void OnMouseDown()
    {
        GameObject.Find("Game Manager").SendMessage("SquareClicked", gameObject);
        Destroy(this);
    }

}
