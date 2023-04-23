using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    void Start()
    {
        GameController.instance = this;
    }

    void InstanceLose()
    {
        Debug.Log("You clicked a mine and lost");
    }

    public static void Lose()
    {
        instance.InstanceLose();
    }

    void InstanceWin()
    {
        Debug.Log("You flagged all the mines");
    }

    public static void Win()
    {
        instance.InstanceWin();
    }
}
