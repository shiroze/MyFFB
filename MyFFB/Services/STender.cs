using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class STender
    {
        static IContainer _container;

        static STender()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RTender>().As<IRTender>();
            _container = builder.Build();
        }

        public static IRTender Tender(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IRTender>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
