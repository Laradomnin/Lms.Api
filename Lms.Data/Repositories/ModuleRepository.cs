using Lms.Core.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
    internal class ModuleRepository : IModuleRepository

    {
        private readonly LmsApiContext db;
        public ModuleRepository(LmsApiContext db)
        {
            this.db = db;
        }
        public async Task<IEnumerable<Module>> GetAllModules()
        {
            return await db.Module.ToListAsync();
        }

        public Task<Module> GetModule(int? id)
        {
            throw new NotImplementedException();
        }
      
        public Task<Module> FindAsync(int? id)
        {
            throw new NotImplementedException();
        }


        public Task<bool> AnyAsync(int? id)
        {
            throw new NotImplementedException();
        }
        public void Add(Module module)
        {
            throw new NotImplementedException();
        }
        public  void Update (Module module)
        {
            throw new NotImplementedException();
        }
        public void Remove (Module module)
        {
            throw new NotImplementedException();
        }

       
    }
}
