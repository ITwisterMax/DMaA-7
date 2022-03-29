using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Syntax;
using Syntax.Types;
using Element = Syntax.Elements.Element;
using Line = Syntax.Elements.Line;

namespace Laba7
{
    /// <summary>
    ///     WPF main class
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        ///     Start point
        /// </summary>
        private Point startPoint;

        /// <summary>
        ///     Flag for line drawing
        /// </summary>
        private bool isDrawingModeEnabled = false;

        /// <summary>
        ///     Geometry group
        /// </summary>
        private GeometryGroup currentGroup;

        /// <summary>
        ///     Elements
        /// </summary>
        private List<Element> drawedElements = new List<Element>();

        /// <summary>
        ///     Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     On synthesis button click
        /// </summary>
        ///
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments</param>
        private void SynthesisButton_Click(object sender, RoutedEventArgs e)
        {
            Clear();

            var grammar = new ManGrammar();
            Element home = grammar.GetMan();

            foreach (Line line in home.Lines)
            {
                drawedElements.Add(ManGrammar.GetTerminalElement(line));
            }

            home.ScaleTransform((BorderImage.ActualWidth - 100) / home.Length, (BorderImage.ActualHeight - 100) / home.Height);
            currentGroup = home.GetGeometryGroup();

            UpdateImage();
        }

        /// <summary>
        ///     On recognition button click
        /// </summary>
        ///
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments</param>
        private void RecognitionButton_Click(object sender, RoutedEventArgs e)
        {
            var grammar = new ManGrammar();
            RecognazingResult recognazingResult = grammar.IsMan(drawedElements);

            if (recognazingResult.IsMan)
            {
                MessageBox.Show("Данный рисунок полностью соответствует граматике.");
            }
            else
            {
                MessageBox.Show($"Данный рисунок не соответствует грамматике!{Environment.NewLine}Не найден элемент: {recognazingResult.ErrorElementName}.");
            }
        }

        /// <summary>
        ///     On clear button click
        /// </summary>
        ///
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments</param>
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        /// <summary>
        ///     Clear image
        /// </summary>
        private void Clear()
        {
            currentGroup = new GeometryGroup();
            drawedElements = new List<Element>();

            ClearImage();
        }

        /// <summary>
        ///     Clear image
        /// </summary>
        private void ClearImage()
        {
            currentGroup = new GeometryGroup();
            currentGroup.Children.Add(new LineGeometry(new Point(0, 0), new Point(0, BorderImage.ActualHeight)));
            currentGroup.Children.Add(new LineGeometry(new Point(0, BorderImage.ActualHeight), new Point(BorderImage.ActualWidth, BorderImage.ActualHeight)));
            currentGroup.Children.Add(new LineGeometry(new Point(BorderImage.ActualWidth, BorderImage.ActualHeight), new Point(BorderImage.ActualWidth, 0)));
            currentGroup.Children.Add(new LineGeometry(new Point(BorderImage.ActualWidth, 0), new Point(0, 0)));

            UpdateImage();
        }

        /// <summary>
        ///     Redraw image
        /// </summary>
        private void UpdateImage()
        {
            Image.Source = new DrawingImage(
                new GeometryDrawing(
                    new SolidColorBrush(Color.FromRgb(75, 0, 130)),
                    new Pen(new SolidColorBrush(Color.FromRgb(75, 0, 130)), 1),
                    currentGroup
                )
            );
        }

        /// <summary>
        ///     On mouse up
        /// </summary>
        ///
        /// <param name="sender">Sender object</param>
        /// <param name="e">Arguments</param>
        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isDrawingModeEnabled)
            {
                isDrawingModeEnabled = false;

                drawedElements.Add(
                    ManGrammar.GetTerminalElement(
                        new Line(GetCoordinates(startPoint), GetCoordinates(e.GetPosition(Image)))
                    )
                );
                currentGroup.Children.Add(new LineGeometry(startPoint, e.GetPosition(Image)));

                UpdateImage();
            }
            else
            {
                isDrawingModeEnabled = true;
                startPoint = e.GetPosition(Image);
            }
        }

        /// <summary>
        ///     Update coordinates if needed
        /// </summary>
        ///
        /// <param name="position">Point position</param>
        ///
        /// <returns>Point</returns>
        private Point GetCoordinates(Point position)
        {
            return new Point(position.X, WindowGrid.ActualHeight - 20 - position.Y);
        }
    }
}
