using MyAttd.Models;

namespace MyAttd.DataAccess
{
    public interface IRSeqMstr
    {
        T_SeqMstr GetAvailCode(string code, string parameter);
        bool UpdateTSeqMstr(T_SeqMstr seqMstr);
    }
}
