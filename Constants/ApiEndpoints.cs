namespace GadgetShop.Constants;

public static class ApiEndpoints
{
    public static class Auth
    {
        public const string Login = "api/Auth/Login";
        public const string RequestOtp = "api/Auth/RequestLoginOtp";
        public const string VerifyOtp = "api/Auth/VerifyLoginOtp";
    }
    public static class CustomerAccount
    {
        public const string Register = "api/gadget/CustomerAccount/Register";
        public const string GetMyProfile = "api/gadget/CustomerAccount/GetMyProfile";
        public const string UpdateMyProfile = "api/gadget/CustomerAccount/UpdateMyProfile";
        public const string ChangePassword = "api/gadget/CustomerAccount/ChangePassword";
        public const string ForgotPassword = "api/gadget/CustomerAccount/ForgotPassword";
        public const string DeactivateMyAccount = "api/gadget/CustomerAccount/DeactivateMyAccount";
    }
    public static class CustomerAddress
    {
        public const string SaveAddress = "api/gadget/CustomerAddress/SaveAddress";
        public const string DeleteAddress = "api/gadget/CustomerAddress/DeleteAddress";
        public const string GetMyAddresses = "api/gadget/CustomerAddress/GetMyAddresses";
    }
    public static class CustomerCatalog
    {
        public const string GetProductList = "api/gadget/CustomerCatalog/GetProductList";
        public const string GetProductById = "api/gadget/CustomerCatalog/GetProductById";
        public const string GetVariantById = "api/gadget/CustomerCatalog/GetVariantById";
    }
    public static class Cart
    {
        public const string GetMyCart = "api/gadget/Cart/GetMyCart";
        public const string AddItem = "api/gadget/Cart/AddItem";
        public const string UpdateItemQuantity = "api/gadget/Cart/UpdateItemQuantity";
        public const string RemoveItem = "api/gadget/Cart/RemoveItem";
        public const string ClearCart = "api/gadget/Cart/ClearCart";
        public const string ApplyCoupon = "api/gadget/Cart/ApplyCoupon";
        public const string RemoveCoupon = "api/gadget/Cart/RemoveCoupon";
    }
    public static class Wishlist
    {
        public const string Add = "api/gadget/Wishlist/Add";
        public const string Remove = "api/gadget/Wishlist/Remove";
        public const string GetMyWishlist = "api/gadget/Wishlist/GetMyWishlist";
        public const string MoveToCart = "api/gadget/Wishlist/MoveToCart";
    }
    public static class Order
    {
        public const string PlaceOrder = "api/gadget/Order/PlaceOrder";
        public const string GetMyOrderById = "api/gadget/Order/GetMyOrderById";
        public const string GetMyOrderList = "api/gadget/Order/GetMyOrderList";
        public const string CancelMyOrder = "api/gadget/Order/CancelMyOrder";
        public const string Reorder = "api/gadget/Order/Reorder";
        public const string GetMyInvoice = "api/gadget/Order/GetMyInvoice";
        public const string GetMyShipmentByOrderId = "api/gadget/Order/GetMyShipmentByOrderId";
        public const string GetMyReturnList = "api/gadget/Order/GetMyReturnList";
        // Admin order
        public const string GetOrderById = "api/gadget/Order/GetOrderById";
        public const string GetOrderList = "api/gadget/Order/GetOrderList";
        public const string UpdateOrderStatus = "api/gadget/Order/UpdateOrderStatus";
        public const string CreateShipment = "api/gadget/Order/CreateShipment";
        public const string UpdateShipmentStatus = "api/gadget/Order/UpdateShipmentStatus";
        public const string GetShipmentByOrderId = "api/gadget/Order/GetShipmentByOrderId";
        public const string GetShipmentList = "api/gadget/Order/GetShipmentList";
        public const string CreateReturn = "api/gadget/Order/CreateReturn";
        public const string GetReturnById = "api/gadget/Order/GetReturnById";
        public const string GetReturnList = "api/gadget/Order/GetReturnList";
        public const string UpdateReturnStatus = "api/gadget/Order/UpdateReturnStatus";
    }
    public static class Payment
    {
        public const string CreatePaymentOrder = "api/Payment/CreatePaymentOrder";
        public const string VerifyPayment = "api/Payment/VerifyPayment";
        public const string RetryPayment = "api/Payment/RetryPayment";
    }
    public static class PaymentAdmin
    {
        public const string GetPaymentById = "api/gadget/PaymentAdmin/GetPaymentById";
        public const string GetPaymentList = "api/gadget/PaymentAdmin/GetPaymentList";
        public const string UpdatePaymentStatus = "api/gadget/PaymentAdmin/UpdatePaymentStatus";
        public const string ProcessRefund = "api/gadget/PaymentAdmin/ProcessRefund";
    }
    public static class ProductReview
    {
        public const string Save = "api/gadget/ProductReview/Save";
        public const string Delete = "api/gadget/ProductReview/Delete";
        public const string GetByProduct = "api/gadget/ProductReview/GetByProduct";
    }
    public static class Notification
    {
        public const string GetMyNotifications = "api/gadget/CustomerNotification/GetMyNotifications";
        public const string MarkAsRead = "api/gadget/CustomerNotification/MarkAsRead";
    }
    public static class SupportTicket
    {
        public const string Create = "api/gadget/SupportTicket/Create";
        public const string Reply = "api/gadget/SupportTicket/Reply";
        public const string GetMyTickets = "api/gadget/SupportTicket/GetMyTickets";
    }
    // Admin
    public static class AdminDashboard
    {
        public const string GetSummary = "api/gadget/AdminDashboard/Summary";
        public const string GetDashboardOverview = "api/gadget/AdminDashboard/GetDashboardOverview";
        public const string GetDashboardReport = "api/gadget/AdminDashboard/GetDashboardReport";
    }
    public static class ProductMaster
    {
        public const string CreateProduct = "api/gadget/ProductMaster/CreateProduct";
        public const string UpdateProduct = "api/gadget/ProductMaster/UpdateProduct";
        public const string GetProductById = "api/gadget/ProductMaster/GetProductById";
        public const string GetProductList = "api/gadget/ProductMaster/GetProductList";
        public const string UpdateProductStatus = "api/gadget/ProductMaster/UpdateProductStatus";
        public const string UploadProductImage = "api/gadget/ProductMaster/UploadProductImage";
        public const string BulkUploadProducts = "api/gadget/ProductMaster/BulkUploadProducts";
        public const string GetProductGallery = "api/gadget/ProductMaster/GetProductGallery";
        public const string DeleteProductGalleryImage = "api/gadget/ProductMaster/DeleteProductGalleryImage";
        public const string SetPrimaryProductGalleryImage = "api/gadget/ProductMaster/SetPrimaryProductGalleryImage";
    }
    public static class CategoryMaster
    {
        public const string CreateCategory = "api/gadget/CategoryMaster/CreateCategory";
        public const string UpdateCategory = "api/gadget/CategoryMaster/UpdateCategory";
        public const string GetCategoryById = "api/gadget/CategoryMaster/GetCategoryById";
        public const string GetCategoryList = "api/gadget/CategoryMaster/GetCategoryList";
        public const string UpdateCategoryStatus = "api/gadget/CategoryMaster/UpdateCategoryStatus";
    }
    public static class BrandMaster
    {
        public const string CreateBrand = "api/gadget/BrandMaster/CreateBrand";
        public const string UpdateBrand = "api/gadget/BrandMaster/UpdateBrand";
        public const string GetBrandById = "api/gadget/BrandMaster/GetBrandById";
        public const string GetBrandList = "api/gadget/BrandMaster/GetBrandList";
        public const string UpdateBrandStatus = "api/gadget/BrandMaster/UpdateBrandStatus";
    }
    public static class CouponMaster
    {
        public const string CreateCoupon = "api/gadget/CouponMaster/CreateCoupon";
        public const string UpdateCoupon = "api/gadget/CouponMaster/UpdateCoupon";
        public const string GetCouponById = "api/gadget/CouponMaster/GetCouponById";
        public const string GetCouponList = "api/gadget/CouponMaster/GetCouponList";
        public const string UpdateCouponStatus = "api/gadget/CouponMaster/UpdateCouponStatus";
    }
    public static class AdminUser
    {
        public const string CreateUser = "api/gadget/AdminUser/CreateUser";
        public const string UpdateUser = "api/gadget/AdminUser/UpdateUser";
        public const string GetUserById = "api/gadget/AdminUser/GetUserById";
        public const string GetUserList = "api/gadget/AdminUser/GetUserList";
        public const string UpdateUserStatus = "api/gadget/AdminUser/UpdateUserStatus";
        public const string ArchiveUser = "api/gadget/AdminUser/ArchiveUser";
        public const string ResetPassword = "api/gadget/AdminUser/ResetPassword";
        public const string GetRoleOptions = "api/gadget/AdminUser/GetRoleOptions";
    }
    public static class ProductInventory
    {
        public const string StockIn = "api/gadget/ProductInventory/StockIn";
        public const string StockOut = "api/gadget/ProductInventory/StockOut";
        public const string GetTransactionList = "api/gadget/ProductInventory/GetTransactionList";
        public const string GetInventoryAdminReport = "api/gadget/ProductInventory/GetInventoryAdminReport";
    }
    public static class AuditLog
    {
        public const string GetAuditLogList = "api/AuditLog/GetAuditLogList";
        public const string GetAuditLogById = "api/AuditLog/GetAuditLogById";
    }
    public static class AdminOperations
    {
        public const string ReleaseExpiredReservations = "api/gadget/AdminOperations/ReleaseExpiredReservations";
        public const string RecalculateOrderTotals = "api/gadget/AdminOperations/RecalculateOrderTotals";
        public const string GetDataIntegrityCheck = "api/gadget/AdminOperations/GetDataIntegrityCheck";
    }
    public static class Invoice
    {
        public const string GenerateInvoice = "api/gadget/InvoiceAdmin/GenerateInvoice";
        public const string GetInvoiceById = "api/gadget/InvoiceAdmin/GetInvoiceById";
        public const string GetInvoiceList = "api/gadget/InvoiceAdmin/GetInvoiceList";
        public const string GetInvoicePrintDocument = "api/gadget/InvoiceAdmin/GetInvoicePrintDocument";
    }
}
