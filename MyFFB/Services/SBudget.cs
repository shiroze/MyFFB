using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class SBudget
    {
        static IContainer _container;

        static SBudget()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RBudget>().As<IRBudget>();
            _container = builder.Build();
        }

        public static IRBudget Budget(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IRBudget>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
