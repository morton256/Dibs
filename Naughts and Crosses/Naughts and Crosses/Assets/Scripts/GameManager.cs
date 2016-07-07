using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject Naught;
    public GameObject Cross;

    int turn = 1; // 1 = 0, 2 = X
    int winner = 0;
    int click = 0;

    //Array of Squares
    int[] squares = new int[9];

	public void SquareClicked(GameObject square)
    {
        int squareNum = square.GetComponent<ClickableSquare>().squareNum;
        print(squareNum);

        //Create Naught/Cross
        squares[squareNum] = turn;
        SpawnPrefab(square.transform.position);
        CheckForWinner();
        NextTurn();
        click++;
    }

    void CheckForWinner()
    {
        for(int player = 1; player <= 2; player++)
        {
            if (squares[0] == player && squares[1] == player && squares[2] == player)
            {
                DisableSquares();
                print(player + " wins");
                winner = player;
            }
            else if (squares[3] == player && squares[4] == player && squares[5] == player)
            {
                DisableSquares();
                print(player + " wins");
                winner = player;
            }
            else if (squares[6] == player && squares[7] == player && squares[8] == player)
            {
                DisableSquares();
                print(player + " wins");
                winner = player;
            }


            else if (squares[0] == player && squares[3] == player && squares[6] == player)
            {
                DisableSquares();
                print(player + " wins");
                winner = player;
            }
            else if (squares[1] == player && squares[4] == player && squares[7] == player)
            {
                DisableSquares();
                print(player + " wins");
                winner = player;
            }
            else if (squares[2] == player && squares[5] == player && squares[8] == player)
            {
                DisableSquares();
                print(player + " wins");
                winner = player;
            }

            else if (squares[0] == player && squares[4] == player && squares[8] == player)
            {
                DisableSquares();
                print(player + " wins");
                winner = player;
            }
            else if (squares[6] == player && squares[4] == player && squares[2] == player)
            {
                DisableSquares();
                print(player + " wins");
                winner = player;
            }
        }

        if (click == 8 && winner == 0)
        {
            winner = 3;
        }
        
    }

    void DisableSquares()
    {
        foreach(ClickableSquare square in GameObject.FindObjectsOfType<ClickableSquare>())
        {
            Destroy(square);
        }
    }
    void SpawnPrefab(Vector3 pos)
    {
        pos.z = 0;
        if (turn == 1)
            Instantiate(Naught, pos, Quaternion.identity);
        else if (turn == 2)
            Instantiate(Cross, pos, Quaternion.identity);
    }

    void NextTurn()
    {
        turn += 1;
        if (turn == 3)
        {
            turn = 1;
        }
    }

    void OnGUI()
    {
        if(winner == 1)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), "Naught is Winner");
        }
        else if(winner == 2)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), "Cross is Winner");
        }
        else if (winner == 3)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), "Draw!");
        }
        
        if(winner != 0)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 + 25, 100, 50), "Restart"))
            {
                Application.LoadLevel(0);   
            }
        }
    }
}
