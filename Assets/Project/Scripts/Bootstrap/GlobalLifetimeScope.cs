namespace BC.Bootstrap
{
    public class GlobalLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            new InputInstaller().Install(builder);
        }
    }
}