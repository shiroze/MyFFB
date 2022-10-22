using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class SGroupIncentive
    {
        static IContainer _container;

        static SGroupIncentive()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RGroupIncentive>().As<IRGroupIncentive>();
            _container = builder.Build();
        }

        public static IRGroupIncentive GroupIncentive(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IRGroupIncentive>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
