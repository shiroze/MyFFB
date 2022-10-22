using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class SCompetitor
    {
        static IContainer _container;

        static SCompetitor()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RCompetitor>().As<IRCompetitor>();
            _container = builder.Build();
        }

        public static IRCompetitor Competitor(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IRCompetitor>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
