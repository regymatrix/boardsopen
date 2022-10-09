using Boards.DAL.DAO.Base;
using Boards.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boards.DAL.DAO
{
    public class LeadDAO : BaseDAO<Lead>
    {
        public new void Add(Lead lead) {
            if (base.Get().Where(x => x.Email == lead.Email).Count() == 0)
            {
                base.Add(lead);
            }
        } 
    }
}
