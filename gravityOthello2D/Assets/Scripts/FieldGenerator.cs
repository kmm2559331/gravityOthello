using System.Drawing;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEditor.PlayerSettings;

public enum StoneState
{
    Black,
    White,
    None
}

public enum ChangeState
{
    True,
    False,
}

public class FieldGenerator : MonoBehaviour
{
    const int stoneMax = 8;//配列の上限（縦横）
    const int BoardMax = stoneMax * stoneMax;//ボードの上限

    //bool?[,] stone = new bool?[stoneMax, stoneMax];//false:黒 true:白 null:ブロックなし
    public StoneState[,] stone = new StoneState[stoneMax, stoneMax];
    public StoneState _turn = StoneState.Black;

    public GameObject[,] save = new GameObject[stoneMax, stoneMax];

    public ChangeState[,] change = new ChangeState[stoneMax, stoneMax];

    int waitStone;//上部に待機中の石

    Vector2 Linepos;//横線の座標（石の上）
    Vector2 Stonepos;

    private Vector2Int Retry = new Vector2Int(-1, -1);
    private Vector2Int Exit = new Vector2Int(-10, -10);

    private InputAction moveAction;

    [SerializeField] GameObject LineParent;//横線の親オブジェクト
    [SerializeField] GameObject LinePrefab;//横線のプレハブ
    [SerializeField] GameObject StoneParent;//石の親オブジェクト
    [SerializeField] GameObject StonePrefab;//石のプレハブ
    //[SerializeField] GameObject StoneBlackPrefab;//黒石のプレハブ
    //[SerializeField] GameObject StoneWihtePrefab;//白石のプレハブ
    [SerializeField] GameObject Arrow;//矢印

    private ChangeColor ChangeColor;

    private int endCount = 0;
    private int BlackCount = 0;
    private int WhiteCount = 0;

    [SerializeField] private GameObject BlackUI;            //ターン表示UI
    [SerializeField] private GameObject WhiteUI;            //ターン表示UI
    [SerializeField] private TextMeshProUGUI BlackCountText;//TextUI
    [SerializeField] private TextMeshProUGUI WhiteCountText;//TextUI

    private void Start()
    {
        for (int x = 0; x < stoneMax;x++)
        {
            for(int y = 0; y < stoneMax; y++)
            {
                stone[x, y] = StoneState.None;
            }
        }

        //ResetChangeState();

        //ChangeColor = StonePrefab.GetComponent<ChangeColor>();

        //stone[0,stoneMax-1] = false;
        //stone[stoneMax-1,stoneMax-1] = true;
        Vector3 BlackarrowPos;
        BlackarrowPos.x = 0.5f;
        BlackarrowPos.y = 1;
        BlackarrowPos.z = 5;

        Vector3 WihtearrowPos;
        WihtearrowPos.x = 7.5f;
        WihtearrowPos.y = 1;
        WihtearrowPos.z = 5;

        //ArrowTransform(BlackarrowPos);

        moveAction = InputSystem.actions["move"];

        FallStone(0, _turn);//左下に黒石を置く
        FallStone(stoneMax-1, _turn);//右下に白石を置く

        UISet();
    }


