using UnityEngine;


public enum StoneColor
{
    Black,
    White
}

public class ChangeColor : MonoBehaviour
{
    private Renderer a;

    StoneState _thisState;

    public Material Black;
    public Material White;

    private void Awake()
    {
        a = GetComponent<Renderer>();
    }

    private void FixedUpdate()
    {
        //_ChangeColor();
    }

    public void _ChangeColor(StoneState _turn)
    {
        if (_turn == StoneState.Black)
        {
            a.material.color = Color.black;
        }
        else if (_turn == StoneState.White)
        {
            a.material.color = Color.white;
        }
        else 
        {
            return;
        }
    }
}
