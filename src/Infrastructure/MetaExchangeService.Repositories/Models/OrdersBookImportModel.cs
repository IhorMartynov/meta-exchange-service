namespace MetaExchangeService.Repositories.Models;

internal record OrdersBookImportModel(DateTime AcqTime, OrdersBookItemModel[] Bids, OrdersBookItemModel[] Asks);
internal record OrdersBookItemModel(OrderImportModel Order);
internal record OrderImportModel(long? Id, DateTime Time, decimal Amount, decimal Price);