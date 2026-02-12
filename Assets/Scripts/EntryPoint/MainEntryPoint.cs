using System;
using UnityEngine;


public class MainEntryPoint : MonoBehaviour
{
    private void Awake()
    {
        if (GameManager.Instance == null)
        {
            GameManager manager = Resources.Load<GameManager>("Menegers/GameBootstrap");
            Instantiate(manager).Initializ();
        }   
        
        Init();
    }


    private void Init()
    {
        
    }
}
