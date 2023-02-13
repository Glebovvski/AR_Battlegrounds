using UnityEngine;
using ViewModels;
using Zenject;

namespace Views
{
    public class DefensesView : MonoBehaviour
    {
        private DefensesViewModel ViewModel { get; set; }

        [SerializeField] private GameObject defensesPanel;
        [SerializeField] private GameObject cancelBtn;

        [Inject]
        private void Construct(DefensesViewModel vm)
        {
            ViewModel = vm;
        }

        private void Start()
        {
            ViewModel.OnDefenseSelected += DefenseSelected;
            ViewModel.OnOpen += Show;
            ViewModel.OnClose += Hide;
        }

        public void Show() => defensesPanel.SetActive(true);
        public void Hide() => defensesPanel.SetActive(false);

        public void CancelSelection()
        {
            ViewModel.DeselectDefense();
            ToggleCancelBtn(false);
        }

        private void DefenseSelected()
        {
            ToggleCancelBtn(true);
        }

        public void ToggleCancelBtn(bool active) => cancelBtn.SetActive(active);

        private void OnDestroy()
        {
            ViewModel.OnDefenseSelected -= DefenseSelected;
            ViewModel.OnOpen -= Show;
            ViewModel.OnClose -= Hide;
        }
    }
}