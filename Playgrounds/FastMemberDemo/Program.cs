using FastMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastMemberDemo
{
    class Program
    {
        class MyClass
        {
            public int IntProperty { get; set; } = 1234567890;

            public string StringProeprty { get; set; } = "asdf";
        }

        static void Main(string[] args)
        {
            TypeAccessor typeAccessor = TypeAccessor.Create(typeof(MyClass));

            ObjectAccessor objectAccessor = ObjectAccessor.Create(new MyClass());

            MyClass newMyClass = typeAccessor.CreateNew() as MyClass;

            MemberSet members = typeAccessor.GetMembers();
        }
    }
}
