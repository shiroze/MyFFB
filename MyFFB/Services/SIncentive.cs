using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class SIncentive
    {
        static IContainer _container;

        static SIncentive()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RIncentive>().As<IRIncentive>();
            _container = builder.Build();
        }

        public static IRIncentive Incentive(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IRIncentive>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
