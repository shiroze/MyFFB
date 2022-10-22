using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class SUser
    {
        static IContainer _container;

        static SUser()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RUser>().As<IRUser>();
            _container = builder.Build();
        }

        public static IRUser User(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IRUser>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
