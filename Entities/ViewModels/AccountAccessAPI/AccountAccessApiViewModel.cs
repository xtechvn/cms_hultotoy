using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels.AccountAccessAPI
{
    public class AccountAccessApiViewModel
    {
        public int Id { get; set; }
        public int Id_AccountAccessAPIPermission { get; set; }
        public int Id_AllCode { get; set; }
        public string UserName { get; set; }

        public string CodeName { get; set; }

        public short Status { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateLast { get; set; }

        public string Description { get; set; }
    }
}
