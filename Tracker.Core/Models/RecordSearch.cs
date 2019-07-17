﻿using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using Tracker.Core.StaticTypes;

namespace Tracker.Core.Models
{
    public class RecordSearch : BindableBase
    {
        //TODO Handle the AdditionalRequestorDBField
        //TODO Create Processor field for employees
        //RSID Info
        private int _id;
        private string _rsTypePrefix;
        private string _rsYear;
        private int _rsEnumeration;
        private string _rsSuffix;
        //Dates
        private DateTime? _dateReceived = null;
        private DateTime? _dateEntered = null;
        private DateTime? _dateOfResponse;
        private DateTime? _dateBilled;
        private DateTime? _datePaid;
        private DateTime? _lastUpdated;
        // Requestor
        private int _requestorID;
        private Person _requestor;
        private int _clientID;
        private Client _client;
        private Address _mailingAddress;
        private bool _isRequestorSameAsBilling;
        private Address _billingAddress;
        // Meta
        private string _projectName;
        private string _rsType; //TODO make RSType type
        private bool _isPriority;
        private bool _isEmergency;
        private bool _isInHouse;
        private bool _isSpecial;
        private string _specialDetails;
        private string _processor;
        // Location
        private string _mainCounty; //TODO make County type
        private ObservableCollection<County> _additionalCounties = new ObservableCollection<County>();
        private string _plss;
        private int _acres;
        private int _linearMiles;
        //Results
        private string _resourcesInProject; //TODO make resource codes type
        private string _recommendation;
        private bool _isReportReceived;
        private string _encryptionPassword;
        //Fees
        private string _feeVersion;
        private int _feeID;
        private Fees.Fee _fee;
        private bool _isPrePaid;
        private decimal _discretionaryAdjustment;
        private string _adjustmentExplanation;
        private decimal _totalFee;
        //Billing Information
        private string _projectNumber;
        private string _invoiceNumber;
        private string _checkName;
        private string _checkNumber;
        //Utility
        private bool _isSelected;
        

        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        #region RSID Info
        public string RSTypePrefix
        {
            get { return _rsTypePrefix; }
            set { SetProperty(ref _rsTypePrefix, value); }
        }

        public string RSYear
        {
            get { return _rsYear; }
            set { SetProperty(ref _rsYear, value); }
        }

        public int RSEnumeration
        {
            get { return _rsEnumeration; }
            set { SetProperty(ref _rsEnumeration, value); }
        }

        public string RSSuffix
        {
            get { return _rsSuffix; }
            set { SetProperty(ref _rsSuffix, value); }
        }

        #endregion

        #region Dates
        public DateTime? DateReceived
        {
            get { return _dateReceived; }
            set { SetProperty(ref _dateReceived, value); }
        }

        public DateTime? DateEntered
        {
            get { return _dateEntered; }
            set { SetProperty(ref _dateEntered, value); }
        }

        public DateTime? DateOfResponse
        {
            get { return _dateOfResponse; }
            set { SetProperty(ref _dateOfResponse, value); }
        }

        public DateTime? DateBilled
        {
            get { return _dateBilled; }
            set { SetProperty(ref _dateBilled, value); }
        }

        public DateTime? DatePaid
        {
            get { return _datePaid; }
            set { SetProperty(ref _datePaid, value); }
        }

        public DateTime? LastUpdated
        {
            get { return _lastUpdated; }
            set { SetProperty(ref _lastUpdated, value); }
        }

        #endregion

        #region Requestor

        public int RequestorID
        {
            get { return _requestorID; }
            set { SetProperty(ref _requestorID, value); }
        }

        public Person Requestor
        {       
            get { return _requestor; }
            set { SetProperty(ref _requestor, value); }
        }

        public int ClientID
        {
            get { return _clientID; }
            set { SetProperty(ref _clientID, value); }
        }

        public Client ClientModel
        {
            get { return _client; }
            set { SetProperty(ref _client, value); }
        }

        public Address MailingAddress
        {
            get { return _mailingAddress; }
            set {
                SetProperty(ref _mailingAddress, value);
                if (IsMailingSameAsBilling)
                    BillingAddress = value;
            }
        }

