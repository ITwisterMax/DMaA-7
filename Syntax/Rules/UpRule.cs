using System.Collections.Generic;
using Syntax.Types;
using Syntax.Elements;

namespace Syntax.Rules
{
    /// <summary>
    ///     Up rule class
    /// </summary>
    public class UpRule : Rule
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
        public UpRule(ElementType startElementType, ElementType firstArgumentType, ElementType secondArgumentType) 
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
            MakeSameLength(first, second);

            first.ShiftTransform(0, second.StartPosition.Y + Random.Next(0, 3));

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

            var connect = new Element(StartElementType, resultLines, first.StartPosition, second.EndPosition);

            return connect;
        }

        /// <summary>
        ///     Make same length
        /// </summary>
        ///
        /// <param name="first">First element</param>
        /// <param name="second">Second element</param>
        private static void MakeSameLength(Element first, Element second)
        {
            Element largestElement = GetLargestElement(first, second);
            Element shortestElement = GetShortestElement(first, second);

            shortestElement.ScaleTransform(largestElement.Length/shortestElement.Length, 1.0);
        }

        /// <summary>
        ///     Get largest element
        /// </summary>
        ///
        /// <param name="first">First element</param>
        /// <param name="second">Second element</param>
        ///
        /// <returns>Element</returns>
        private static Element GetLargestElement(Element first, Element second)
        {
            return first.Length > second.Length ? first : second;
        }

        /// <summary>
        ///     Get shortest element
        /// </summary>
        ///
        /// <param name="first">First element</param>
        /// <param name="second">Second element</param>
        ///
        /// <returns>Element</returns>
        private static Element GetShortestElement(Element first, Element second)
        {
            return first.Length < second.Length ? first : second;
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

            return second.StartPosition.Y - randomDelta < first.EndPosition.Y;
        }
    }
}