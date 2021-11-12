using Infracciones.App.Models;
using Infracciones.App.Services.Endpoints;
using Infracciones.Services.DTO;
using Infracciones.Services.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infracciones.App.Services
{
    public static class PaymentService
    {
        private static readonly PaymentEndpoints _endpoint = new PaymentEndpoints();
        private static readonly LogService _logService = new LogService();

        public static async Task<PaymentModel> Pay(string paymentReference, PaymentModel payment)
        {
            try
            {
                payment.Log = await _logService.CreateLog();
                return await _endpoint.Pay(paymentReference, payment);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<IEnumerable<PaymentDTO>> Close(IEnumerable<PaymentDTO> payments)
        {
            try
            {
                return await _endpoint.Close(payments);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<PaymentDTO> Close(int sanctionId, PaymentDTO payment)
        {
            try
            {
                return await _endpoint.Close(sanctionId, payment);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<PaymentModel> GetByReference(string paymentReference)
        {
            return await _endpoint.GetByReference(paymentReference);
        }

        public static async Task<List<PaymentModel>> GetOByOfficer(string officerId)
        {
            return await _endpoint.GetByOfficer(officerId);
        }

        public static async Task<List<PaymentModel>> GetSanctionsPaid()
        {
            return await _endpoint.GetSanctionsPaid();
        }

        public static async Task<List<PaymentModel>> GetPaymentOpenByStatusAndOfficerId(bool open, string status, string officerId)
        {
            return await _endpoint.GetOpenByStatusAndOfficer(open, status, officerId);
        }
    }
}
