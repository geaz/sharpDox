using System;
using System.Collections.Generic;

namespace SharpDox.TestProject
{
    /// <summary>
    /// Test
    /// </summary>
    /// <seealso cref="Regression8"/>
    /// <seealso cref=""/>
    public class SeeAlsoDocType<TK>
    {
        public event Action TestEvent;

        private string TestField;

        /// <summary>
        /// Test <see cref="Regression1"/>
        /// </summary>
        public void TestMethod()
        {

        }

        /// <summary>
        /// Test 
        /// <see cref=""></see> 
        /// <see cref="NotExist"></see> 
        /// <see cref="SeeAlsoDocType{TK}"/>
        /// <see cref="TestMethod"/>
        /// <see cref="TestProp"/>
        /// <see cref="TestEvent"/>
        /// <see cref="TestField"/>
        /// <see cref="TestMethod3{T}"/>
        /// <see cref="TestMethod3{T,TV}"/>
        /// </summary>
        public void TestMethod2()
        {
            
        }

        public void TestMethod3<T>(T s)
        {
            
        }

        public void TestMethod3<T, TV>(T s, TV v)
        {
            
        }

        public string TestProp { get; set; }
    }
}
