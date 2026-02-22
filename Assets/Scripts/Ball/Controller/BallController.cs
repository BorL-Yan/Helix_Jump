using Ball.Configuration;
using Ball.Controller;
using UnityEngine;
using VContainer.Unity;


public class BallController : IStartable
{
    private readonly BallFlags _flags;
    private readonly Transform _root;
    
    
    public BallController(BallFlags flags, Transform root)
    {
        _flags = flags;
        _root = root;
    }

    public void Start()
    {
        _flags.gravity = true;
        BallGroundedEffect.Instance.Initialize(_root);
    }
}
