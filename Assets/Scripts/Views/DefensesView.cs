using UnityEngine;

public class DefensesView : MonoBehaviour
{
    [SerializeField] private DefensesViewModel DefensesViewModel;
    [SerializeField] private GameObject cancelBtn;

    private void Start()
    {
        DefensesViewModel.OnDefenseSelected += DefenseSelected;
    }

    public void Show() => this.gameObject.SetActive(true);
    public void Hide() => this.gameObject.SetActive(false);

    public void CancelSelection()
    {
        DefensesViewModel.DeselectDefense();
        ToggleCancelBtn(false);
    }

    private void DefenseSelected()
    {
        ToggleCancelBtn(true);
    }

    public void ToggleCancelBtn(bool active) => cancelBtn.SetActive(active);

    private void OnDestroy()
    {
        DefensesViewModel.OnDefenseSelected -= DefenseSelected;
    }
}
