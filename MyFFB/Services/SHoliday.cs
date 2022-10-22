using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class SHoliday
    {
        static IContainer _container;

        static SHoliday()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RHoliday>().As<IRHoliday>();
            _container = builder.Build();
        }

        public static IRHoliday Holiday(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IRHoliday>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
