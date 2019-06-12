using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookAddin.Domain
{
    public class FacilityBookingDetailSearchCriteria
    {
        public int tenantId { get; set; }
        public int tenantUserId { get; set; }
        public int? condoFacilityId { get; set; }
        public int? condoFacilityGroupId { get; set; }
        public int? timeSlotConfigid { get; set; }
        public int? id { get; set; }
        public int? excludeId { get; set; }
        public int? excludeFacilityBookingId { get; set; }
        public string bookedBy { get; set; }
        public string similarBookedBy { get; set; }
        public string similarName { get; set; }
        public string similarEmail { get; set; }
        public string similarMobile { get; set; }
        public string similarNameOrEmailOrMobile { get; set; }
        public string status { get; set; }
        public string requestNo { get; set; }
        public long? from { get; set; }
        public long? to { get; set; }
        public long? fromDateFrom { get; set; }
        public long? fromDateTo { get; set; }
        public long? toDateFrom { get; set; }
        public long? toDateTo { get; set; }
        public long? cancelledDateFrom { get; set; }
        public long? cancelledDateTo { get; set; }
        public bool? isBlocking { get; set; }
        public bool? isCurrent { get; set; }
        public int? condoId { get; set; }
        public int? minimumDepositAmount { get; set; }
    }

    public class FacilitySearchCriteria
    {
        public int? facilityGroupId { get; set; }
        public int? id { get; set; }
        public int? excludeId { get; set; }
        public string name { get; set; }
        public string similarName { get; set; }
        public bool? isAllowGathering { get; set; }
        public bool? isActive { get; set; }
        public string code { get; set; }
        public int? tenantId { get; set; }
    }

    public class Tenant
    {
        public int version { get; set; }
        public int id { get; set; }
        public long? createdDate { get; set; }
        public string createdBy { get; set; }
        public long? updatedDate { get; set; }
        public string updatedBy { get; set; }
        public int? totalUser { get; set; }
        public int? totalAccountedUser { get; set; }
        public int? blockNo { get; set; }
        public int? floorNo { get; set; }
        public int? unitNo { get; set; }
        public int? type { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string directoryName { get; set; }
        public bool enabled { get; set; }
        public long? disableDate { get; set; }
        public int? condoId { get; set; }
        public string condoName { get; set; }
        public string condoCode { get; set; }
        public string estateType { get; set; }
        public string unitCode { get; set; }
        public string unitType { get; set; }
    }

    public class Facility : ABaseViewModel
    {
        public int version { get; set; }
        public int id { get; set; }
        public long? createdDate { get; set; }
        public string createdBy { get; set; }
        public long? updatedDate { get; set; }
        public string updatedBy { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public bool? active { get; set; }
        public string rateDtType { get; set; }
        public string imageFilePath { get; set; }
        public string fullImageFilePath { get; set; }
        public decimal? latestRate { get; set; }
        public FacilityGroup facilityGroup { get; set; }
    }

    public class FacilityBookingOrder
    {
        public int version { get; set; }
        public int id { get; set; }
        public long? createdDate { get; set; }
        public string createdBy { get; set; }
        public long? updatedDate { get; set; }
        public string updatedBy { get; set; }
        public string paymentUrl { get; set; }
        public string paymentENetsUrl { get; set; }
        public string paymentCCUrl { get; set; }
        public long? orderDate { get; set; }
        public string refNo { get; set; }
        public string paymentStatus { get; set; }
        public string paymentType { get; set; }
        public string bookedBaseAmount { get; set; }
        public string bookedGstAmount { get; set; }
        public decimal? refundableBaseAmount { get; set; }
        public decimal? refundableGstAmount { get; set; }
        public List<Payment> payments { get; set; }
    }

    public class Payment
    {

    }

    public class FacilityGroup
    {
        public int version { get; set; }
        public int id { get; set; }
        public long? createdDate { get; set; }
        public string createdBy { get; set; }
        public long? updatedDate { get; set; }
        public string updatedBy { get; set; }
        public string name { get; set; }
        public bool? allowGathering { get; set; }
        public string termsAndConditions { get; set; }
    }

    public class FacilityBooking
    {
        public int version { get; set; }
        public int id { get; set; }
        public long? createdDate { get; set; }
        public string createdBy { get; set; }
        public long? updatedDate { get; set; }
        public string updatedBy { get; set; }
        public string requestNo { get; set; }
        public string bookedBy { get; set; }
        public string paymentType { get; set; }
        public string status { get; set; }
        public long? approvedTime { get; set; }
        public long? expiredTime { get; set; }
        public long? requestedStartDate { get; set; }
        public long? requestedEndDate { get; set; }
        public Tenant tenant { get; set; }
        public Facility facility { get; set; }
        public FacilityBookingOrder facilityBookingOrder { get; set; }
        public bool penaltyCharge { get; set; }
        public bool? isExpired { get; set; }
        public bool? isResident { get; set; }
        public string contactName { get; set; }
        public string contactNo { get; set; }
        public string contactEmail { get; set; }
        public string contactCompany { get; set; }
        public string remark { get; set; }
        public string requestRemark { get; set; }
        public string eventType { get; set; }
        public string refNo { get; set; }
        public string totalBookedHour { get; set; }
        public string totalCancelledHour { get; set; }
        public string rates { get; set; }
        public string gstRates { get; set; }
        public string totalBaseAmount { get; set; }
        public string gstAmount { get; set; }
        public long? requestDate { get; set; }
        public string totalAmount { get; set; }
        public string statusDesc { get; set; }
        public string cancelledRemark { get; set; }
    }

    public class TimeSlotConfig
    {
        public int version { get; set; }
        public int id { get; set; }
        public long? createdDate { get; set; }
        public string createdBy { get; set; }
        public long? updatedDate { get; set; }
        public string updatedBy { get; set; }
        public string name { get; set; }
        public string effectiveFrom { get; set; }
        public string effectiveTo { get; set; }
        public long? from { get; set; }
        public long? to { get; set; }
        public int? slotDuration { get; set; }
    }

    public class BookingDetail
    {
        public int version { get; set; }
        public int id { get; set; }
        public long? createdDate { get; set; }
        public string createdBy { get; set; }
        public long? updatedDate { get; set; }
        public string updatedBy { get; set; }
        public string requestNo { get; set; }
        public string status { get; set; }
        public long? from { get; set; }
        public long? to { get; set; }
        public long? selectedDate { get; set; }
        public string cancelledBy { get; set; }
        public long? cancelledTime { get; set; }
        public string cancelledRemark { get; set; }
        public string detailBaseAmount { get; set; }
        public int? detailRefundableBaseAmount { get; set; }
        public int? detailRefundableGstAmount { get; set; }
        public int? detailCancelledGstRate { get; set; }
        public FacilityBooking facilityBooking { get; set; }
        public int? fromTimeSlotConfigid { get; set; }
        public string fromTimeSlotConfigDesc { get; set; }
        public int? toTimeSlotConfigid { get; set; }
        public string toTimeSlotConfigDesc { get; set; }
        public bool? penaltyCharge { get; set; }
        public TimeSlotConfig fromTimeSlotConfig { get; set; }
        public TimeSlotConfig toTimeSlotConfig { get; set; }
        public string statusDesc { get; set; }
    }

    public class Condo
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string estateType { get; set; }
    }

    public class Device
    {
        public string deviceToken { get; set; }
        public string notificationService { get; set; }
        public string devicePlatform { get; set; }
    }

    public class Account
    {
        public int id { get; set; }
        public string userName { get; set; }
    }

    public class Authentication
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string fullName { get; set; }
        public string accessType { get; set; }
    }
}
