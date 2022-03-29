using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Syntax.Types;

namespace Syntax.Elements
{
    /// <summary>
    ///     Element class
    /// </summary>
    public class Element
    {
        /// <summary>
        ///     Element type
        /// </summary>
        public ElementType ElementType { get; private set; }

        /// <summary>
        ///     Start position
        /// </summary>
        public Point StartPosition { get; set; }

        /// <summary>
        ///     End position
        /// </summary>
        public Point EndPosition { get; set; }

        /// <summary>
        ///     Lines
        /// </summary>
        public IEnumerable<Line> Lines { get; private set; }

        /// <summary>
        ///     Length
        /// </summary>
        public double Length
        {
            get { return Math.Abs(EndPosition.X - StartPosition.X); }
        }

        /// <summary>
        ///     Height
        /// </summary>
        public double Height
        {
            get { return Math.Abs(EndPosition.Y - StartPosition.Y); }
        }

        /// <summary>
        ///     Default constructor
        /// </summary>
        /// <param name="elementType">Element type</param>
        public Element(ElementType elementType)
        {
            ElementType = elementType;
            Lines = new List<Line>();
        }

        /// <summary>
        ///     Default constructor
        /// </summary>
        /// <param name="elementType">Element type</param>
        /// <param name="line">Line</param>
        public Element(ElementType elementType, Line line)
        {
            ElementType = elementType;
            Lines = new List<Line> {line};

            StartPosition = new Point(Math.Min(line.From.X, line.To.X), Math.Max(line.From.Y, line.To.Y));
            EndPosition = new Point(Math.Max(line.From.X, line.To.X), Math.Min(line.From.Y, line.To.Y));
        }

        /// <summary>
        ///     Default constructor
        /// </summary>
        /// <param name="elementType">Element type</param>
        /// <param name="lines">Line</param>
        /// <param name="startPoint">Start point</param>
        /// <param name="endPoint">End point</param>
        public Element(ElementType elementType, IEnumerable<Line> lines, Point startPoint, Point endPoint)
        {
            StartPosition = startPoint;
            EndPosition = endPoint;
            ElementType = elementType;
            Lines = lines;
        }

        /// <summary>
        ///     Scale transform
        /// </summary>
        ///
        /// <param name="xScale">X scale</param>
        /// <param name="yScale">Y scale</param>
        public void ScaleTransform(double xScale, double yScale)
        {
            Vector delta = EndPosition - StartPosition;

            delta.X *= xScale;
            delta.Y *= yScale;

            EndPosition = StartPosition + delta;

            foreach (Line line in Lines)
            {
                line.ScaleTransform(xScale, yScale, StartPosition);
            }
        }

        /// <summary>
        ///     Shift transform
        /// </summary>
        ///
        /// <param name="xDelta">X delta</param>
        /// <param name="yDelta">Y delta</param>
        public void ShiftTransform(double xDelta, double yDelta)
        {
            var shift = new Vector(xDelta, yDelta);

            StartPosition += shift;
            EndPosition += shift;

            foreach (Line line in Lines)
            {
                line.ShiftTransform(xDelta, yDelta);
            }
        }

        /// <summary>
        ///     Get geometry group
        /// </summary>
        ///
        /// <returns>GeometryGroup</returns>
        public GeometryGroup GetGeometryGroup()
        {
            var result = new GeometryGroup();

            foreach (Line line in Lines)
            {
                result.Children.Add(new LineGeometry(GetScreenPoint(line.From), GetScreenPoint(line.To)));
            }

            return result;
        }

        /// <summary>
        ///     Get screen point
        /// </summary>
        ///
        /// <param name="point">Point</param>
        ///
        /// <returns>Point</returns>
        private Point GetScreenPoint(Point point)
        {
            return new Point(point.X, StartPosition.Y - point.Y);
        }
    }
}