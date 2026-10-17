using Domain.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data.Converters;

public static class OrderStatusConversion
{
    public static readonly ValueConverter<OrderStatus, string> Converter = new(
        value => ToDatabase(value),
        value => FromDatabase(value));

    private static string ToDatabase(OrderStatus status) => status switch
    {
        OrderStatus.Pending => "pending",
        OrderStatus.Confirmed => "confirmed",
        OrderStatus.Preparing => "preparing",
        OrderStatus.ReadyForPickup => "ready_for_pickup",
        OrderStatus.OutForDelivery => "out_for_delivery",
        OrderStatus.Delivered => "delivered",
        OrderStatus.Cancelled => "cancelled",
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Unknown order status.")
    };

    private static OrderStatus FromDatabase(string status) => status switch
    {
        "pending" => OrderStatus.Pending,
        "confirmed" => OrderStatus.Confirmed,
        "preparing" => OrderStatus.Preparing,
        "ready_for_pickup" => OrderStatus.ReadyForPickup,
        "out_for_delivery" => OrderStatus.OutForDelivery,
        "delivered" => OrderStatus.Delivered,
        "cancelled" => OrderStatus.Cancelled,
        _ => throw new ArgumentException("Unknown order status.", nameof(status))
    };
}
