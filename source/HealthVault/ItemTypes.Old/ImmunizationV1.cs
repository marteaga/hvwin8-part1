// Copyright(c) Microsoft Corporation.
// This content is subject to the Microsoft Reference Source License,
// see http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
// All other rights reserved.


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Microsoft.Health.ItemTypes.Old
{
    /// <summary>
    /// Represents health record item type that encapsulates an immunization.
    /// </summary>
    /// <remarks>
    /// Note: Please use the new version of this data type instead of this version.
    /// </remarks>
    /// 
    public class ImmunizationV1 : HealthRecordItem
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ImmunizationV1"/> class with 
        /// default values.
        /// </summary>
        /// 
        /// <remarks>
        /// The item is not added to the health record until the
        /// <see cref="Microsoft.Health.HealthRecordAccessor.NewItem(HealthRecordItem)"/> method 
        /// is called.
        /// </remarks>
        /// 
        public ImmunizationV1()
            : base(TypeId)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ImmunizationV1"/> class 
        /// specifying the mandatory values.
        /// </summary>
        /// 
        /// <param name="name">
        /// The name of the immunization.
        /// </param>
        /// 
        /// <param name="dateAdministrated">
        /// The approximate date that the immunization was adminstrated.
        /// </param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="name"/> or <paramref name="dateAdministrated"/> parameter is null.
        /// </exception>
        /// 
        public ImmunizationV1(CodableValue name, ApproximateDateTime dateAdministrated)
            : base(TypeId)
        {
            this.Name = name;
            this.DateAdministrated = dateAdministrated;
        }        

        /// <summary>
        /// Creates a new instance of the <see cref="ImmunizationV1"/> class 
        /// specifying the mandatory values.
        /// </summary>
        /// 
        /// <param name="name">
        /// The name of the immunization.
        /// </param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="name"/> parameter is null.
        /// </exception>
        /// 
        public ImmunizationV1(CodableValue name)
            : base(TypeId)
        {
            this.Name = name;
        }        

        /// <summary>
        /// Retrieves the unique identifier for the item type.
        /// </summary>
        /// 
        /// <value>
        /// A GUID.
        /// </value>
        /// 
        public new static readonly Guid TypeId =
            new Guid("3d817dbe-af42-4a9d-a553-d1298b4d08fc");

        /// <summary>
        /// Populates this <see cref="ImmunizationV1"/> instance from the data in the XML.
        /// </summary>
        /// 
        /// <param name="typeSpecificXml">
        /// The XML to get the immunization data from.
        /// </param>
        /// 
        /// <exception cref="InvalidOperationException">
        /// The first node in <paramref name="typeSpecificXml"/> is not
        /// an immunization node.
        /// </exception>
        /// 
        protected override void ParseXml(IXPathNavigable typeSpecificXml)
        {
            XPathNavigator itemNav =
                typeSpecificXml.CreateNavigator().SelectSingleNode(
                    "immunization");

            Validator.ThrowInvalidIfNull(itemNav, "ImmunizationUnexpectedNode");

            // <name>
            _name.ParseXml(itemNav.SelectSingleNode("name"));

            // <administration-date>
            _dateAdministrated =
                XPathHelper.GetOptNavValue<ApproximateDateTime>(
                    itemNav,
                    "administration-date");

            // <administrator>
            _administrator =
                XPathHelper.GetOptNavValue<PersonItem>(
                    itemNav,
                    "administrator");
            
            // <manufacturer>
            _manufacturer = 
                XPathHelper.GetOptNavValue(itemNav, "manufacturer");
            
            // <lot>
            _lot = XPathHelper.GetOptNavValue(itemNav, "lot");
            
            // <route>
            _route =
                XPathHelper.GetOptNavValue<CodableValue>(
                    itemNav,
                    "route");
            
            // <expiration-date> approx-date-time
            _expirationDate =
                XPathHelper.GetOptNavValue<ApproximateDate>(
                    itemNav,
                    "expiration-date");

            // <sequence>
            _sequence =
                XPathHelper.GetOptNavValue(itemNav, "sequence");
            
            // <anatomic-surface>
            _anatomicSurface =
                XPathHelper.GetOptNavValue<CodableValue>(
                    itemNav,
                    "anatomic-surface");
            
            // <adverse-event> string
            _adverseEvent =
                XPathHelper.GetOptNavValue(itemNav, "adverse-event");

            // <consent>
            _consent =
                XPathHelper.GetOptNavValue(itemNav, "consent");
        }

        /// <summary>
        /// Writes the immunization data to the specified XmlWriter.
        /// </summary>
        /// 
        /// <param name="writer">
        /// The XmlWriter to write the immunization data to.
        /// </param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="writer"/> is <b>null</b>.
        /// </exception>
        /// 
        /// <exception cref="HealthRecordItemSerializationException">
        /// The <see cref="Name"/> or <see cref="DateAdministrated"/> has not been set.
        /// </exception>
        /// 
        public override void WriteXml(XmlWriter writer)
        {
            Validator.ThrowIfWriterNull(writer);
            Validator.ThrowSerializationIfNull(_name.Text, "ImmunizationNameNotSet");
            Validator.ThrowSerializationIfNull(_dateAdministrated, "ImmunizationDateAdministratedNotSet");

            // <immunization>
            writer.WriteStartElement("immunization");

            _name.WriteXml("name", writer);

            _dateAdministrated.WriteXml("administration-date", writer);

            // <administrator>
            XmlWriterHelper.WriteOpt<PersonItem>(
                writer,
                "administrator",
                Administrator);
            
            // <manufacturer>
            XmlWriterHelper.WriteOptString(
                writer,
                "manufacturer",
                _manufacturer);

            // <lot>
            XmlWriterHelper.WriteOptString(
                writer,
                "lot",
                _lot);

            // <route>
            XmlWriterHelper.WriteOpt<CodableValue>(
                writer,
                "route",
                Route);
            
            // <expiration-date>
            XmlWriterHelper.WriteOpt<ApproximateDate>(
                writer,
                "expiration-date",
                _expirationDate);
            
            // <sequence>
            XmlWriterHelper.WriteOptString(
                writer,
                "sequence",
                _sequence);

            // <anatomic-surface> 
            XmlWriterHelper.WriteOpt<CodableValue>(
                writer,
                "anatomic-surface",
                AnatomicSurface); 
            
            // <adverse-event>
            XmlWriterHelper.WriteOptString(
                writer,
                "adverse-event",
                _adverseEvent); 

            // <consent>
            XmlWriterHelper.WriteOptString(
                writer,
                "consent",
                _consent);

            // </immunization>
            writer.WriteEndElement();
        }

        /// <summary>
        /// Gets or sets the name of the immunization.
        /// </summary>
        /// 
        /// <value>
        /// A <see cref="CodableValue"/> instance representing the name.
        /// </value>
        /// 
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="value"/> parameter is null during set.
        /// </exception>
        /// 
        public CodableValue Name
        {
            get { return _name; }
            set
            {
                Validator.ThrowIfArgumentNull(value, "Name", "ImmunizationNameMandatory");
                _name = value;
            }
        }
        private CodableValue _name =  new CodableValue();
        
        /// <summary>
        /// Gets or sets the date the immunization was administrated.
        /// </summary>
        /// 
        /// <value>
        /// An <see cref="ApproximateDateTime"/> instance representing the date.
        /// </value> 
        /// 
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="value"/> parameter is null.
        /// </exception>
        /// 
        public ApproximateDateTime DateAdministrated
        {
            get { return _dateAdministrated; }
            set
            {
                Validator.ThrowIfArgumentNull(value, "DateAdministrated", "ImmunizationDateAdministratedMandatory");
                _dateAdministrated = value;
            }
        }
        private ApproximateDateTime _dateAdministrated;

        /// <summary>
        /// Gets or sets the person or organization that administered the 
        /// immunization.
        /// </summary>
        /// 
        /// <value>
        /// A <see cref="PersonItem"/> instance representing the person.
        /// </value> 
        /// 
        /// <remarks>
        /// Set the value to null if the administrator should not be stored.
        /// </remarks>
        /// 
        public PersonItem Administrator
        {
            get { return _administrator; }
            set { _administrator = value; }
        }
        private PersonItem _administrator;

        /// <summary>
        /// Gets or sets the manufacturer of the vaccine.
        /// </summary>
        /// 
        /// <value>
        /// A string representing the manufacturer.
        /// </value> 
        /// 
        /// <remarks>
        /// Set the value to null if the manufacturer should not be stored.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException">
        /// If <paramref name="value"/> contains only whitespace.
        /// </exception>
        /// 
        public string Manufacturer
        {
            get { return _manufacturer; }
            set 
            {
                Validator.ThrowIfStringIsWhitespace(value, "Manufacturer");
                _manufacturer = value;
            }
        }
        private string _manufacturer;

        /// <summary>
        /// Gets or sets the lot of the vaccine.
        /// </summary>
        /// 
        /// <value>
        /// A string representing the lot.
        /// </value> 
        /// 
        /// <remarks>
        /// Set the value to null if the lot should not be stored.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException">
        /// If <paramref name="value"/> contains only whitespace.
        /// </exception>
        /// 
        public string Lot
        {
            get { return _lot; }
            set 
            {
                Validator.ThrowIfStringIsWhitespace(value, "Lot");
                _lot = value;
            }
        }
        private string _lot;
        
        /// <summary>
        /// Gets or sets the medical route for the immunization.
        /// </summary>
        /// 
        /// <value>
        /// A <see cref="CodableValue"/> instance representing the route.
        /// </value> 
        /// 
        public CodableValue Route
        {
            get { return _route; }
            set { _route = value; }
        }
        private CodableValue _route;

        /// <summary>
        /// Gets or sets the expiration date for the vaccine.
        /// </summary>
        /// 
        /// <value>
        /// An <see cref="ApproximateDate"/> instance representing the date.
        /// </value> 
        /// 
        public ApproximateDate ExpirationDate
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }
        }
        private ApproximateDate _expirationDate;
        
        /// <summary>
        /// Gets or sets the sequence for the immunization.
        /// </summary>
        /// 
        /// <value>
        /// A string representing the sequence.
        /// </value>
        /// 
        /// <remarks>
        /// Set the value to null if the consent should not be stored.
        /// </remarks>
        /// 
        public string Sequence
        {
            get { return _sequence; }
            set 
            {
                Validator.ThrowIfStringIsWhitespace(value, "Sequence");
                _sequence = value;
            }
        }
        private string _sequence;
        
        /// <summary>
        /// Gets or sets the anatomic surface for the immunization.
        /// </summary>
        /// 
        /// <value>
        /// A <see cref="CodableValue"/> instance representing the surface.
        /// </value>
        /// 
        public CodableValue AnatomicSurface
        {
            get { return _anatomicSurface; }
            set { _anatomicSurface = value; }
        }
        private CodableValue _anatomicSurface;

        /// <summary>
        /// Gets or sets any adverse event description for the immunization.
        /// </summary>
        /// 
        /// <value>
        /// A string representing the event description.
        /// </value> 
        /// 
        /// <remarks>
        /// Set the value to null if the description should not be stored.
        /// </remarks>
        /// 
        public string AdverseEvent
        {
            get { return _adverseEvent; }
            set 
            {
                Validator.ThrowIfStringIsWhitespace(value, "AdverseEvent");
                _adverseEvent = value;
            }
        }
        private string _adverseEvent;

        /// <summary>
        /// Gets or sets the consent description for the immunization.
        /// </summary>
        /// 
        /// <value>
        /// A string representing the consent description.
        /// </value> 
        /// 
        /// <remarks>
        /// Set the value to null if the consent should not be stored.
        /// </remarks>
        /// 
        public string Consent
        {
            get { return _consent; }
            set 
            {
                Validator.ThrowIfStringIsWhitespace(value, "Consent");
                _consent = value;
            }
        }
        private string _consent;

        /// <summary>
        /// Gets a string representation of the immunization item.
        /// </summary>
        /// 
        /// <returns>
        /// A string representation of the immunization item.
        /// </returns>
        /// 
        public override string ToString()
        {
            return
                String.Format(
                    CultureInfo.InvariantCulture,
                    ResourceRetriever.GetResourceString(
                        "ImmunizationToStringFormat"),
                    Name.Text,
                    DateAdministrated.ToString());
        }
    }
}
