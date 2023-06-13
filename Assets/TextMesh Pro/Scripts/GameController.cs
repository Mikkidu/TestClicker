using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private int _score;
    private delegate void OnScoreChange(int score);

    public void AddScore(int amount)
    {
        _score += amount;
    }

}
