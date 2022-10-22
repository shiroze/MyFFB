using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class SEmailPrice
    {
        static IContainer _container;

        static SEmailPrice()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<REmailPrice>().As<IREmailPrice>();
            _container = builder.Build();
        }

        public static IREmailPrice EmailPrice(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IREmailPrice>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
