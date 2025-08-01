using Client.Shared;
using CloudStructures.Structures;
using Humanizer.DateTimeHumanizeStrategy;
using Repository.NoSql;
using Server.Shared.Models.Auth;
using StackExchange.Redis;

namespace GameService.Services
{
    public interface IMemoryDbService
    {
        public Task<(int errorCode, AuthorizedUser result)> IsAuthorizedUser(long accountNo);

        public ValueTask<bool> VerifyToken(AuthorizedUser value, string token);

        public Task<int> RegistAuthorizedUser(long accountNo, AuthorizedUser value);

        public Task<bool> TryLockUserRequest(long accountNo);

        public Task<bool> UnlockUserRequest(long accountNo);

    }

    public class MemoryDbService : IMemoryDbService
    {
        private readonly ILogger<MemoryDbService> _logger;

        private readonly IConnector _connector;

        public MemoryDbService(ILogger<MemoryDbService> logger, IConnector connector) 
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null.");
            this._connector = connector;
        }
  
        public async Task<int> RegistAuthorizedUser(long accountNo, AuthorizedUser value)
        {
            var key = MemoryDbKey.MakeUserAuthKey(accountNo);

            try
            {
                var handle = new RedisString<AuthorizedUser>(_connector.Connection, key, null);
                await handle.SetAsync(value, TimeSpan.FromMinutes(MemoryDbExpireTime.UserAuthMin));
                return ErrorCodes.SUCCESS;
            }
            catch (Exception)
            {
                return ErrorCodes.REDIS_EXCEPTION;
            }
        }

        public async Task<(int errorCode, AuthorizedUser result)> IsAuthorizedUser(long accountNo)
        {
            var key = MemoryDbKey.MakeUserAuthKey(accountNo);

            try
            {
                var authorizedUser = await getAuthorizedUser(accountNo);
                if (authorizedUser == null)
                    return (ErrorCodes.USER_NOT_AUTHORIZED, null);

                return (ErrorCodes.SUCCESS, authorizedUser);
            }
            catch (Exception)
            {
                return (ErrorCodes.REDIS_EXCEPTION, null);
            }
        }

        public ValueTask<bool> VerifyToken(AuthorizedUser authorizedUser, string token)
            => ValueTask.FromResult(authorizedUser?.AuthToken == token);

        /// <summary>
        /// 유저의 여러 요청 병렬 처리 방지를 위한 Lock 설정
        /// </summary>
        public async Task<bool> TryLockUserRequest(long accountNo)
        {
            var key = MemoryDbKey.MakeUserRequestLockKey(accountNo);

            try
            {
                var handle = new RedisString<RequestLock>(_connector.Connection, key, null);

                // 이미 있는 Key라면 false
                if (await handle.SetAsync(new RequestLock { }, TimeSpan.FromSeconds(MemoryDbExpireTime.UserRequestLockSeconds), When.NotExists) == false)
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UnlockUserRequest(long accountNo)
        {
            var key = MemoryDbKey.MakeUserRequestLockKey(accountNo);

            try
            {
                var handle = new RedisString<RequestLock>(_connector.Connection, key, null);
                return await handle.DeleteAsync();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<AuthorizedUser> getAuthorizedUser(long accountNo)
        {
            var key = MemoryDbKey.MakeUserAuthKey(accountNo);

            try
            {
                var handle = new RedisString<AuthorizedUser>(_connector.Connection, key, null);
                var data = await handle.GetAsync();
                if (data.HasValue == false)
                {
                    return null;
                }

                return data.Value;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
