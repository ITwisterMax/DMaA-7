using System.Windows;
using Syntax.Elements;

namespace Syntax.Types
{
    /// <summary>
    ///     Terminal element type class
    /// </summary>
    internal class TerminalElementType : ElementType
    {
        /// <summary>
        ///     Standart element line
        /// </summary>
        private readonly Line standartElementLine;

        /// <summary>
        ///     Default constructor
        /// </summary>
        ///
        /// <param name="name">Name</param>
        /// <param name="standartElementLine">Standart element line</param>
        public TerminalElementType(string name, Line standartElementLine) : base(name)
        {
            this.standartElementLine = standartElementLine;
        }

        /// <summary>
        ///     Standart element getter
        /// </summary>
        public Element StandartElement
        {
            get
            {
                return new Element(
                    this,
                    new Line(new Point(standartElementLine.From.X, standartElementLine.From.Y),
                    new Point(standartElementLine.To.X, standartElementLine.To.Y))
                );
            }
        }
    }
}