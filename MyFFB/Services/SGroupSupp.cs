using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class SGroupSupp
    {
        static IContainer _container;

        static SGroupSupp()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RGroupSupp>().As<IRGroupSupp>();
            _container = builder.Build();
        }

        public static IRGroupSupp GroupSupp(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IRGroupSupp>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
