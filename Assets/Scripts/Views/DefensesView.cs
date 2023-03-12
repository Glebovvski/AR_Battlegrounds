using Managers;
using UnityEngine;
using UnityEngine.UI;
using ViewModels;
using Zenject;

namespace Views
{
    public class DefensesView : MonoBehaviour
    {
        private DefensesViewModel ViewModel { get; set; }
        private AudioManager AudioManager { get; set; }
        private GameModeModel GameModeModel {get;set;}

        [SerializeField] private GameObject defensesPanel;
        [SerializeField] private GameObject cancelBtn;
        [SerializeField] private Button resetCameraBtn;

        [Inject]
        private void Construct(DefensesViewModel vm, AudioManager audioManager, GameModeModel gameModeModel)
        {
            ViewModel = vm;
            AudioManager = audioManager;
            GameModeModel = gameModeModel;
        }

        private void Start()
        {
            ViewModel.OnDefenseSelected += DefenseSelected;
            ViewModel.OnOpen += Show;
            ViewModel.OnClose += Hide;
        }

        public void Show()
        {
            defensesPanel.SetActive(true);
            resetCameraBtn.gameObject.SetActive(!GameModeModel.IsARModeSelected);
        }
        public void Hide()
        {
            defensesPanel.SetActive(false);
        }
        public void CancelSelection()
        {
            AudioManager.PlayUI();
            ViewModel.DeselectDefense();
            ToggleCancelBtn(false);
        }

        private void DefenseSelected()
        {
            AudioManager.PlayUI();
            ToggleCancelBtn(true);
        }

        public void ToggleCancelBtn(bool active) => cancelBtn.SetActive(active);

        public void ResetCamera() => ViewModel.ResetCamera();

        private void OnDestroy()
        {
            ViewModel.OnDefenseSelected -= DefenseSelected;
            ViewModel.OnOpen -= Show;
            ViewModel.OnClose -= Hide;
        }
    }
}