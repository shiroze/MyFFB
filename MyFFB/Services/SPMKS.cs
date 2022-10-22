using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class SPMKS
    {
        static IContainer _container;

        static SPMKS()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RPMKS>().As<IRPMKS>();
            _container = builder.Build();
        }

        public static IRPMKS PMKS(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IRPMKS>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
