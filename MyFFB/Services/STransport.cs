using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class STransport
    {
        static IContainer _container;

        static STransport()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RTransport>().As<IRTransport>();
            _container = builder.Build();
        }

        public static IRTransport Transport(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IRTransport>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
