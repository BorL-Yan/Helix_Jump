using System;
using UnityEngine;

namespace Level
{
    public class ChampionsLeagueUIIcons : MonoBehaviour
    {
        [SerializeField] private GameObject _bronze;
        [SerializeField] private GameObject _sliver;


        private void OnEnable()
        {
            int id = GameSave.GetSettings().RankedID;

            _bronze.SetActive(id == 0);
            _sliver.SetActive(id == 1);
        }
    }
}