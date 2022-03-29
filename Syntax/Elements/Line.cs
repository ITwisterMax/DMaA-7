 using System.Windows;

namespace Syntax.Elements
{
    public class Line
    {
        /// <summary>
        ///     From point
        /// </summary>
        public Point From { get; set; }

        /// <summary>
        ///     To point
        /// </summary>
        public Point To { get; set; }

        /// <summary>
        ///     Default constructor
        /// </summary>
        ///
        /// <param name="from">From point</param>
        /// <param name="to">To point</param>
        public Line(Point from, Point to)
        {
            From = from;
            To = to;
        }

        /// <summary>
        ///     Scale transform
        /// </summary>
        ///
        /// <param name="xScale">X scale</param>
        /// <param name="yScale">Y scale</param>
        /// <param name="centerPoint">Center point</param>
        public void ScaleTransform(double xScale, double yScale, Point centerPoint)
        {
            Vector length = To - From;
            Vector startDelta = From - centerPoint;

            startDelta.X *= xScale;
            startDelta.Y *= yScale;

            From = centerPoint + startDelta;

            length.X *= xScale;
            length.Y *= yScale;

            To = From + length;
        }

        /// <summary>
        ///     Shift transform
        /// </summary>
        ///
        /// <param name="xDelta">X delta</param>
        /// <param name="yDelta">y delta</param>
        public void ShiftTransform(double xDelta, double yDelta)
        {
            var shift = new Vector(xDelta, yDelta);

            To += shift;
            From += shift;
        }
    }
}