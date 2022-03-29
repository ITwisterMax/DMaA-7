using System;

namespace Syntax.Types
{
    /// <summary>
    ///     Element type class
    /// </summary>
    public class ElementType
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        ///
        /// <param name="name">Name</param>
        public ElementType(string name)
        {
            Name = name;
        }

        /// <summary>
        ///     Name
        /// </summary>
        public String Name { get; private set; }
    }
}