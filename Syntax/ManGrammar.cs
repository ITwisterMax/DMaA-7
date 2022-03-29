using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Syntax.Elements;
using Syntax.Rules;
using Syntax.Types;

namespace Syntax
{
    /// <summary>
    ///     Man grammar class
    /// </summary>
    public class ManGrammar
    {
        /// <summary>
        ///     Elements
        /// </summary>
        private static readonly Dictionary<string, ElementType> ElementTypes = GetElementTypes();

        /// <summary>
        ///     Rules
        /// </summary>
        private readonly List<Rule> rules;

        /// <summary>
        ///     Start element type
        /// </summary>
        private readonly ElementType startElementType;

        /// <summary>
        ///     Default constructor
        /// </summary>
        public ManGrammar()
        {
            startElementType = new ElementType("Человек");

            rules = new List<Rule>
            {
                new LeftRule(ElementTypes["Верхняя часть формы"], ElementTypes["a2"], ElementTypes["a3"]),
                new LeftRule(ElementTypes["Нижняя часть формы"], ElementTypes["a3"], ElementTypes["a2"]),
                new UpRule(ElementTypes["Форма"], ElementTypes["Верхняя часть формы"], ElementTypes["Нижняя часть формы"]),
                new UpRule(ElementTypes["Правый глаз"], ElementTypes["a1"], ElementTypes["Нижняя часть формы"]),
                new UpRule(ElementTypes["Левый глаз"], ElementTypes["a1"], ElementTypes["Нижняя часть формы"]),
                new LeftRule(ElementTypes["Глаза"], ElementTypes["Левый глаз"], ElementTypes["Правый глаз"]),
                new UpRule(ElementTypes["Лицо"], ElementTypes["Глаза"], ElementTypes["a1"]),
                new InsideRule(ElementTypes["Голова"], ElementTypes["Лицо"], ElementTypes["Форма"]),
                new LeftRule(ElementTypes["Волосы справа"], ElementTypes["a2"], ElementTypes["a2"]),
                new LeftRule(ElementTypes["Волосы слева"], ElementTypes["a2"], ElementTypes["a2"]),
                new LeftRule(ElementTypes["Волосы"], ElementTypes["Волосы слева"], ElementTypes["Волосы справа"]),
                new UpRule(startElementType, ElementTypes["Волосы"], ElementTypes["Голова"]),
            };
        }

        /// <summary>
        ///     Get element types
        /// </summary>
        ///
        /// <returns>static Dictionary<string, ElementType></returns>
        private static Dictionary<string, ElementType> GetElementTypes()
        {
            return new Dictionary<string, ElementType>
            {
                {"a1", new TerminalElementType("a1", new Line(new Point(0, 0), new Point(10, 0)))},
                {"a2", new TerminalElementType("a2", new Line(new Point(0, 0), new Point(10, 10)))},
                {"a3", new TerminalElementType("a3", new Line(new Point(10, 0), new Point(0, 10)))},
                {"Глаза", new ElementType("Глаза")},
                {"Правый глаз", new ElementType("Правый глаз")},
                {"Левый глаз", new ElementType("Левый глаз")},
                {"Нижняя часть формы", new ElementType("Нижняя часть формы")},
                {"Верхняя часть формы", new ElementType("Верхняя часть формы")},
                {"Форма", new ElementType("Форма")},
                {"Лицо", new ElementType("Лицо")},
                {"Волосы справа", new ElementType("Волосы справа")},
                {"Волосы слева", new ElementType("Волосы слева")},
                {"Голова", new ElementType("Голова")},
                {"Волосы", new ElementType("Волосы")},
            };
        }

        /// <summary>
        ///     Get start element type
        /// </summary>
        /// <returns></returns>
        public Element GetMan()
        {
            return GetElement(startElementType);
        }

        /// <summary>
        ///     Get element
        /// </summary>
        ///
        /// <param name="elementType">Element type</param>
        ///
        /// <returns>Element</returns>
        private Element GetElement(ElementType elementType)
        {
            var terminalElementType = elementType as TerminalElementType;

            if (terminalElementType != null)
            {
                return terminalElementType.StandartElement;
            }

            Rule rule = rules.FirstOrDefault(x => x.StartElementType.Name == elementType.Name);

            return rule.TransformConnect(GetElement(rule.FirstArgumentType), GetElement(rule.SecondArgumentType));
        }

