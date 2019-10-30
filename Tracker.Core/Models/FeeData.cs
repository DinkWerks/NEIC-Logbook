using Prism.Mvvm;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tracker.Core.Models
{
    public class FeeData : BindableBase
    {
        private int _id;
        private decimal _staffTime;
        private decimal _halfStaffTime;
        private decimal _inHouseTime;
        private decimal _staffAssistanceTime;
        private int _gisFeatures;
        private bool _isAddressedMappedFee;
        private int _DBRows;
        private int _quadsEntered;
        private bool _isPDFFee;
        private int _pdfPages;
        private int _printedPages;
        private decimal _adjustment;
        private string _adjustmentExplanation;
        private bool _isPriority;
        private bool _isEmergency;
        private bool _isRapidResponse;

        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        [Column(TypeName = "decimal(9, 3)")]
        public decimal StaffTime
        {
            get { return _staffTime; }
            set { SetProperty(ref _staffTime, value); }
        }

        [Column(TypeName = "decimal(9, 3)")]
        public decimal HalfStaffTime
        {
            get { return _halfStaffTime; }
            set { SetProperty(ref _halfStaffTime, value); }
        }

        [Column(TypeName = "decimal(9, 3)")]
        public decimal InHouseTime
        {
            get { return _inHouseTime; }
            set { SetProperty(ref _inHouseTime, value); }
        }

        [Column(TypeName = "decimal(9, 3)")]
        public decimal StaffAssistanceTime
        {
            get { return _staffAssistanceTime; }
            set { SetProperty(ref _staffAssistanceTime, value); }
        }

        public int GISFeatures
        {
            get { return _gisFeatures; }
            set { SetProperty(ref _gisFeatures, value); }
        }

        public bool IsAddressMappedFee
        {
            get { return _isAddressedMappedFee; }
            set { SetProperty(ref _isAddressedMappedFee, value); }
        }

        public int DBRows
        {
            get { return _DBRows; }
            set { SetProperty(ref _DBRows, value); }
        }

        public int QuadsEntered
        {
            get { return _quadsEntered; }
            set { SetProperty(ref _quadsEntered, value); }
        }

        public bool IsPDFFee
        {
            get { return _isPDFFee; }
            set { SetProperty(ref _isPDFFee, value); }
        }

        public int PDFPages
        {
            get { return _pdfPages; }
            set { SetProperty(ref _pdfPages, value); }
        }

        public int PrintedPages
        {
            get { return _printedPages; }
            set { SetProperty(ref _printedPages, value); }
        }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Adjustment
        {
            get { return _adjustment; }
            set { SetProperty(ref _adjustment, value); }
        }

        public string AdjustmentExplanation
        {
            get { return _adjustmentExplanation; }
            set { SetProperty(ref _adjustmentExplanation, value); }
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

        public bool IsRapidResponse
        {
            get { return _isRapidResponse; }
            set { SetProperty(ref _isRapidResponse, value); }
        }

        public int ProjectID { get; set; }
        public Project Project { get; set; }

        public FeeData()
        {

        }

        public decimal GetAsDecimal(string propname)
        {
            var t = GetType().GetProperty(propname).GetValue(this);
            if (t.GetType() == typeof(decimal))
                return (decimal)t;
            else if (t.GetType() == typeof(int))
                return (decimal)(int)t;
            else
                return 0;
        }

        public void SetFromDecimal(string propname, decimal value)
        {
            var t = GetType().GetProperty(propname).PropertyType;
            if (t == typeof(decimal))
                GetType().GetProperty(propname).SetValue(this, value);
            else if(t == typeof(int))
                GetType().GetProperty(propname).SetValue(this, decimal.ToInt32(value));
        }
    }
}
