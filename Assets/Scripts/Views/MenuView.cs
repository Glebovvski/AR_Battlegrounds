using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ViewModels;
using Zenject;

namespace Views
{
    public class MenuView : MonoBehaviour
    {
        private MenuViewModel MenuViewModel { get; set; }
        private AdManager AdManager { get; set; }
        private AudioManager AudioManager { get; set; }

        [SerializeField] private GameObject menuPanel;

        [SerializeField] private Button startBtn;
        [SerializeField] private Button buyBtn;
        [SerializeField] private Button donateBtn;
        [SerializeField] private Button noAdsBtn;
        [SerializeField] private Button changeModeBtn;
        [SerializeField] private TextMeshProUGUI modeText;

        [Inject]
        private void Construct(MenuViewModel menuViewModel, AdManager adManager, AudioManager audioManager)
        {
            MenuViewModel = menuViewModel;
            AdManager = adManager;
            AudioManager = audioManager;
        }

        private void Start()
        {
            MenuViewModel.OnOpen += Show;
            AdManager.OnCanShowAdValueChanged += SetNoAdsButtonVisibility;
            changeModeBtn.interactable = MenuViewModel.IsModeChangeButtonInteractable;
            if (!changeModeBtn.interactable)
                modeText.text = "Classic Mode";
        }

        public void Close()
        {
            AudioManager.PlayUI();
            menuPanel.SetActive(false);
            MenuViewModel.Close();
        }

        public void Show()
        {
            menuPanel.SetActive(true);
        }

        public void BuyCoins()
        {
            AudioManager.PlayUI();
            MenuViewModel.BuyCoins();
        }

        public void ChangeMode()
        {
            bool isAR = MenuViewModel.ChangeMode();
            if (isAR)
                modeText.text = "AR mode";
            else
                modeText.text = "Classic Mode";
        }

        public void Donation()
        {
            AudioManager.PlayUI();
            MenuViewModel.Donation();
        }

        public void NoAds()
        {
            AudioManager.PlayUI();
            MenuViewModel.NoAds();
        }

        private void SetNoAdsButtonVisibility()
        {
            noAdsBtn.gameObject.SetActive(AdManager.CanShowAd);
        }

        private void OnDestroy()
        {
            MenuViewModel.OnOpen -= Show;
        }
    }
}