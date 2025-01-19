using Dion.Api.Entities;
using Dion.Models.Dtos.Dtos.Accounting;
using Dion.Models.Dtos.Dtos.Currency;
using Dion.Models.Dtos.Dtos.CustomerParticipant;
using Dion.Models.Dtos.Dtos.StoreType;
using Dion.Models.Dtos.Dtos.StoreWallets;
using Dion.Models.Dtos.Dtos.Transactions;
using Dion.Models.Dtos.Dtos.User;


namespace Dion.Api.Extensions
{
    public static class DtoConversions
    {
        //store type
        public static IEnumerable<StroreTypeGetDto> ConvertToDto(this IEnumerable<StroreType> products)
        {
            return (from product in products
                    select new StroreTypeGetDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Details = product.Details,
                        EnteredDate= product.EnteredDate,
                        IconPath= product.IconPath
                    }).ToList();

        } 
        public static StroreTypeGetDto ConvertToDto(this StroreType storeTypeItem
                                                  )
        {
            return new StroreTypeGetDto
            {
                Id = storeTypeItem.Id,
                Name = storeTypeItem.Name,
                EnteredDate = storeTypeItem.EnteredDate,
                Details= storeTypeItem.Details,
                IconPath= storeTypeItem.IconPath
                
               
            };
        }


        // store customers 
        public static IEnumerable<StoreCustomerGetDto> ConvertToDto(this IEnumerable<StoreCustomers> storeCustomers)
        {
            return storeCustomers.Select(sc => new StoreCustomerGetDto
            {
                AccountCapacity = sc.AccountCapacity,
                IsLock = sc.IsLock,
                EnteredDate = sc.EnteredDate,
                PayNotification = sc.PayNotification,
                CuName = sc.CuName,
                CuAddress = sc.CuAddress,
                IsAccepted = sc.IsAccepted,
                StoreTypeId = sc.StoreTypeId,
                UserId = sc.UserId,
                StoreId = sc.StoreId
            }).ToList();
        }
        public static StoreCustomerGetDto ConvertToDto(this StoreCustomers storeCustomers)
        {
            return new StoreCustomerGetDto
            {
                Id = storeCustomers.Id,
                AccountCapacity = storeCustomers.AccountCapacity,
                IsLock = storeCustomers.IsLock,
                EnteredDate = storeCustomers.EnteredDate,
                PayNotification = storeCustomers.PayNotification,
                CuName = storeCustomers.CuName,
                CuAddress = storeCustomers.CuAddress,
                IsAccepted = storeCustomers.IsAccepted,
                StoreTypeId = storeCustomers.StoreTypeId,
                UserId = storeCustomers.UserId,
                StoreId = storeCustomers.StoreId
            };
        }

        public static IEnumerable<StoreCustomerOrderGetDto> ConvertToOrderDto(this IEnumerable<StoreCustomers> storeCustomers)
        {
            return storeCustomers.Select(sc => new StoreCustomerOrderGetDto
            
            {
                Id=sc.Id,
                AccountCapacity = sc.AccountCapacity,
                
                IsLock = sc.IsLock,
                EnteredDate = sc.EnteredDate,
                PayNotification = sc.PayNotification,
                CuName = sc.CuName,
                CuAddress = sc.CuAddress,
                IsAccepted = sc.IsAccepted,
                StoreTypeId = sc.StoreTypeId,
                UserId = sc.UserId,
                StoreId = sc.StoreId
            }).ToList();
        }

        //user 
        public static UserGetDto ConvertToDto(this User user)
        {
            return new UserGetDto
            {
                Id = user.Id,
                UserName = user.Name,
                EnteredDate = user.EnteredDate,
                UserType = user.Type,
                PhoneNo = user.PhoneNo,
                UserAddress = user.Address,
                UserPassword = user.Password
            };
        }

        //store
        public static StoreGetDto ConvertToDto(this Store store)
        {
            return new StoreGetDto
            {
                Id = store.Id,
                Name = store.Name,
                Latitude = store.Latitude,
                Longitude = store.Longitude,
                Verified = store.Verified,
                EnteredDate = store.EnteredDate,
                StoreTypeId = store.StoreTypeId,
                UserId = store.UserId,StorePhoneNo=store.StorePhoneNo
            };
        }

        // Customer Participant
        public static CustomerParticipantGetDto ConvertToDto(this CustomerParticipant customerParticipant)
        {
            return new CustomerParticipantGetDto
            {
                Id = customerParticipant.Id,
                Name = customerParticipant.Name,
                EnteredDate = customerParticipant.EnteredDate,
                CustomerId = customerParticipant.CustomerId,
                IsActive = customerParticipant.IsActive
            };
        }
        //Currency
        public static IEnumerable<CurrencyGetDto> ConvertToDto(this IEnumerable<Currency> currencies)
        {
            return currencies.Select(currency => new CurrencyGetDto
            {
                Id = currency.Id,
                Name = currency.Name,
                EnteredDate = currency.EnteredDate
            }).ToList();
        }
        public static CurrencyGetDto ConvertToDto(this Currency currency)
        {
            return new CurrencyGetDto
            {
                Id = currency.Id,
                Name = currency.Name,
                EnteredDate = currency.EnteredDate
            };
        }

        //invoice
        public static InvoiceDetailsGetDto ConvertToDto(this InvoiceDetails invoiceDetails)
        {
            return new InvoiceDetailsGetDto
            {
                Id = invoiceDetails.Id,
                Statement = invoiceDetails.Statement,
                Quantity = invoiceDetails.Quantity,
                 InvoiceId= invoiceDetails.InvoiceId,
                UnitPrice = invoiceDetails.UnitPrice
            };
        }

        //transactions
        public static TransactionGetDto ConvertToDto(this TransactionDetails transaction)
        {
            return new TransactionGetDto
            {
                Id = transaction.Id,
                Statement = transaction.Statement,
                EnteredDate = transaction.EnteredDate,
                CustomerId = transaction.CustomerId,
                Credit = transaction.Credit,
                Debit = transaction.Debit,  
                CurrencyId = transaction.CurrencyId,
                InvoiceId = transaction.InvoiceId,
                LockDate = transaction.LockDate,    
                LockStatus = transaction.LockStatus,    
                StoreId = transaction.StoreId,
            };
        }
        // store wallets 
        public static StoreWalletsGetDto ConvertToDto(this StoreWallets storeWallets)
        {
            return new StoreWalletsGetDto
            {
                Id = storeWallets.Id,
                AccountNo = storeWallets.AccountNo,
                Details = storeWallets.Details,
                EnteredDate= storeWallets.EnteredDate,  
                StoreId = storeWallets.StoreId,
                WalletId= storeWallets.WalletId,
                IconPath=String.Empty,

            };
        }
    }
}