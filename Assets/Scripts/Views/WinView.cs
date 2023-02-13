using UnityEngine;
using TMPro;
using Zenject;
using UnityEngine.UI;
using System;
using ViewModels;

namespace Views
{
    public class WinView : MonoBehaviour
    {
        private WinViewModel WinViewModel { get; set; }

        [SerializeField] private GameObject WinPanel;

        [Space(10)]
        [SerializeField] private StarView starCentre;
        [SerializeField] private StarView starLeft;
        [SerializeField] private StarView starRight;

        [Space(10)]
        [SerializeField] private TextMeshProUGUI timer;
        [SerializeField] private TextMeshProUGUI bestScore;

        [Space(10)]
        [SerializeField] private TextMeshProUGUI enemiesDestroyed;
        [SerializeField] private TextMeshProUGUI defensesDestroyed;

        [Space(10)]
        [SerializeField] private Button NextLevelBtn;
        [SerializeField] private Button MenuBtn;

        [Inject]
        private void Construct(WinViewModel winViewModel)
        {
            WinViewModel = winViewModel;
        }

        private void Start()
        {
            WinViewModel.OnShowWinScreen += ShowWinScreen;
            WinViewModel.OnCloseWinScreen += CloseWinScreen;
            NextLevelBtn.onClick.AddListener(WinViewModel.NextLevel);
            MenuBtn.onClick.AddListener(WinViewModel.Menu);
        }

        public event Action OnNextLevelClick;

        private void ShowWinScreen(int stars)
        {
            WinPanel.SetActive(true);
            UpdateData(stars);
        }

        private void CloseWinScreen()
        {
            Reset();
            WinPanel.SetActive(false);
        }

        private void UpdateData(int stars)
        {
            UpdateStars(stars);
            UpdateTimer();
            UpdateBestScore();
            UpdateEnemiesKilled();
            UpdateDefensesDestroyed();
        }

        private void UpdateStars(int stars)
        {
            Reset();
            starCentre.Activate();
            if (stars > 1)
                starLeft.Activate();
            if (stars == 3)
                starRight.Activate();
        }

        private void UpdateTimer() => timer.text = WinViewModel.GetTimer();
        private void UpdateBestScore() => bestScore.text = WinViewModel.GetBestScore();
        private void UpdateEnemiesKilled() => enemiesDestroyed.text = WinViewModel.GetEnemiesKilled();
        private void UpdateDefensesDestroyed() => defensesDestroyed.text = WinViewModel.GetDefensesDestroyed();

        private void Reset()
        {
            starCentre.Reset();
            starLeft.Reset();
            starRight.Reset();
        }

        private void OnDestroy()
        {
            WinViewModel.OnShowWinScreen -= ShowWinScreen;
        }
    }
}