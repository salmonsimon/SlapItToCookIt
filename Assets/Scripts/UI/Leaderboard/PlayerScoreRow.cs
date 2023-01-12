using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreRow : MonoBehaviour
{
    [SerializeField] private Image image;

    [SerializeField] private Text rankingText;
    public Text RankingText { get { return rankingText; } set { rankingText = value; } }

    [SerializeField] private Text nameText;
    public Text NameText { get { return nameText; } set { nameText = value; } }

    [SerializeField] private Text scoreText;
    public Text ScoreText { get { return scoreText; } set { scoreText = value; } }

    public void HighlightRow() 
    {
        image.color = Color.yellow;
    }
}
