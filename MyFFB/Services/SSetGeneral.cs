using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class SSetGeneral
    {
        static IContainer _container;

        static SSetGeneral()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RSetGeneral>().As<IRSetGeneral>();
            _container = builder.Build();
        }

        public static IRSetGeneral SetGeneral(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IRSetGeneral>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
