using UnityEngine;

public class PearShogunController : MonoBehaviour
{
    private void OnDestroy()
    {
        PlotController.StrawberryShogunDefeated = true;
    }
}
