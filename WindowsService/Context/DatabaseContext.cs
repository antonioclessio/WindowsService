using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passaredo.Integracao.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=Entities")
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<IntegracaoSoap> IntegracaoSOAP { get; set; }
    }
}
