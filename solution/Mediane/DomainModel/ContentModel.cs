using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Mediane.DomainModel
{
    public class Article
    {
        public string Title { get; private set; }
        public IArticleRepository Repository { get; private set; }

        public Article(string title, IArticleRepository repository)
        {
            Title = title.Trim();
            Repository = repository;
        }

        public string Rendered
        {
            get
            {
                return Render(Content);
            }
        }

        private string ContentValue;

        public string Content
        {
            get
            {
                return ContentValue;
            }

            set
            {
                if (value != null)
                {
                    ContentValue = value;
                }
            }
        }

        public string Render(string content)
        {
            var parser = new MediaWikiParser(content);
            parser.Parse();
            return parser.Result;
            //return "<p>" + content + "</p>";
        }

        public bool IsNew { get; set; }

        public IEnumerable<SelectListItem> AvailableTypes
        {
            get
            {
                var types = new List<SelectListItem>();
                types.Add(new SelectListItem { Text = "Page", Value = "Page" });
                types.Add(new SelectListItem { Text = "Category", Value = "Category" });
                return types;
            }
        }

    }

    class MediaWikiParser
    {
        const string BaseUrl = "/Home/Index/";
        private string Content;
        private StringBuilder Builder = new StringBuilder();
        int position = 0;

        public string Result { get { return Builder.ToString(); } }

        public MediaWikiParser(string content)
        {
            Content = content;
        }

        public void Parse()
        {
            Builder.Append("<p>");

            Token prevToken = new Token(TokenType.End);
            do
            {
                Token token = GetToken();
                switch (token.Type)
                {
                    case TokenType.Text:
                        Builder.Append(token.Text);
                        break;

                    case TokenType.Link:
                        Builder.Append("<a href=\"");
                        Builder.Append(BaseUrl);
                        Builder.Append(token.Text);
                        Builder.Append("\">");
                        Builder.Append(token.Text);
                        Builder.Append("</a>");
                        break;

                    case TokenType.EmptyLine:
                        Builder.Append("</p>\n<p>");
                        break;

                    case TokenType.End:
                        break;
                }

                prevToken = token;
            } while (prevToken.Type != TokenType.End);

            Builder.Append("</p>");
        }

        LexStateType LexState = LexStateType.Text;
        StringBuilder Word = new StringBuilder();
        private int NewLineCount;

        Char GetCh(int n = 0)
        {
            if ((position + n) >= Content.Length)
            {
                return '\0';
            }

            return Content[position + n];
        }

        Token GetToken()
        {
            if (position >= Content.Length)
            {
                return new Token(TokenType.End);
            }

            while (true)
            {
                Char ch = GetCh();
                ++position;

                switch (LexState)
                {
                    case LexStateType.Text:
                        switch (ch)
                        {
                            case '[':
                                if (GetCh() == '[')
                                {
                                    ++position;
                                    LexState = LexStateType.Link;
                                    if (Word.Length > 0)
                                    {
                                        return MakeToken(TokenType.Text);
                                    }

                                    continue;
                                }

                                break;

                            case '\r':
                                if (GetCh() != '\n')
                                {
                                    Word.Append(' ');
                                }

                                continue;

                            case '\n':
                                NewLineCount = 1;
                                LexState = LexStateType.NewLine;
                                if (Word.Length > 0)
                                {
                                    return MakeToken(TokenType.Text);
                                }

                                continue;
                        }

                        Word.Append(ch);
                        break;

                    case LexStateType.NewLine:
                        switch (ch)
                        {
                            case '\r':
                                break;

                            case '\n':
                                ++NewLineCount;
                                break;

                            default:
                                LexState = LexStateType.Text;
                                if (NewLineCount > 1)
                                {
                                    --position;
                                    return new Token(TokenType.EmptyLine);
                                }

                                Word.Append('\n');
                                Word.Append(ch);
                                break;
                        }

                        break;

                    case LexStateType.Link:
                        switch (ch)
                        {
                            case ']':
                                if (GetCh() == ']')
                                {
                                    ++position;
                                    LexState = LexStateType.Text;
                                    return MakeToken(TokenType.Link);
                                }

                                break;
                        }

                        Word.Append(ch);
                        break;
                }

                if (position < Content.Length)
                {
                    continue;
                }

                return new Token(TokenType.Text, Word.ToString());
            }
        }

        private Token MakeToken(TokenType type)
        {
            var result = new Token(type, Word.ToString());
            Word.Clear();
            return result;
        }
    }

    struct Token
    {
        public Token(TokenType type)
        {
            Type = type;
            Text = "";
        }

        public Token(TokenType type, string text)
        {
            Type = type;
            Text = text;
        }

        public TokenType Type;
        public string Text;
    }

    enum TokenType
    {
        End,
        Text,
        Link,
        EmptyLine,
    }

    enum LexStateType
    {
        Text,
        Link,
        NewLine,
    }
}