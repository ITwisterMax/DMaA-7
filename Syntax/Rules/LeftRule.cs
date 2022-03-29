using System;
using System.Collections.Generic;
using System.Windows;
using Syntax.Types;
using Syntax.Elements;

namespace Syntax.Rules
{
    /// <summary>
    ///     Left rule class
    /// </summary>
    public class LeftRule : Rule
    {
        /// <summary>
        ///     Random delta
        /// </summary>
        private const int randomDelta = 3;

        /// <summary>
        ///     Default constructor
        /// </summary>
        ///
        /// <param name="startElementType">Start element type</param>
        /// <param name="firstArgumentType">First element type</param>
        /// <param name="secondArgumentType">Second element type</param>
        public LeftRule(ElementType startElementType, ElementType firstArgumentType, ElementType secondArgumentType) 
            : base(startElementType, firstArgumentType, secondArgumentType)
        {

        }

        /// <summary>
        ///     Transform connect
        /// </summary>
        ///
        /// <param name="first">First element</param>
        /// <param name="second">Second element</param>
        ///
        /// <returns>Element</returns>
        public override Element TransformConnect(Element first, Element second)
        {
            second.ShiftTransform(first.Length + Random.Next(1, 10), 0);

            return Connect(first, second);
        }

        /// <summary>
        ///     Connect
        /// </summary>
        ///
        /// <param name="first">First element</param>
        /// <param name="second">Second element</param>
        ///
        /// <returns>Element</returns>
        public override Element Connect(Element first, Element second)
        {
            var resultLines = new List<Line>(first.Lines);
            resultLines.AddRange(second.Lines);

            var startPosition = new Point(first.StartPosition.X, Math.Max(first.StartPosition.Y, second.StartPosition.Y));
            var endPosition = new Point(second.EndPosition.X, Math.Min(first.EndPosition.Y, second.EndPosition.Y));

            var connect = new Element(StartElementType, resultLines, startPosition, endPosition);

            return connect;
        }

        /// <summary>
        ///     Is rule pare
        /// </summary>
        ///
        /// <param name="first">First element</param>
        /// <param name="second">Second element</param>
        ///
        /// <returns>bool</returns>
        public override bool IsRulePare(Element first, Element second)
        {
            if (first.ElementType.Name != FirstArgumentType.Name || second.ElementType.Name != SecondArgumentType.Name) 
            {
                return false;
            }

            return first.EndPosition.X - randomDelta < second.StartPosition.X;
        }
    }
}