    public void FixedUpdate()
    {
        //if(moveAction. = ) //InputSystemで

        BlackCountText.text = BlackCount.ToString();
        WhiteCountText.text = WhiteCount.ToString();
    }
    public void FallStone(int x, StoneState turn)//石を落としたら（マウスクリック）
    {
        if (x < 0 || x > 7) return;
        for (int i = stoneMax-1; i >= 0; i--)
        {
            if (stone[x, i] == StoneState.None)
            {
                stone[x, i] = turn;
                Stonepos = new Vector2((float)(x + 0.5), -(float)(i+0.5));//0.5を足す
                //if (turn == StoneState.Black)
                //{
                //    Instantiate(StoneBlackPrefab, Stonepos, Quaternion.identity, StoneParent.transform);//石を置く
                //} else 
                //{
                //    Instantiate(StoneWihtePrefab, Stonepos, Quaternion.identity, StoneParent.transform);//石を置く
                //}
                //ChangeColor = Instantiate(StonePrefab, Stonepos, Quaternion.identity, StoneParent.transform).GetComponent<ChangeColor>();
                //ChangeColor._ChangeColor(turn);
                save[x,i] = Instantiate(StonePrefab, Stonepos, Quaternion.identity, StoneParent.transform);
                save[x, i].GetComponent<ChangeColor>()._ChangeColor(turn);
                if (i != 0)
                {
                    Linepos = new Vector2((float)(x + 0.5), -i);
                    Instantiate(LinePrefab, Linepos, Quaternion.identity, LineParent.transform);//石の上に線を引く
                }

                //ひっくり返す関数を呼ぶ
                TurnOver(x, i, turn);

                if (turn == StoneState.Black)
                {
                    _turn = StoneState.White;
                }
                else
                {
                    _turn = StoneState.Black;
                }

                if (BlackCount+WhiteCount == BoardMax-1)
                {
                    //ゲーム終了
                    EndGame();
                }

                //if (i == 0)
                //{
                //    endCount++;
                //    if (endCount == stoneMax - 1)
                //    {
                //        EndGame();
                //    }
                //}

                UISet();

                CountStone();
                break;
            }
        }
    }

    public void GenerateStone(StoneState turn)
    {
        waitStone = 0;//黒なら左(0)

        FallStone(waitStone, turn);//クリックしたら
    }

