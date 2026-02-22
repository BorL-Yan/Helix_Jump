using VContainer;
using VContainer.Unity;

namespace Ball.Menu
{
    public class MenuBallLifeTimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<BallAction>(Lifetime.Scoped).AsSelf();
        }
    }
}