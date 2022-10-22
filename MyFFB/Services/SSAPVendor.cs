using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class SSAPVendor
    {
        static IContainer _container;

        static SSAPVendor()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RSAPVendor>().As<IRSAPVendor>();
            _container = builder.Build();
        }

        public static IRSAPVendor SAPVendor(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IRSAPVendor>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
