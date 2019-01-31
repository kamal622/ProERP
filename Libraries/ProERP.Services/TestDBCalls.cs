using ProERP.Core.Data;
using ProERP.Data;
using ProERP.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProERP.Services
{
    public class TestDBCalls
    {
        private readonly IRepository<Data.Models.User> _userRepository;

        public TestDBCalls(IRepository<Data.Models.User> userRepository)
        {
            this._userRepository = userRepository;
        }

        public Data.Models.User[] Test()
        {
            return this._userRepository.Table.ToArray();
        }
    }
}
