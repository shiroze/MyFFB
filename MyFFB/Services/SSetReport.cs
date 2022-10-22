using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class SSetReport
    {
        static IContainer _container;

        static SSetReport()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RSetReport>().As<IRSetReport>();
            _container = builder.Build();
        }

        public static IRSetReport SetReport(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IRSetReport>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
