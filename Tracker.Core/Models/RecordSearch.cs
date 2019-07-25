using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using Tracker.Core.StaticTypes;

namespace Tracker.Core.Models
{
    public class RecordSearch : BindableBase
    {
        private int _id;
        //IC FileNumber
        private string _icTypePrefix;
        private string _icYear;
        private int _icEnumeration;
        private string _icSuffix;
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
        private string _additionalRequestors;
        private int _clientID;
        private Client _client;
        private Address _mailingAddress;
        private bool _isRequestorSameAsBilling;
        private Address _billingAddress;
        // Meta
        private string _projectName;
        private string _rsType; //TODO make RSType type
        private string _status;
        private string _specialDetails;
        // Location
        private string _mainCounty; //TODO make County type
        private ObservableCollection<County> _additionalCounties = new ObservableCollection<County>() { Counties.BUTTE };
        private string _plss;
        private int _acres;
        private int _linearMiles;
        //Results
        private bool _areResourcesInProject;
        private string _recommendation;
        private bool _isReportReceived;
        private string _processor;
        private string _encryptionPassword;
        //Fees
        private string _feeVersion;
        private int _feeID;
        private Fees.Fee _fee;
        private bool _isPrePaid;
        private decimal _totalFee;
        //Billing Information
        private string _projectNumber;
        private string _invoiceNumber;
        private string _checkName;
        private string _checkNumber;
        //Utility
        private bool _isSelected;
        private string _notes;


        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        #region IC File Number Info
        public string ICTypePrefix
        {
            get { return _icTypePrefix; }
            set { SetProperty(ref _icTypePrefix, value); }
        }

        public string ICYear
        {
            get { return _icYear; }
            set { SetProperty(ref _icYear, value); }
        }

        public int ICEnumeration
        {
            get { return _icEnumeration; }
            set { SetProperty(ref _icEnumeration, value); }
        }

        public string ICSuffix
        {
            get { return _icSuffix; }
            set { SetProperty(ref _icSuffix, value); }
        }

        #endregion

        #region Dates
        public DateTime? DateReceived
        {
            get { return _dateReceived; }
            set
            {
                SetProperty(ref _dateReceived, value);
                Status = CalculateStatus();
            }
        }

        public DateTime? DateEntered
        {
            get { return _dateEntered; }
            set
            {
                SetProperty(ref _dateEntered, value);
                Status = CalculateStatus();
            }
        }

        public DateTime? DateOfResponse
        {
            get { return _dateOfResponse; }
            set
            {
                SetProperty(ref _dateOfResponse, value);
                Status = CalculateStatus();
            }
        }

        public DateTime? DateBilled
        {
            get { return _dateBilled; }
            set
            {
                SetProperty(ref _dateBilled, value);
                Status = CalculateStatus();
            }
        }

        public DateTime? DatePaid
        {
            get { return _datePaid; }
            set
            {
                SetProperty(ref _datePaid, value);
                Status = CalculateStatus();
            }
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

        public string AdditionalRequestors
        {
            get { return _additionalRequestors; }
            set { SetProperty(ref _additionalRequestors, value); }
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
            set
            {
                SetProperty(ref _mailingAddress, value);
                if (IsMailingSameAsBilling)
                    BillingAddress = value;
            }
        }

        public bool IsMailingSameAsBilling
        {
            get { return _isRequestorSameAsBilling; }
            set
            {
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

        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
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
        public bool AreResourcesInProject
        {
            get { return _areResourcesInProject; }
            set { SetProperty(ref _areResourcesInProject, value); }
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
        public string EncryptionPassword
        {
            get { return _encryptionPassword; }
            set { SetProperty(ref _encryptionPassword, value); }
        }
        public string Processor
        {
            get { return _processor; }
            set { SetProperty(ref _processor, value); }
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

        public Fees.Fee Fee
        {
            get { return _fee; }
            set { SetProperty(ref _fee, value); }
        }

        public bool IsPrePaid
        {
            get { return _isPrePaid; }
            set { SetProperty(ref _isPrePaid, value); }
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

        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        public string Notes
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value); }
        }

        //Constructor
        public RecordSearch()
        {

        }

        //Method
        private string CalculateStatus()
        {
            DateTime today = DateTime.Now;

            if (DatePaid != null)
            {
                return "Complete";
            }
            else if (DateBilled != null)
            {
                if ((today - DateBilled.Value).TotalDays < 60)
                    return "Awaiting Payment";
                else
                    return "Overdue Payment";
            }
            else if (DateOfResponse != null)
            {
                return "Awaiting Billing";
            }
            else if (DateReceived != null)
            {
                if ((today - DateReceived.Value).TotalDays < 60)
                    return "Awaiting Response";
                else
                    return "Overdue Response";
            }
            return "Entered";
        }
    }
}
