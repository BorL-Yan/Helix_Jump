using VContainer.Unity;

public class PlatformEntry : IStartable
{
    private readonly LevelAction _levelAction;
    
    PlatformEntry(LevelAction levelAction)
    {
        _levelAction = levelAction;
    }

    public void Start()
    {
        _levelAction.OnStartLevel?.Invoke();
    }
}
