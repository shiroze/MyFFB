using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class SSeqMstr
    {
        static IContainer _container;

        static SSeqMstr()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RSeqMstr>().As<IRSeqMstr>();
            _container = builder.Build();
        }

        public static IRSeqMstr SeqMstr(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IRSeqMstr>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
