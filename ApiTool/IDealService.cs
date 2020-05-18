using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiTool
{
    interface IDealService
    { 
        void GetListings(int listing = -1);

        void Default();
    }
}
