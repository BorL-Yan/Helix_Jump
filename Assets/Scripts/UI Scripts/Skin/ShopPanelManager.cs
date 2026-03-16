using System;
using UI_Scripts;
using UnityEngine;

public class ShopPanelManager : MonoBehaviour
{
    [SerializeField] private SkinCategoryButton basicPanel;
    [SerializeField] private SkinCategoryButton rarePanel;
    [SerializeField] private SkinCategoryButton funkyPanel;
    [SerializeField] private SkinCategoryButton epicPanel;
    [SerializeField] private SkinCategoryButton chestPanel;
    
    public void Start()
    {
        SelectCategory(basicPanel);
    }
    
    public void SelectCategory(SkinCategoryButton obj)
    {
        basicPanel.SetActive(obj == basicPanel);
        rarePanel.SetActive(obj == rarePanel);
        funkyPanel.SetActive(obj == funkyPanel);
        epicPanel.SetActive(obj == epicPanel);
        chestPanel.SetActive(obj == chestPanel);
    }
}