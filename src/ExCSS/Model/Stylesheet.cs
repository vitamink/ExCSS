using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExCSS
{
    public sealed class Stylesheet : StylesheetNode
    {
        private readonly StylesheetParser _parser;

        internal Stylesheet(StylesheetParser parser)
        {
            _parser = parser;
            Rules = new RuleList(this);
        }

        public RuleList Rules { get; }

        public IEnumerable<CharsetRule> CharacterSetRules => Rules.Where(r => r is CharsetRule).Cast<CharsetRule>();
        public IEnumerable<FontFaceRule> FontfaceSetRules => Rules.Where(r => r is FontFaceRule).Cast<FontFaceRule>();
        public IEnumerable<MediaRule> MediaRules => Rules.Where(r => r is MediaRule).Cast<MediaRule>();
        public IEnumerable<ImportRule> ImportRules => Rules.Where(r => r is ImportRule).Cast<ImportRule>();
        public IEnumerable<NamespaceRule> NamespaceRules => Rules.Where(r => r is NamespaceRule).Cast<NamespaceRule>();
        public IEnumerable<PageRule> PageRules => Rules.Where(r => r is PageRule).Cast<PageRule>();
        public IEnumerable<StyleRule> StyleRules => Rules.Where(r => r is StyleRule).Cast<StyleRule>();

        public IRule Add(RuleType ruleType)
        {
            var rule = _parser.CreateRule(ruleType);
            Rules.Add(rule);
            return rule;
        }

        public void RemoveAt(int index)
        {
            Rules.RemoveAt(index);
        }

        public int Insert(string ruleText, int index)
        {
            var rule = _parser.ParseRule(ruleText);
            rule.Owner = this;
            Rules.Insert(index, rule);

            return index;
        }

        public override void ToCss(TextWriter writer, IStyleFormatter formatter)
        {
            writer.Write(formatter.Sheet(Rules));
        }
    }
}