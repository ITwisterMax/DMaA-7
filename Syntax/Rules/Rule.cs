using System;
using Syntax.Types;
using Syntax.Elements;

namespace Syntax.Rules
{
    /// <summary>
    ///     Abstract rule class
    /// </summary>
    public abstract class Rule
    {
        /// <summary>
        ///     Random
        /// </summary>
        protected Random Random { get; private set; }

        /// <summary>
        ///     Start element type
        /// </summary>
        public ElementType StartElementType { get; private set; }

        /// <summary>
        ///     First element type
        /// </summary>
        public ElementType FirstArgumentType { get; private set; }

        /// <summary>
        ///     Second element type
        /// </summary>
        public ElementType SecondArgumentType { get; private set; }

        /// <summary>
        ///     Default constructor
        /// </summary>
        ///
        /// <param name="startElementType">Start element type</param>
        /// <param name="firstArgumentType">First element type</param>
        /// <param name="secondArgumentType">Second element type</param>
        protected Rule(ElementType startElementType, ElementType firstArgumentType, ElementType secondArgumentType)
        {
            SecondArgumentType = secondArgumentType;
            FirstArgumentType = firstArgumentType;
            StartElementType = startElementType;

            Random = new Random();
        }

        /// <summary>
        ///     Transform connect
        /// </summary>
        ///
        /// <param name="first">First element</param>
        /// <param name="second">Second element</param>
        ///
        /// <returns>Element</returns>
        public abstract Element TransformConnect(Element first, Element second);

        /// <summary>
        ///     Connect
        /// </summary>
        ///
        /// <param name="first">First element</param>
        /// <param name="second">Second element</param>
        ///
        /// <returns>Element</returns>
        public abstract Element Connect(Element first, Element second);

        /// <summary>
        ///     Is rule pare
        /// </summary>
        ///
        /// <param name="first">First element</param>
        /// <param name="second">Second element</param>
        ///
        /// <returns>bool</returns>
        public abstract bool IsRulePare(Element first, Element second);
    }
}