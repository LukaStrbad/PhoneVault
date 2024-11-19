using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using PhoneVault.Models;

namespace PhoneVault.Services;

public class EmailService
{
    private readonly string _from;
    private readonly SmtpClient _smtpClient;
    private readonly string? _serverUrl;

    public EmailService(IConfiguration configuration)
    {
        var from = configuration["Email:From"];
        var pass = configuration["Email:Password"];
        ArgumentNullException.ThrowIfNull(from, "Email:From");
        ArgumentNullException.ThrowIfNull(pass, "Email:Password");
        _from = from;

        _serverUrl = configuration["FrontendServerUrl"];

        _smtpClient = new SmtpClient
        {
            Port = 587,
            Host = "smtp.gmail.com",
            Credentials = new NetworkCredential(_from, pass),
            EnableSsl = true
        };
    }

    public void SendEmail(string recipientAddress, string subject, string body)
    {
        if (string.IsNullOrEmpty(recipientAddress))
        {
            throw new Exception("Recipient address is invalid");
        }

        try
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_from),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            mailMessage.To.Add(recipientAddress);
            _smtpClient.Send(mailMessage);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to send email: " + ex.Message);
        }
    }

    public void SendOrderMail(string recipientAddress, List<Product> products, ICollection<OrderItem> orderOrderItems)
    {
        var orderString = string.Join("\n", orderOrderItems.Select(item =>
        {
            var product = products.FirstOrDefault(p => p.Id == item.ProductId);
            var productName = product?.Name ?? $"Unknown product (id: {item.ProductId})";
            var totalPrice = item.PriceAtPurchase * item.Quantity;
            return $"\t- {productName} x {item.Quantity} (${totalPrice})";
        }));
        
        var totalPrice = orderOrderItems.Sum(oi => oi.PriceAtPurchase * oi.Quantity);
        
        SendEmail(recipientAddress, "Order Confirmation",
            $"""
            Thank you for your order. 
            Here is the list of products you have ordered:
            {orderString}
            Total price: ${totalPrice} €
            
            Your PhoneVault team!
            """);
    }

    public void SendNewProductMail(string recipientAddress, Product product)
    {
        var newProductString = "";
        if (_serverUrl is not null)
        {
            var productUrl = $"{_serverUrl}/product/{product.Id}";
            newProductString = $"""
                               
                               Name: {product.Name}
                               Link: {productUrl}
                               """;
        }

        SendEmail(recipientAddress, "New Product Alert",
            """
            A new product has been added to our store. 
            Check it out now!
            """ +
            newProductString +
            "\nYour PhoneVault team!");
    }
}