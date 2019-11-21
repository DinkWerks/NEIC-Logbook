using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tracker.Core.StaticTypes;

namespace Tracker.Core.Models
{
    public class Project : BindableBase
    {
        private int _id;
        // IC File Number
        private string _icTypePrefix;
        private string _icYear;
        private int _icEnumeration;
        private string _icSuffix;
        // Dates
        private DateTime? _dateReceived = null;
        private DateTime? _dateEntered = null;
        private DateTime? _dateOfResponse;
        private DateTime? _dateBilled;
        private DateTime? _datePaid;
        private DateTime? _lastUpdated;
        // Requestor
        private Person _requestor;
        private string _additionalRequestors;
        private Organization _client;
        private Address _mailingAddress;
        private Address _billingAddress;
        private bool _isRequestorSameAsBilling;
        // Meta
        private string _projectName;
        private string _projectType;
        private string _status;
        private string _specialDetails;
        // Location
        private County _mainCounty;
        private ObservableCollection<County> _additionalCounties = new ObservableCollection<County>();
        private string _plss;
        private decimal _acres;
        private decimal _linearMiles;
        // Results
        private bool _areResourcesInProject;
        private string _recommendation;
        private bool _isReportReceived;
        private Staff _processor;
        private string _encryptionPassword;
        // Fees
        private string _feeVersion;
        private int _feeID;
        private FeeData _feeData;
        private FeeX _fee;
        private bool _isPrePaid;
        private decimal _totalFee;
        // Billing Information
        private ProjectNumber _projectNumber;
        private string _invoiceNumber;
        private string _checkName;
        private string _checkNumber;

        //Properties
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        #region IC File Number Info
        [MaxLength(1)]
        public string ICTypePrefix
        {
            get { return _icTypePrefix; }
            set { SetProperty(ref _icTypePrefix, value); }
        }

        [MaxLength(2)]
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

        [MaxLength(3)]
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

        //Gets Converterted to RequestorID in DB
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

        //Gets Converterted to RequestorID in DB
        public Organization Client
        {
            get { return _client; }
            set { SetProperty(ref _client, value); }
        }

        //Gets Converted to Individual Fields in DB
        public Address MailingAddress
        {
            get { return _mailingAddress; }
            set
            {
                SetProperty(ref _mailingAddress, value);
                if (IsMailingSameAsBilling)
                    BillingAddress = new Address(value);
            }
        }

        public Address BillingAddress
        {
            get { return _billingAddress; }
            set { SetProperty(ref _billingAddress, value); }
        }

        public bool IsMailingSameAsBilling
        {
            get { return _isRequestorSameAsBilling; }
            set
            {
                SetProperty(ref _isRequestorSameAsBilling, value);
                if (value)
                    BillingAddress = new Address(MailingAddress);
                else
                    BillingAddress = new Address();
            }
        }
        #endregion

        #region Meta
        public string ProjectName
        {
            get { return _projectName; }
            set { SetProperty(ref _projectName, value); }
        }

        [MaxLength(100)]
        public string ProjectType
        {
            get { return _projectType; }
            set { SetProperty(ref _projectType, value); }
        }

        [MaxLength(100)]
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
        
        public County MainCounty
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

        public decimal Acres
        {
            get { return _acres; }
            set { SetProperty(ref _acres, value); }
        }

        public decimal LinearMiles
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

        [MaxLength(1000)]
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

        public Staff Processor
        {
            get { return _processor; }
            set { SetProperty(ref _processor, value); }
        }
        #endregion

        #region Fees
        [MaxLength(50)]
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

        public FeeData FeeData
        {
            get { return _feeData; }
            set { SetProperty(ref _feeData, value); }
        }

        [NotMapped]
        public FeeX Fee
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
        public ProjectNumber ProjectNumber
        {
            get { return _projectNumber; }
            set { SetProperty(ref _projectNumber, value); }
        }

        [MaxLength(50)]
        public string InvoiceNumber
        {
            get { return _invoiceNumber; }
            set { SetProperty(ref _invoiceNumber, value); }
        }

        [MaxLength(100)]
        public string CheckName
        {
            get { return _checkName; }
            set { SetProperty(ref _checkName, value); }
        }

        [MaxLength(50)]
        public string CheckNumber
        {
            get { return _checkNumber; }
            set { SetProperty(ref _checkNumber, value); }
        }
        #endregion

        //Constructor
        public Project()
        {

        }

        //Method
        public string CalculateStatus()
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
            else if (DateEntered != null)
            {
                return "Entered";
            }
            else
                return string.Empty;
        }

        public string GetFileNumberFormatted()
        {
            if (ICSuffix == null)
                return ICTypePrefix + ICYear + "-" + ICEnumeration;
            else
                return ICTypePrefix + ICYear + "-" + ICEnumeration + ICSuffix;
        }

        public void GenerateFee()
        {
            Fee = new FeeX(FeeVersion, FeeData ?? new FeeData());
        }

        public bool ValidateCompleteness()
        {
            if (Requestor != null && Client != null)
            {
                return true;
            }
            return false;
        }
    }
}
