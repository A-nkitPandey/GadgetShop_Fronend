# Frontend / Backend API Integration Status

| Backend Module | API Endpoint/Constant | Frontend Page/Component | Service | Repository | Status | Notes |
|---|---|---|---|---|---|---|
| Auth | `ApiEndpoints.Auth.Login` | `Pages/Auth/Login.razor` | `AuthService` | `AuthRepository` | Integrated | Existing flow preserved |
| Auth | `ApiEndpoints.Auth.RefreshToken` | `Infrastructure/Authentication/AuthHeaderHandler.cs` | `AuthService` / `TokenRefreshCoordinator` | `AuthRepository` | Integrated | 401 refresh + one retry + logout fallback |
| Customer Notifications | `ApiEndpoints.Notification.GetMyNotifications` | `Pages/User/Notifications.razor` | `UserAccountService` | `UserAccountRepository` | Integrated | List + unread state |
| Customer Notifications | `ApiEndpoints.Notification.MarkAsRead` | `Pages/User/Notifications.razor` | `UserAccountService` | `UserAccountRepository` | Integrated | Single + mark-all loop |
| Customer Notifications | `ApiEndpoints.Notification.RegisterDevice` | None yet | `UserAccountService` | `UserAccountRepository` | Partial | Wired in services, no push-device UI because app has no push setup |
| Customer Notifications | `ApiEndpoints.Notification.RemoveDevice` | None yet | `UserAccountService` | `UserAccountRepository` | Partial | Wired in services, no push-device UI because app has no push setup |
| Support Ticket | `ApiEndpoints.SupportTicket.GetMyTickets` | `Pages/User/SupportTickets.razor` | `SupportTicketService` | `SupportTicketRepository` | Integrated | Thread list |
| Support Ticket | `ApiEndpoints.SupportTicket.Create` | `Pages/User/SupportTickets.razor` | `SupportTicketService` | `SupportTicketRepository` | Integrated | Create ticket form |
| Support Ticket | `ApiEndpoints.SupportTicket.Reply` | `Pages/User/SupportTickets.razor` | `SupportTicketService` | `SupportTicketRepository` | Integrated | Reply in thread |
| Admin Operations | `ApiEndpoints.AdminOperations.GetDataIntegrityCheck` | `Pages/Admin/AdminOperations.razor` | `AdminCatalogAdminService` | `AdminOperationsRepository` | Integrated | Real API now, no fake response |
| Admin Operations | `ApiEndpoints.AdminOperations.ReleaseExpiredReservations` | `Pages/Admin/AdminOperations.razor` | `AdminCatalogAdminService` | `AdminOperationsRepository` | Integrated | Confirmation + result summary |
| Admin Operations | `ApiEndpoints.AdminOperations.RecalculateOrderTotals` | `Pages/Admin/AdminOperations.razor` | `AdminCatalogAdminService` | `AdminOperationsRepository` | Integrated | Confirmation + result summary |
| Customer Invoice | `ApiEndpoints.Order.GetMyInvoice` | `Pages/User/OrderDetail.razor`, `Pages/User/OrderInvoice.razor` | `OrderService` | `OrderRepository` | Integrated | View + print |
| Customer Shipment | `ApiEndpoints.Order.GetMyShipmentByOrderId` | `Pages/User/OrderDetail.razor` | `OrderService` | `OrderRepository` | Integrated | Read-only display |
| Customer Returns | `ApiEndpoints.Order.GetMyReturnList` | `Pages/User/OrderDetail.razor` | `OrderService` | `OrderRepository` | Integrated | Read-only list for current order |
| Admin Invoice | `ApiEndpoints.Invoice.GetInvoiceList` | `Pages/Admin/AdminInvoices.razor` | `AdminCatalogAdminService` | `AdminOperationsRepository` | Integrated | Existing list retained |
| Admin Invoice | `ApiEndpoints.Invoice.GenerateInvoice` | `Pages/Admin/AdminInvoices.razor` | `AdminCatalogAdminService` | `AdminOperationsRepository` | Integrated | Existing action retained |
| Admin Invoice | `ApiEndpoints.Invoice.GetInvoiceById` | `Pages/Admin/InvoiceDetail.razor` | `AdminCatalogAdminService` | `AdminOperationsRepository` | Integrated | New detail page |
| Admin Invoice | `ApiEndpoints.Invoice.GetInvoicePrintDocument` | `Pages/Admin/InvoicePrint.razor` | `AdminCatalogAdminService` | `AdminOperationsRepository` | Integrated | Print view page |
| Payment | `Payment.Webhook` | None | None | None | Backend Only | Server-to-server callback only |
| Product Media Upload | `ApiEndpoints.ProductMaster.UploadProductImage` | `Pages/Admin/AdminProducts.razor`, `Pages/Admin/AdminProductMedia.razor` | `AdminProductService` | `AdminProductRepository` | Integrated | Primary upload in product form and media page |
| Product Media Upload | `ApiEndpoints.ProductMaster.UploadProductGalleryImage` | `Pages/Admin/AdminProductMedia.razor` | `AdminProductService` | `AdminProductRepository` | Integrated | Multi-select uploads one by one to backend |
| Product Media Upload | `ApiEndpoints.ProductMaster.GetProductGallery` | `Pages/Admin/AdminProductMedia.razor` | `AdminProductService` | `AdminProductRepository` | Integrated | Existing gallery display |
| Product Media Upload | `ApiEndpoints.ProductMaster.DeleteProductGalleryImage` | `Pages/Admin/AdminProductMedia.razor` | `AdminProductService` | `AdminProductRepository` | Integrated | Delete action |
| Product Media Upload | `ApiEndpoints.ProductMaster.SetPrimaryProductGalleryImage` | `Pages/Admin/AdminProductMedia.razor` | `AdminProductService` | `AdminProductRepository` | Integrated | Set primary action |
| Product Variant | `ApiEndpoints.ProductVariant.*` | `Pages/Admin/AdminProductVariants.razor` | `AdminVariantService` | `AdminVariantRepository` | Integrated | CRUD/status + product-scoped list |
| Product Attribute | `ApiEndpoints.ProductAttribute.CreateAttribute` | `Pages/Admin/AdminAttributes.razor` | `AdminAttributeService` | `AdminAttributeRepository` | Integrated | Attribute master CRUD |
| Product Attribute | `ApiEndpoints.ProductAttribute.CreateAttributeValue` | `Pages/Admin/AdminAttributes.razor` | `AdminAttributeService` | `AdminAttributeRepository` | Integrated | Attribute value CRUD |
| Product Attribute | `ApiEndpoints.ProductAttribute.SaveVariantAttributeMappings` | `Pages/Admin/AdminProductVariants.razor` | `AdminAttributeService` | `AdminAttributeRepository` | Integrated | Save full mapping set |
| Product Attribute | `ApiEndpoints.ProductAttribute.GetVariantAttributeMappings` | `Pages/Admin/AdminProductVariants.razor` | `AdminAttributeService` | `AdminAttributeRepository` | Integrated | List current mappings |
| Shipment Admin | `ApiEndpoints.Order.CreateShipment` | `Pages/Admin/AdminOrderDetail.razor` | `AdminOrderService` | `OrderRepository` | Integrated | Create shipment from order detail |
| Shipment Admin | `ApiEndpoints.Order.GetShipmentList` | `Pages/Admin/AdminShipments.razor` | `AdminOrderService` | `OrderRepository` | Integrated | Shipment list page |
| Shipment Admin | `ApiEndpoints.Order.UpdateShipmentStatus` | `Pages/Admin/AdminShipments.razor` | `AdminOrderService` | `OrderRepository` | Integrated | Status update dialog |
| Return Admin | `ApiEndpoints.Order.GetReturnList` | `Pages/Admin/AdminReturns.razor` | `AdminOrderService` | `OrderRepository` | Integrated | Return list page |
| Return Admin | `ApiEndpoints.Order.UpdateReturnStatus` | `Pages/Admin/AdminReturns.razor` | `AdminOrderService` | `OrderRepository` | Integrated | Status update dialog |
| Role Permission | `ApiEndpoints.RolePermission.*` | `Pages/Admin/AdminRoles.razor` | `RolePermissionService` | `RolePermissionRepository` | Integrated | Role CRUD with permission assignment |
| Admin Archive | `ApiEndpoints.AdminArchive.RunArchive` | `Pages/Admin/AdminArchive.razor` | `AdminArchiveService` | `AdminArchiveRepository` | Integrated | Dry-run + confirmation + result summary |
| AI Product Content | `ApiEndpoints.AiProductContent.GenerateAIDescription` | `Pages/Admin/AdminProducts.razor` | `AdminProductService` | `AdminProductRepository` | Integrated | Suggestion dialog with apply action |
