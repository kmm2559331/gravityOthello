using System.Collections;
using UnityEngine;


//public enum StoneColor
//{
//    Black,
//    White
//}

public class ChangeColor : MonoBehaviour
{
    //private Renderer a;
    //
    //StoneState _thisState;
    //
    //public Material Black;
    //public Material White;
    //
    //private void Awake()
    //{
    //    a = GetComponent<Renderer>();
    //}
    //
    //private void FixedUpdate()
    //{
    //    //_ChangeColor();
    //}

    private FieldGenerator FieldGenerator;
    private GameObject GameObject;

    private void Start()
    {
        GameObject = GameObject.Find("GameObject");
        FieldGenerator = GameObject.GetComponent<FieldGenerator>();
    }

    public void _ChangeColor(StoneState _turn)
    {
        if (_turn == StoneState.Black)
        {
            //a.material.color = Color.black;
            //transform.Rotate(0, 180, 0);
            StartCoroutine(MyRotate());
            
        }
        else if (_turn == StoneState.White)
        {
            //a.material.color = Color.white;
            //transform.Rotate(0, 180, 0);
            StartCoroutine(MyRotate());
            
        }
        else 
        {
            return;
        }
    }

    IEnumerator MyRotate()
    {
        while (FieldGenerator.isFall)
        {
            FieldGenerator.coroutine = true;
            yield return null;
        }
        for (int i = 0; i < 180; i += 2)
        {
            FieldGenerator.coroutine = true;
            transform.Rotate(0, 2, 0);
            yield return new WaitForSeconds(0.01f);
        }
        FieldGenerator.coroutine = false;
    }

    public void SetColor(StoneState _turn)
    {
        if (_turn == StoneState.White)
        {
            transform.Rotate(0, 180, 0);
        }
        else
        {
            return;
        }
    }
}
