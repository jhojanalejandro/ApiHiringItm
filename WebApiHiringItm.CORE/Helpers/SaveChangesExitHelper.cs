using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiHiringItm.CONTEXT.Context;
using WebApiHiringItm.CORE.Helpers.InterfacesHelpers;

namespace WebApiHiringItm.CORE.Helpers
{
    public class SaveChangesExitHelper : ISaveChangesExitHelper
    {
        #region Fields
        private readonly HiringContext _context;
        #endregion

        #region Builder
        public SaveChangesExitHelper(HiringContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public async Task<bool> SaveChangesDB()
        {
            var resp = await _context.SaveChangesAsync();
            if (resp > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
