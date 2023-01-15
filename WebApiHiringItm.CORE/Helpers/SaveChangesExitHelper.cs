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
        private readonly Hiring_V1Context _context;
        #endregion

        #region Builder
        public SaveChangesExitHelper(Hiring_V1Context context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public async Task<bool> SaveChangesDB()
        {
            await _context.SaveChangesAsync();
            return await Task.FromResult(true);
        }
        #endregion
    }
}
