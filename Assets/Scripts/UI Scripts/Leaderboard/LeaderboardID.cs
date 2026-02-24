using TMPro;
using UnityEngine;

namespace UI_Scripts.Leaderboard
{
    public class LeaderboardID : MonoBehaviour
    {
        [SerializeField] private TMP_Text _score;
        [SerializeField] private TMP_Text _int;
        
        public void SetBoard(int value, int score)
        {
            _score.text = NumberFormatter.FormatValue(score);
            _int.text = value.ToString();
        }
    }
}