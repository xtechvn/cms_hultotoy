using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IIdentifierServiceRepository
    {
        Task<string> buildOrderNoManual(int company_type = 0);// don thu cong

    }
}
