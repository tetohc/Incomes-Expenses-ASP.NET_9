using MyFinance.Models.Entities;
using MyFinance.Models.ViewModels;

namespace MyFinance.Mappers
{
    public static class TransactionMapper
    {
        /// <summary>
        /// Recibe un ViewModel de transacción y lo convierte a una entidad de transacción.
        /// </summary>
        /// <param name="transactionVM">ViewModel de transacción.</param>
        /// <returns>Entidad de transacción `Transaction`.</returns>
        public static Transaction ToEntity(this TransactionVM transactionVM)
        {
            return new Transaction
            {
                Id = transactionVM.Id,
                UserId = transactionVM.UserId,
                ServiceId = transactionVM.ServiceId,
                Comment = transactionVM.Comment.Trim(),
                TotalAmount = transactionVM.TotalAmount,
                Date = transactionVM.Date,
                RegisterDate = DateTime.Now,
                IsActive = transactionVM.IsActive
            };
        }

        /// <summary>
        /// Recibe una entidad de transacción y la convierte a un ViewModel de transacción.
        /// </summary>
        /// <param name="transaction">Entidad de transacción.</param>
        /// <returns>ViewModel de transaction.</returns>
        public static TransactionVM ToViewModel(this Transaction transaction)
        {
            return new TransactionVM
            {
                Id = transaction.Id,
                UserId = transaction.UserId,
                ServiceId = transaction.ServiceId,
                Service = transaction.Service.Name,
                Comment = transaction.Comment.Trim(),
                TotalAmount = transaction.TotalAmount,
                Date = transaction.Date,
                RegisterDate = transaction.RegisterDate,
                IsActive = transaction.IsActive,
                TypeTransaction = transaction.Service.Type,
            };
        }
    }
}