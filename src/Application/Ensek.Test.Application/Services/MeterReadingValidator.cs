using AutoMapper;
using Ensek.Test.Application.Models;
using Ensek.Test.Application.Services.Interfaces;
using Ensek.Test.Domain.Entities;
using Ensek.Test.Domain.Interfaces.Repositories;

namespace Ensek.Test.Application.Services
{
    public class MeterReadingValidator : IMeterReadingValidator
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMeterReadingRepository _meterReadingRepository;
        private readonly IMapper _mapper;
        private const int RequiredReadingValueLength = 5;

        public MeterReadingValidator(IAccountRepository accountRepository, IMeterReadingRepository meterReadingRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _meterReadingRepository = meterReadingRepository;
            _mapper = mapper;
        }

        public async Task<ValidateReadingResponse> ValidateReading(MeterReadingModel meterReadingModel)
        {
            if (await AccountDoesNotExist(meterReadingModel.AccountId))
            {
                return ValidateReadingResponse.CreateInvalidResponse();
            }

            if (ReadingIsInvalid(meterReadingModel))
            {
                return ValidateReadingResponse.CreateInvalidResponse();
            }

            var meterReading = _mapper.Map<MeterReading>(meterReadingModel);

            if (await ReadingExists(meterReading))
            {
                return ValidateReadingResponse.CreateInvalidResponse();
            }

            return ValidateReadingResponse.CreateValidResponse(meterReading);
        }

        private async Task<bool> AccountDoesNotExist(int accountId)
        {
            var account = await _accountRepository.GetById(accountId);
            return account == null;
        }

        private static bool ReadingIsInvalid(MeterReadingModel meterReadingModel)
        {
            if (string.IsNullOrWhiteSpace(meterReadingModel.ReadingValue))
            {
                return true;
            }

            if (!int.TryParse(meterReadingModel.ReadingValue, out var result) || result < 0)
            {
                return true;
            }

            if (meterReadingModel.ReadingValue.Length > RequiredReadingValueLength)
            {
                return true;
            }

            return false;
        }

        private async Task<bool> ReadingExists(MeterReading meterReading)
        {
            var existingMeterReading = await _meterReadingRepository.GetExistingReading(meterReading);
            return existingMeterReading != null;
        }
    }
}
