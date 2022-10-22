using MyAttd.Models;
using System.Collections.Generic;

namespace MyAttd.DataAccess
{
    public interface IRPriceCompetitor
    {
        List<T_PriceCompetitor> GetListPriceCompetitor(bool? defunct, string Periode, int FCUserID);
        T_PriceCompetitor FindPriceCompetitor(string Date, string PMKSID, string CompetitorName);
        bool Add(T_PriceCompetitor obj);
        bool Edit(T_PriceCompetitor obj);
        bool Delete(T_PriceCompetitor obj);
        bool TarikData(string Date, int ACCESSID);

        //int CalculatePriceCompetitor(int AccessID, string p_tglTimbang);
    }
}
