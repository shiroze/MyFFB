using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class SPriceDefault
    {
        static IContainer _container;

        static SPriceDefault()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RPriceDefault>().As<IRPriceDefault>();
            _container = builder.Build();
        }

        public static IRPriceDefault PriceDefault(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IRPriceDefault>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
