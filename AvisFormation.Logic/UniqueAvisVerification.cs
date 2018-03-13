using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvisFormation.Data;

namespace AvisFormation.Logic
{
    public class UniqueAvisVerification
    {
        public bool EstAuthoriseACommenter(string userId, int formationId)
        {
            using (var context = new AvisEntities())
            {
                var personEntity = context.Avis.FirstOrDefault(a => a.UserId == userId && a.IdFormation == formationId);
                if (personEntity == null)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
