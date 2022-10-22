using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class SReport
    {
        static IContainer _container;

        static SReport()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RReport>().As<IRReport>();
            _container = builder.Build();
        }

        public static IRReport Report(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IRReport>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
