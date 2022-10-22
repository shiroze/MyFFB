using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class SPrice
    {
        static IContainer _container;

        static SPrice()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RPrice>().As<IRPrice>();
            _container = builder.Build();
        }

        public static IRPrice Price(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IRPrice>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