    public void TurnOver(int x, int y , StoneState turn)//効率化と隣のブロックチェック、for文の中にひっくり返す処理を書く
    {
        //for (int i = x-1;i >= 0; i--)                                                       {Vector2Int p = StoneCheck(i, y, turn);     if (p != Retry&&p!=Exit) {for (int j = x-1;j >= p.x; j--)    {TrunStone(j, y); } break; } else if(p==Exit) break;} //左
        //for (int i = x+1;i <= stoneMax-1; i++)                                              {Vector2Int p = StoneCheck(i, y, turn);     if (p != Retry&&p!=Exit) {for (int j = x+1;j <= p.x; j++)    {TrunStone(j, y); } break; } else if(p==Exit) break;} //右
        //for (int i = y-1;i >= 0; i--)                                                       {Vector2Int p = StoneCheck(x, i, turn);     if (p != Retry&&p!=Exit) {for (int j = y-1;j >= p.y; j--)    {TrunStone(j, y); } break; } else if(p==Exit) break;} //上
        //for (int i = y+1;i <= stoneMax-1; i++)                                              {Vector2Int p = StoneCheck(x, i, turn);     if (p != Retry&&p!=Exit) {for (int j = y+1;j <= p.y; j++)    {TrunStone(j, y); } break; } else if(p==Exit) break;} //下
        //for (Vector2Int i = new(x-1,y-1); i.x>=0 && i.y>=0; i.x--, i.y--)                   {Vector2Int p = StoneCheck(i.x, i.y, turn); if (p != Retry&&p!=Exit) {for (Vector2Int j = new(x-1,y-1); j.x>=p.x && j.y>=p.y; j.x--, j.y--){TrunStone(j.x, j.y); } break; }else if(p==Exit) break;} //左上
        //for (Vector2Int i = new(x+1,y-1); i.x<=stoneMax-1 && i.y>=0; i.x++, i.y--)          {Vector2Int p = StoneCheck(i.x, i.y, turn); if (p != Retry&&p!=Exit) {for (Vector2Int j = new(x+1,y-1); j.x<=p.x && j.y>=p.y; j.x++, j.y--){TrunStone(j.x, j.y); } break; }else if(p==Exit) break;} //右上
        //for (Vector2Int i = new(x-1,y+1); i.x>=0 && i.y<=stoneMax-1; i.x--, i.y++)          {Vector2Int p = StoneCheck(i.x, i.y, turn); if (p != Retry&&p!=Exit) {for (Vector2Int j = new(x-1,y+1); j.x>=p.x && i.y<=p.y; j.x--, j.y++){TrunStone(j.x, j.y); } break; }else if(p==Exit) break;} //左下
        //for (Vector2Int i = new(x+1,y+1); i.x<=stoneMax-1 && i.y<=stoneMax-1; i.x++, i.y++) {Vector2Int p = StoneCheck(i.x, i.y, turn); if (p != Retry&&p!=Exit) {for (Vector2Int j = new(x+1,y+1); j.x<=p.x && j.y<=p.y; j.x++, j.y++){TrunStone(j.x, j.y); } break; }else if(p==Exit) break;} //右下



        for (int i = x - 1; i >= 0; i--)//左
        {
            if (stone[i, y] != StoneState.None) //石があるなら
            {
                if (stone[i, y] != _turn)
                {
                    continue;
                }
                else if (stone[i, y] == _turn) //戻りながらひっくり返す
                {
                    if (i == x - 1) break;
                    for (int j = i; j <= x - 1; j++)
                    {
                        stone[j, y] = _turn;
                        save[j, y].GetComponent<ChangeColor>()._ChangeColor(_turn); // 色変更
                    }
                    break;
                }
            }
            else break;
        }
        for (int i = x + 1; i <= stoneMax - 1; i++)//右
        {
            if (stone[i, y] != StoneState.None) //石があるなら
            {
                if (stone[i, y] != _turn)
                {
                    continue;
                }
                else if(stone[i, y] == _turn) //戻りながらひっくり返す
                {
                    if (i == x + 1) break;
                    for (int j = i; j >= x + 1; j--)
                    {
                        stone[j, y] = _turn;
                        save[j, y].GetComponent<ChangeColor>()._ChangeColor(_turn); // 色変更
                    }
                    break;
                }
            }
            else break;
        }
        for (int i = y - 1; i >= 0; i--)//上
        {
            if (stone[x, i] != StoneState.None) //石があるなら
            {
                if (stone[x, i] != _turn)
                {
                    continue;
                }
                else if (stone[x, i] == _turn) //戻りながらひっくり返す
                {
                    if (i == y - 1) break;
                    for (int j = i; j <= y - 1; j++)
                    {
                        stone[x, j] = _turn;
                        save[x, j].GetComponent<ChangeColor>()._ChangeColor(_turn); // 色変更
                    }
                    break;
                }
            }
            else break;
        }
        for (int i = y + 1; i <= stoneMax - 1; i++)//下
        {
            if (stone[x, i] != StoneState.None) //石があるなら
            {
                if (stone[x, i] != _turn)
                {
                    continue;
                }
                else if (stone[x, i] == _turn) //戻りながらひっくり返す
                {
                    if (i == y + 1) break;
                    for (int j = i; j >= y + 1; j--)
                    {
                        stone[x, j] = _turn;
                        save[x, j].GetComponent<ChangeColor>()._ChangeColor(_turn); // 色変更
                    }
                    break;
                }
            }
            else break;
        }
        for (Vector2Int i = new(x - 1, y - 1); i.x >= 0 && i.y >= 0; i.x--, i.y--)//左上
        {
            if (stone[i.x, i.y] != StoneState.None) //石があるなら
            {
                if (stone[i.x, i.y] != _turn)
                {
                    continue;
                }
                else if (stone[i.x, i.y] == _turn) //戻りながらひっくり返す
                {
                    if (i.x == x - 1 && i.y == y - 1) break;
                    for (Vector2Int v = i; v.x <= x - 1 && v.y <= y - 1; v.x++,v.y++)
                    {
                        stone[v.x, v.y] = _turn;
                        save[v.x, v.y].GetComponent<ChangeColor>()._ChangeColor(_turn); // 色変更
                    }
                    break;
                }
            }
            else break;
        }                                                                         
        for (Vector2Int i = new(x + 1, y - 1); i.x <= stoneMax - 1 && i.y >= 0; i.x++, i.y--)//右上
        {
            if (stone[i.x, i.y] != StoneState.None) //石があるなら
            {
                if (stone[i.x, i.y] != _turn)
                {
                    continue;
                }
                else if (stone[i.x, i.y] == _turn) //戻りながらひっくり返す
                {
                    if(i.x == x + 1 && i.y == y - 1)break;
                    for (Vector2Int v = i; v.x >= x + 1 && v.y <= y - 1; v.x--, v.y++)
                    {
                        stone[v.x, v.y] = _turn;
                        save[v.x, v.y].GetComponent<ChangeColor>()._ChangeColor(_turn); // 色変更
                    }
                    break;
                }
            }
            else break;
        }
        for (Vector2Int i = new(x - 1, y + 1); i.x >= 0 && i.y <= stoneMax - 1; i.x--, i.y++)//左下
        {
            if (stone[i.x, i.y] != StoneState.None) //石があるなら
            {
                if (stone[i.x, i.y] != _turn)
                {
                    continue;
                }
                else if (stone[i.x, i.y] == _turn) //戻りながらひっくり返す
                {
                    if (i.x == x - 1 && i.y == y + 1) break;
                    for (Vector2Int v = i; v.x <= x - 1 && v.y >= y + 1; v.x++, v.y--)
                    {
                        stone[v.x, v.y] = _turn;
                        save[v.x, v.y].GetComponent<ChangeColor>()._ChangeColor(_turn); // 色変更
                    }
                    break;
                }
            }
            else break;
        }
        for (Vector2Int i = new(x + 1, y + 1); i.x <= stoneMax - 1 && i.y <= stoneMax - 1; i.x++, i.y++)//右下
        {
            if (stone[i.x, i.y] != StoneState.None) //石があるなら
            {
                if (stone[i.x, i.y] != _turn)
                {
                    continue;
                }
                else if (stone[i.x, i.y] == _turn) //戻りながらひっくり返す
                {
                    if (i.x == x + 1 && i.y == y + 1) break;
                    for (Vector2Int v= i; v.x >= x + 1 && v.y >= y + 1; v.x--, v.y--)
                    {
                        stone[v.x, v.y] = _turn;
                        save[v.x, v.y].GetComponent<ChangeColor>()._ChangeColor(_turn); // 色変更
                    }
                    break;
                }
            }
            else break;
        }
    }

