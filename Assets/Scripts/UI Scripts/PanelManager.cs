using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] panels;
    
    private void Start()
    {
        CloseAllPanels();
    }

    public void CloseAllPanels()
    {
        foreach (var panel in panels)
        {
            panel.SetActive(false);
        }
    }
    
    public void OpenPanel(GameObject panelToOpen)
    {
        CloseAllPanels();
        foreach (var panel in panels)
        {
            panel.SetActive(panel == panelToOpen);
        }
    }
}