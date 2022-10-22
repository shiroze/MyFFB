using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class SAreaRegional
    {
        static IContainer _container;

        static SAreaRegional()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RAreaRegional>().As<IRAreaRegional>();
            _container = builder.Build();
        }

        public static IRAreaRegional AreaRegional(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IRAreaRegional>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
