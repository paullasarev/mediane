using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Mediane.Tests.Utils
{
    class FakeViewDataContainer : IViewDataContainer
    {
        private ViewDataDictionary ViewDataValue;

        public ViewDataDictionary ViewData
        {
            get
            {
                return ViewDataValue;
            }
            set
            {
                ViewDataValue = value;
            }
        }
    }

}
