// Copyright(c) Microsoft Corporation.
// This content is subject to the Microsoft Reference Source License,
// see http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Microsoft.Health.ItemTypes
{
    /// <summary>
    /// Information related to a comment.
    /// </summary>
    ///
    public class Comment : HealthRecordItem
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Comment"/> class with default values.
        /// </summary>
        ///
        /// <remarks>
        /// This item is not added to the health record until the
        /// <see cref="Microsoft.Health.HealthRecordAccessor.NewItem(HealthRecordItem)"/> method
        /// is called.
        /// </remarks>
        ///
        public Comment()
            : base(TypeId)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Comment"/> class
        /// specifying mandatory values.
        /// </summary>
        ///
        /// <remarks>
        /// This item is not added to the health record until the
        /// <see cref="Microsoft.Health.HealthRecordAccessor.NewItem(HealthRecordItem)"/> method
        /// is called.
        /// </remarks>
        ///
        /// <param name="when">
        /// The date and time associated with the comment.
        /// </param>
        /// <param name="content">
        /// The text content of this health comment.
        /// </param>
        ///
        /// <exception cref="ArgumentException">
        /// If <paramref name="content"/> is <b>null</b>, empty or contains only whitespace.
        /// </exception>
        ///
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="when"/> is <b>null</b>.
        /// </exception>
        ///
        public Comment(
            ApproximateDateTime when,
            string content)
            : base(TypeId)
        {
            When = when;
            Content = content;
        }

        /// <summary>
        /// Retrieves the unique identifier for the item type.
        /// </summary>
        ///
        /// <value>
        /// A GUID.
        /// </value>
        ///
        public static new readonly Guid TypeId =
            new Guid("9f4e0fcd-10d7-416d-855a-90514ce2016b");

        /// <summary>
        /// Populates this <see cref="Comment"/> instance from the data in the specified XML.
        /// </summary>
        ///
        /// <param name="typeSpecificXml">
        /// The XML to get the comment data from.
        /// </param>
        ///
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="typeSpecificXml"/> parameter is <b>null</b>.
        /// </exception>
        /// 
        /// <exception cref="InvalidOperationException">
        /// If the first node in <paramref name="typeSpecificXml"/> is not
        /// a comment node.
        /// </exception>
        /// 
        protected override void ParseXml(IXPathNavigable typeSpecificXml)
        {
            Validator.ThrowIfArgumentNull(typeSpecificXml, "typeSpecificXml", "ParseXmlNavNull");

            XPathNavigator itemNav =
                typeSpecificXml.CreateNavigator().SelectSingleNode("comment");

            Validator.ThrowInvalidIfNull(itemNav, "CommentUnexpectedNode");

            _when = new ApproximateDateTime();
            _when.ParseXml(itemNav.SelectSingleNode("when"));
            _content = itemNav.SelectSingleNode("content").Value;
            _category = XPathHelper.GetOptNavValue<CodableValue>(itemNav, "category");
        }

        /// <summary>
        /// Writes the XML representation of the comment into
        /// the specified XML writer.
        /// </summary>
        ///
        /// <param name="writer">
        /// The XML writer into which the comment should be
        /// written.
        /// </param>
        ///
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="writer"/> parameter is <b>null</b>.
        /// </exception>
        /// 
        /// <exception cref="HealthRecordItemSerializationException">
        /// If <see cref="When"/> is <b>null</b>.
        /// If <see cref="Content"/> is <b>null</b> or empty or contains only whitespace.
        /// </exception>
        ///
        public override void WriteXml(XmlWriter writer)
        {
            Validator.ThrowIfWriterNull(writer);

            Validator.ThrowSerializationIfNull(_when, "WhenNullValue");
            Validator.ThrowSerializationIf(
                String.IsNullOrEmpty(_content) || String.IsNullOrEmpty(_content.Trim()),
                "CommentContentMandatory");

            writer.WriteStartElement("comment");

            _when.WriteXml("when", writer);
            writer.WriteElementString("content", _content);
            XmlWriterHelper.WriteOpt<CodableValue>(writer, "category", _category);

            writer.WriteEndElement();
        }

        /// <summary>
        /// Gets or sets the date and time associated with the comment.
        /// </summary>
        ///
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="value"/> parameter is <b>null</b>.
        /// </exception>
        /// 
        public ApproximateDateTime When
        {
            get
            {
                return _when;
            }

            set
            {
                Validator.ThrowIfArgumentNull(value, "value", "WhenNullValue");
                _when = value;
            }
        }

        private ApproximateDateTime _when;

        /// <summary>
        /// Gets or sets the text content of this comment.
        /// </summary>
        /// 
        /// <exception cref="ArgumentException">
        /// The <paramref name="value"/> parameter is null, empty or only whitespace.
        /// </exception>
        /// 
        public string Content
        {
            get
            {
                return _content;
            }

            set
            {
                Validator.ThrowIfStringNullOrEmpty(value, "Content");
                Validator.ThrowIfStringIsWhitespace(value, "Content");
                _content = value;
            }
        }

        private string _content;

        /// <summary>
        /// Gets or sets the category of the comment.
        /// </summary>
        /// 
        /// <remarks>
        /// The category can be used to group related comment entries together. For example, 'mental health'.
        /// If there is no information about category, the value should be set to <b>null</b>.
        /// </remarks>
        /// 
        public CodableValue Category
        {
            get { return _category; }

            set { _category = value; }
        }

        private CodableValue _category;

        /// <summary>
        /// Gets a string representation of the comment.
        /// </summary>
        /// 
        /// <returns>
        /// A string representation of the comment.
        /// </returns>
        ///
        public override string ToString()
        {
            StringBuilder result = new StringBuilder(200);

            result.Append(_content);

            if (_category != null)
            {
                result.Append(ResourceRetriever.GetSpace("resources"));
                result.Append(ResourceRetriever.GetResourceString("OpenParen"));
                result.Append(_category.Text);
                result.Append(ResourceRetriever.GetResourceString("CloseParen"));
            }

            return result.ToString();
        }
    }
}