        /// <summary>
        ///     Check if drawing is correct
        /// </summary>
        ///
        /// <param name="baseElements">Base elements</param>
        ///
        /// <returns>RecognazingResult</returns>
        public RecognazingResult IsMan(IEnumerable<Element> baseElements)
        {
            var elements = new ConcurrentBag<Element>(baseElements);
            for (int i = 0; i < rules.Count - 1; i++)
            {
                ContainingRuleArgumentsResult result = ContainingRuleArguments(elements, rules[i]);
                elements = result.Elements;

                if (!result.IsElementFound)
                {
                    return new RecognazingResult(rules[i].StartElementType.Name, false);
                }
                    
            }

            return new RecognazingResult("", true);
        }

        /// <summary>
        ///     Check if drawing contains rule arguments
        /// </summary>
        ///
        /// <param name="elements">Elements</param>
        /// <param name="rule">Rules</param>
        ///
        /// <returns>ContainingRuleAgrumentsResult</returns>
        private static ContainingRuleArgumentsResult ContainingRuleArguments(ConcurrentBag<Element> elements, Rule rule)
        {
            var result = new ContainingRuleArgumentsResult
            {
                Elements = new ConcurrentBag<Element>(elements),
                IsElementFound = false
            };

            foreach (Element firstElement in elements)
            {
                if (firstElement.ElementType.Name == rule.FirstArgumentType.Name)
                {
                    result = ContainingRuleArgumentsForFirstElement(elements, rule, firstElement, result);
                }
            }

            return result;
        }

        /// <summary>
        ///     Check if drawing contains rule arguments for first element
        /// </summary>
        ///
        /// <param name="elements">Elements</param>
        /// <param name="rule">Rule</param>
        /// <param name="firstElement">First element</param>
        /// <param name="result">Result</param>
        ///
        /// <returns>ContainingRuleArgumentsResult</returns>
        private static ContainingRuleArgumentsResult ContainingRuleArgumentsForFirstElement(
            IEnumerable<Element> elements,
            Rule rule,
            Element firstElement,
            ContainingRuleArgumentsResult result
        ) {
            Element element = firstElement;

            Parallel.ForEach(elements, (Element secondElement) =>
            {
                if (rule.IsRulePare(element, secondElement))
                {
                    result.Elements.Add(rule.Connect(element, secondElement));
                    result.IsElementFound = true;
                }
            });

            return result;
        }

        /// <summary>
        ///     Get terminal element
        /// </summary>
        ///
        /// <param name="line">Line</param>
        ///
        /// <returns>Element</returns>
        public static Element GetTerminalElement(Line line)
        {
            String resultName = GetTerminalElementName(line);

            return new Element(ElementTypes[resultName], line);
        }

        /// <summary>
        ///     Get terminal element name
        /// </summary>
        ///
        /// <param name="line">Line</param>
        ///
        /// <returns>string</returns>
        private static string GetTerminalElementName(Line line)
        {
            double deltaX = line.From.X - line.To.X;
            double deltaY = line.From.Y - line.To.Y;

            if (Math.Abs(deltaY) < 1 || Math.Abs(deltaY / deltaX) < 0.2)
            {
                return "a1";
            }

            Point highPoint = line.To.Y > line.From.Y ? line.To : line.From;
            Point lowPoint = line.To.Y < line.From.Y ? line.To : line.From;

            if (highPoint.X < lowPoint.X)
            {
                return "a3";
            }

            return "a2";
        }

        /// <summary>
        ///     Class for rule arguments check result
        /// </summary>
        private class ContainingRuleArgumentsResult
        {
            /// <summary>
            ///     Elements
            /// </summary>
            public ConcurrentBag<Element> Elements { get; set; }

            /// <summary>
            ///     Is element found
            /// </summary>
            public bool IsElementFound { get; set; }
        }
    }
}