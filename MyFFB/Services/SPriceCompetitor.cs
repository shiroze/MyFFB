using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class SPriceCompetitor
    {
        static IContainer _container;

        static SPriceCompetitor()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RPriceCompetitor>().As<IRPriceCompetitor>();
            _container = builder.Build();
        }

        public static IRPriceCompetitor PriceCompetitor(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IRPriceCompetitor>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
