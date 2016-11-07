using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveDataLayer.AttributeModel
{
    [AttributeUsage(System.AttributeTargets.Property, AllowMultiple = true)]
    public class PrimaryKeyAttribute : System.Attribute
    {
        public PrimaryKeyAttribute(bool IsIdentiy = false)
        {
            IsPrimary = true;
            IsIdentity = IsIdentiy;
        }

        public bool IsPrimary { get; private set; }

        public bool IsIdentity { get; private set; }
    }
}
