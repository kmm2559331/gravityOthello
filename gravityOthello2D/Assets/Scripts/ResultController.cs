using TMPro;
using UnityEngine;

public class ResultController : MonoBehaviour
{
    private int BlackCount = 0;
    private int WhiteCount = 0;

    [SerializeField] private TextMeshProUGUI Winner;
    [SerializeField] private TextMeshProUGUI Black;
    [SerializeField] private TextMeshProUGUI White;
    void Start()
    {
        BlackCount = StaticData.StaticBlackCount;
        WhiteCount = StaticData.StaticWhiteCount;

        Black.text = BlackCount.ToString();
        White.text = WhiteCount.ToString();

        if (BlackCount > WhiteCount)       //•‚ÌŸ‚¿
        {
            Winner.text = "Winner:Black";
            Winner.color = Color.black;
        }
        else if (BlackCount < WhiteCount)  //”’‚ÌŸ‚¿ 
        {
            Winner.text = "Winner:White";
            Winner.color = Color.white;
        }
        else if (BlackCount == WhiteCount) //ˆø‚«•ª‚¯
        {
            Winner.text = "Draw";
        }
    }
}
