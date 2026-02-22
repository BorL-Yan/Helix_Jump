using System;
using VContainer.Unity;

namespace Level.Controller
{
    public class LevelController : IStartable , IDisposable
    {
        
        private readonly LevelAction _levelAction;
        private readonly LoosPanel _loosPanel;
        private readonly LevelProgress _levelProgress;
        private readonly LeaderBoard _leaderBoard;
        private readonly BestPrize _bestPrize;
        private readonly FinalPanel _finalPanel;
        private int _multiplyCount;

        public LevelController(LevelAction levelAction, LoosPanel loosPanel, LevelProgress levelProgress, LeaderBoard leaderBoard, BestPrize bestPrize, FinalPanel finalPanel)
        {
            _levelAction = levelAction;
            _loosPanel = loosPanel;
            _levelProgress = levelProgress;
            _leaderBoard = leaderBoard;
            _bestPrize = bestPrize;
            _finalPanel = finalPanel;
            _multiplyCount = 2;
        }


        private void AddMultiplyCount()
        {
            _multiplyCount++;
        }

        private void ActivateMultiplyAnimation()
        {
            _levelProgress.ActivateAnimationPoint(_multiplyCount, () =>
            {
                ActivateBox();
            });
        }

        private void ActivateBox()
        {
            if (GameSave.GetSettings().Key >= 3)
            {
                //TODO Activate Leader Board
                _bestPrize.Activate(ActivateLeaderBoard);
                GameSave.GetSettings().Key = 0;
                GameSave.Save();
            }
            else
            {
                ActivateLeaderBoard();
            }
        }

        private void ActivateLeaderBoard()
        {
            _leaderBoard.Activate(ActivateFinalPanel);
            _leaderBoard.SetScore(_levelProgress.GetPoint());
        }

        private void ActivateFinalPanel()
        {
            _finalPanel.Activate(() =>
            {
                int finalLevel = GameSave.GetSettings().Level;
                if (finalLevel == GameManager.Instance.CurrentActiveLevel)
                {
                    finalLevel += 1;
                    GameManager.Instance.OpenNewLevel = true;
                }
                
                GameSave.GetSettings().Level = finalLevel;
                GameSave.Save();
                
                GameManager.Instance.ActivateMenuScene();
            });
        }

        private void Loose()
        {
            _loosPanel.SetActive(true);
        }
        
        public void Start()
        {
            _levelAction.AddMultiplyCount += AddMultiplyCount;
            _levelAction.OnActivateFinalPointAnimation += ActivateMultiplyAnimation;
            _levelAction.OnLoos += Loose;
        }

        public void Dispose()
        {
            _levelAction.AddMultiplyCount -= AddMultiplyCount;
            _levelAction.OnActivateFinalPointAnimation -= ActivateMultiplyAnimation;
            _levelAction.OnLoos -= Loose;
        }
    }
}