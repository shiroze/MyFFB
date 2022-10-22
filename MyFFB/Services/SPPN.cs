using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class SPPN
    {
        static IContainer _container;

        static SPPN()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RPPN>().As<IRPPN>();
            _container = builder.Build();
        }

        public static IRPPN PPN(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IRPPN>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
