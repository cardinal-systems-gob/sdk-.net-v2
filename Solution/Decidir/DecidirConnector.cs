﻿using Decidir.Constants;
using Decidir.Model;
using Decidir.Services;
using System;

namespace Decidir
{
    public class DecidirConnector
    {
        #region Constants
        public const string versionDecidir = "1.4.8";

        private const string request_host_sandbox = "https://developers.decidir.com";
        private const string request_host_production = "https://live.decidir.com";
        private const string request_path_payments = "/api/v2/";
        private const string request_path_validate = "/web/";


        #endregion

        private string privateApiKey;
        private string publicApiKey;

        private string endpoint => $"{request_host}{request_path_payments}";
        private string request_host;

        private string validateApiKey;
        private string merchant;

        private HealthCheck healthCheckService;
        private Payments paymentService;
        private UserSite userSiteService;
        private CardTokens cardTokensService;

        public DecidirConnector(int ambiente, string privateApiKey, string publicApiKey, string validateApiKey=null, string  merchant=null )
        {
            if (ambiente == Ambiente.AMBIENTE_PRODUCCION)
            {
                this.request_host = request_host_production;
            }
            else
            {
                this.request_host = request_host_sandbox;
            }

            this.privateApiKey = privateApiKey;
            this.publicApiKey = publicApiKey;
            this.validateApiKey = validateApiKey;
            this.merchant = merchant;
            this.healthCheckService = new HealthCheck(this.endpoint);
            this.paymentService = new Payments(this.endpoint, this.privateApiKey, this.validateApiKey, this.merchant, this.request_host);
            this.userSiteService = new UserSite(this.endpoint, this.privateApiKey);
            this.cardTokensService = new CardTokens(this.endpoint, this.privateApiKey);
        }

        public DecidirConnector(Uri host, string privateApiKey, string publicApiKey, string validateApiKey = null, string merchant = null)
        {
            this.request_host = host.AbsoluteUri.Replace(host.PathAndQuery, "");
            this.privateApiKey = privateApiKey;
            this.publicApiKey = publicApiKey;
            this.validateApiKey = validateApiKey;
            this.merchant = merchant;
            this.healthCheckService = new HealthCheck(this.endpoint);
            this.paymentService = new Payments(this.endpoint, this.privateApiKey, this.validateApiKey, this.merchant, this.request_host);
            this.userSiteService = new UserSite(this.endpoint, this.privateApiKey);
            this.cardTokensService = new CardTokens(this.endpoint, this.privateApiKey);
        }

        public HealthCheckResponse HealthCheck()
        {
            return this.healthCheckService.Execute();
        }
        
        public PaymentResponse Payment(Payment payment)
        {
            return this.paymentService.ExecutePayment(payment);
        }

        public PaymentResponse Payment(OfflinePayment payment)
        {
            return this.paymentService.ExecutePayment(payment);
        }

        public CapturePaymentResponse CapturePayment(long paymentId, double amount)
        {
            return this.paymentService.CapturePayment(paymentId, amount);
        }

        public GetAllPaymentsResponse GetAllPayments(long? offset = null, long? pageSize = null, string siteOperationId = null, string merchantId = null)
        {
            return this.paymentService.GetAllPayments(offset, pageSize, siteOperationId, merchantId);
        }

        public PaymentResponse GetPaymentInfo(long paymentId)
        {
            return this.paymentService.GetPaymentInfo(paymentId);
        }

        public RefundResponse Refund(long paymentId)
        {
            return this.paymentService.Refund(paymentId);
        }

        public DeleteRefundResponse DeleteRefund(long paymentId, long refundId)
        {
            return this.paymentService.DeleteRefund(paymentId, refundId);
        }

        public RefundResponse PartialRefund(long paymentId, double amount)
        {
            return this.paymentService.PartialRefund(paymentId, amount);
        }

        public DeleteRefundResponse DeletePartialRefund(long paymentId, long refundId)
        {
            return this.paymentService.DeletePartialRefund(paymentId, refundId);
        }

        public GetAllCardTokensResponse GetAllCardTokens(string userId)
        {
            return this.userSiteService.GetAllTokens(userId);
        }

        public bool DeleteCardToken(string token)
        {
            return this.cardTokensService.DeleteCardToken(token);
        }

        public ValidateResponse Validate(ValidateData validateData)
        {
            return this.paymentService.ValidatePayment(validateData);
        }
        

    }
}
