using Infracciones.App.Models;
using Infracciones.App.ViewModels;
using Infracciones.Models;
using System;

namespace Infracciones.ViewModels.Sanction
{
    public class SanctionViewModel : BaseViewModel
    {
        #region Fields & Properties
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; NotifyPropertyChanged(); }
        }

        private string _key;
        public string Key
        {
            get { return _key; }
            set { _key = value; NotifyPropertyChanged(); }
        }

        private string _plates;
        public string Plates
        {
            get { return _plates; }
            set { _plates = value; NotifyPropertyChanged(); }
        }

        private int _model;
        public int Model
        {
            get { return _model; }
            set { _model = value; NotifyPropertyChanged(); }
        }

        private string _responsible;
        public string Responsible
        {
            get { return _responsible; }
            set { _responsible = value; NotifyPropertyChanged(); }
        }

        private string _responsibleEmail;
        public string ResponsibleEmail
        {
            get { return _responsibleEmail; }
            set { _responsibleEmail = value; NotifyPropertyChanged(); }
        }

        private int _vehicleTypeId;
        public int VehicleTypeId
        {
            get { return _vehicleTypeId; }
            set { _vehicleTypeId = value; NotifyPropertyChanged(); }
        }

        private VehicleTypeModel _vehicleType;
        public VehicleTypeModel VehicleType
        {
            get { return _vehicleType; }
            set { _vehicleType = value; NotifyPropertyChanged(); }
        }

        private int _brandId;
        public int BrandId
        {
            get { return _brandId; }
            set { _brandId = value; NotifyPropertyChanged(); }
        }

        private BrandModel _brand;
        public BrandModel Brand
        {
            get { return _brand; }
            set { _brand = value; NotifyPropertyChanged(); }
        }

        private int _subBrandId;
        public int SubBrandId
        {
            get { return _subBrandId; }
            set { _subBrandId = value; NotifyPropertyChanged(); }
        }

        private SubBrandModel _subBrand;
        public SubBrandModel SubBrand
        {
            get { return _subBrand; }
            set { _subBrand = value; NotifyPropertyChanged(); }
        }

        private int _origin;
        public int Origin 
        {
            get { return _origin; }
            set { _origin = value; NotifyPropertyChanged(); }
        }

        private StateModel _state;
        public StateModel State
        {
            get { return _state; }
            set { _state = value; NotifyPropertyChanged(); }
        }

        private int _sanctionReasonId;
        public int SanctionReasonId 
        {
            get { return _sanctionReasonId; }
            set { _sanctionReasonId = value; NotifyPropertyChanged(); }
        }

        private SanctionReasonModel _sanctionReason;
        public SanctionReasonModel SanctionReason
        {
            get { return _sanctionReason; }
            set { _sanctionReason = value; NotifyPropertyChanged(); }
        }

        private string _officerId;
        public string OfficerId 
        {
            get { return _officerId; }
            set { _officerId = value; NotifyPropertyChanged(); }
        }

        private decimal _amount;
        public decimal Amount 
        {
            get { return _amount; }
            set { _amount = value; NotifyPropertyChanged(); }
        }

        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; NotifyPropertyChanged(); }
        }

        private bool _enabled;
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; NotifyPropertyChanged(); }
        }

        private string _createdBy;
        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; NotifyPropertyChanged(); }
        }

        private DateTime? _dateCreated;
        public DateTime? DateCreated
        {
            get { return _dateCreated; }
            set { _dateCreated = value; NotifyPropertyChanged(); }
        }

        private string _modifiedBy;
        public string ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = value; NotifyPropertyChanged(); }
        }

        private DateTime? _dateModified;
        public DateTime? DateModified
        {
            get { return _dateModified; }
            set { _dateModified = value; NotifyPropertyChanged(); }
        }

        private PaymentModel _payment;
        public PaymentModel Payment
        {
            get { return _payment; }
            set { _payment = value; NotifyPropertyChanged(); }
        }

        private LogModel _log;
        public LogModel Log
        {
            get { return _log; }
            set { _log = value; NotifyPropertyChanged(); }
        }

        #endregion

        #region Constructor

        public SanctionViewModel()
        {

        }

        #endregion


        #region Commands


        #endregion


        #region Methods


        #endregion
    }
}