    private void CountStone()
    {
        BlackCount = 0;
        WhiteCount = 0;
        for (int x = 0; x < stoneMax; x++)
        {
            for (int y = 0; y < stoneMax; y++)
            {
                if (stone[x, y] == StoneState.Black)
                {
                    BlackCount++;
                }
                else if (stone[x, y] == StoneState.White)
                {
                    WhiteCount++;
                }
            }
        }
    }

    private void EndGame()
    {
        CountStone();

        StaticData.StaticBlackCount = BlackCount;
        StaticData.StaticWhiteCount = WhiteCount;

        SceneManager.LoadScene("ResultScene");
    }
    public void UISet()
    {
        if (_turn == StoneState.Black)
        {
            BlackUI.SetActive(true);
            WhiteUI.SetActive(false);
        }
        else if (_turn == StoneState.White)
        {
            WhiteUI.SetActive(true);
            BlackUI.SetActive(false);
        }
    }
    //public Vector2Int StoneCheck(int x, int y, StoneState turn)//ブロックのチェック
    //{
    //    if (stone[x, y] != StoneState.None) //石があるなら
    //    { 
    //        if (stone[x, y] == turn) //同じ石があったら
    //        {
    //            change[x, y] = ChangeState.True;
    //            Vector2Int ret = new Vector2Int(x, y);
    //            return ret;
    //        }
    //
    //        if (stone[x, y] != turn)
    //        {
    //            return Retry;
    //        }
    //
    //    }
    //
    //    return Exit;
    //}

    //public void TrunStone(int x,int y)
    //{
    //    stone[x, y] = _turn;
    //    if (_turn == StoneState.Black)
    //    {
    //        //stone[x, y] //色変更
    //    }
    //}

    //public void ResetChangeState()
    //{
    //    for (int x = 0; x < stoneMax; x++)
    //    {
    //        for (int y = 0; y < stoneMax; y++)
    //        {
    //            change[x, y] = ChangeState.False;
    //        }
    //    }
    //}

    //public void ArrowTransform(Vector3 pos)
    //{
    //    GameObject a = Arrow;
    //    a.transform.position = pos;
    //}
}
