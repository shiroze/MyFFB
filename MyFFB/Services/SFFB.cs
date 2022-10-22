using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class SFFB
    {
        static IContainer _container;

        static SFFB()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RFFB>().As<IRFFB>();
            _container = builder.Build();
        }

        public static IRFFB FFB(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IRFFB>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