        public bool IsMailingSameAsBilling
        {
            get { return _isRequestorSameAsBilling; }
            set {
                SetProperty(ref _isRequestorSameAsBilling, value);
                if (value)
                    BillingAddress = MailingAddress;
                else
                    BillingAddress = new Address();
            }
        }

        public Address BillingAddress
        {
            get { return _billingAddress; }
            set { SetProperty(ref _billingAddress, value); }
        }
        #endregion

        #region Meta
        public string ProjectName
        {
            get { return _projectName; }
            set { SetProperty(ref _projectName, value); }
        }

        public string RSType
        {
            get { return _rsType; }
            set { SetProperty(ref _rsType, value); }
        }

        public string Processor
        {
            get { return _processor; }
            set { SetProperty(ref _processor, value); }
        }

        public bool IsPriority
        {
            get { return _isPriority; }
            set { SetProperty(ref _isPriority, value); }
        }

        public bool IsEmergency
        {
            get { return _isEmergency; }
            set { SetProperty(ref _isEmergency, value); }
        }

        public bool IsInHouse
        {
            get { return _isInHouse; }
            set { SetProperty(ref _isInHouse, value); }
        }

        public bool Special
        {
            get { return _isSpecial; }
            set { SetProperty(ref _isSpecial, value); }
        }

        public string SpecialDetails
        {
            get { return _specialDetails; }
            set { SetProperty(ref _specialDetails, value); }
        }

        #endregion

        #region Location

        public string MainCounty
        {
            get { return _mainCounty; }
            set { SetProperty(ref _mainCounty, value); }
        }

        public ObservableCollection<County> AdditionalCounties
        {
            get { return _additionalCounties; }
            set { SetProperty(ref _additionalCounties, value); }
        }

        public string PLSS
        {
            get { return _plss; }
            set { SetProperty(ref _plss, value); }
        }

        public int Acres
        {
            get { return _acres; }
            set { SetProperty(ref _acres, value); }
        }

        public int LinearMiles
        {
            get { return _linearMiles; }
            set { SetProperty(ref _linearMiles, value); }
        }
        #endregion

        #region Results
        public string ResourcesInProject
        {
            get { return _resourcesInProject; }
            set { SetProperty(ref _resourcesInProject, value); }
        }

        public string Recommendation
        {
            get { return _recommendation; }
            set { SetProperty(ref _recommendation, value); }
        }

        public bool IsReportReceived
        {
            get { return _isReportReceived; }
            set { SetProperty(ref _isReportReceived, value); }
        }
        #endregion

        #region Fees
        public string FeeVersion
        {
            get { return _feeVersion; }
            set { SetProperty(ref _feeVersion, value); }
        }

        public int FeeID
        {
            get { return _feeID; }
            set { SetProperty(ref _feeID, value); }
        }

        public bool IsPrePaid
        {
            get { return _isPrePaid; }
            set { SetProperty(ref _isPrePaid, value); }
        }

        public decimal DiscretionaryAdjustment
        {
            get { return _discretionaryAdjustment; }
            set { SetProperty(ref _discretionaryAdjustment, value); }
        }

        public string AdjustmentExplanation
        {
            get { return _adjustmentExplanation; }
            set { SetProperty(ref _adjustmentExplanation, value); }
        }

        public decimal TotalFee
        {
            get { return _totalFee; }
            set { SetProperty(ref _totalFee, value); }
        }

        #endregion

        #region Billing Information
        public string ProjectNumber
        {
            get { return _projectNumber; }
            set { SetProperty(ref _projectNumber, value); }
        }

        public string InvoiceNumber
        {
            get { return _invoiceNumber; }
            set { SetProperty(ref _invoiceNumber, value); }
        }

        public string CheckName
        {
            get { return _checkName; }
            set { SetProperty(ref _checkName, value); }
        }

        public string CheckNumber
        {
            get { return _checkNumber; }
            set { SetProperty(ref _checkNumber, value); }
        }
        #endregion

        public string EncryptionPassword
        {
            get { return _encryptionPassword; }
            set { SetProperty(ref _encryptionPassword, value); }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        public Fees.Fee Fee
        {
            get { return _fee; }
            set { SetProperty(ref _fee, value); }
        }

        public RecordSearch()
        {

        }
    }
}
