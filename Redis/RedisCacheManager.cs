using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessagePack;
using MessagePack.Resolvers;
using THMS.Core.API.Configuration;

namespace THMS.Core.API.Redis
{
    public class RedisCacheManager : IRedisCacheManager
    {

        private readonly string redisConnenctionString;

        public volatile ConnectionMultiplexer redisConnection;

        private readonly object redisConnectionLock = new object();

        public RedisCacheManager()
        {
            string redisConfiguration = ConfigAppsetting.RedisConfig;//获取连接字符串

            if (string.IsNullOrWhiteSpace(redisConfiguration))
            {
                throw new ArgumentException("redis config is empty", nameof(redisConfiguration));
            }
            this.redisConnenctionString = redisConfiguration;
            this.redisConnection = GetRedisConnection();
            MessagePackSerializer.SetDefaultResolver(ContractlessStandardResolver.Instance);
        }

        /// <summary>
        /// 核心代码，获取连接实例
        /// 通过双if 夹lock的方式，实现单例模式
        /// </summary>
        /// <returns></returns>
        private ConnectionMultiplexer GetRedisConnection()
        {
            //如果已经连接实例，直接返回
            if (this.redisConnection != null && this.redisConnection.IsConnected)
            {
                return this.redisConnection;
            }
            //加锁，防止异步编程中，出现单例无效的问题
            lock (redisConnectionLock)
            {
                if (this.redisConnection != null)
                {
                    //释放redis连接
                    this.redisConnection.Dispose();
                }
                try
                {
                    this.redisConnection = ConnectionMultiplexer.Connect(redisConnenctionString);
                }
                catch (Exception)
                {
                    //throw new Exception("Redis服务未启用，请开启该服务，并且请注意端口号，本项目使用的的6319，而且我的是没有设置密码。");
                }
            }
            return this.redisConnection;
        }
        /// <summary>
        /// 清除
        /// </summary>
        public void Clear()
        {
            foreach (var endPoint in this.GetRedisConnection().GetEndPoints())
            {
                var server = this.GetRedisConnection().GetServer(endPoint);
                foreach (var key in server.Keys())
                {
                    redisConnection.GetDatabase().KeyDelete(key);
                }
            }
        }
        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Get(string key)
        {
            return redisConnection.GetDatabase().KeyExists(key);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            return redisConnection.GetDatabase().StringGet(key);
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public TEntity Get<TEntity>(string key)
        {
            var value = redisConnection.GetDatabase().StringGet(key);
            if (value.HasValue)
            {
                //需要用的反序列化，将Redis存储的Byte[]，进行反序列化
                return SerializeHelper.Deserialize<TEntity>(value);
            }
            else
            {
                return default(TEntity);
            }
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            redisConnection.GetDatabase().KeyDelete(key);
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheTime"></param>
        public void Set(string key, object value, TimeSpan cacheTime)
        {
            if (value != null)
            {
                //序列化，将object值生成RedisValue
                redisConnection.GetDatabase().StringSet(key, SerializeHelper.Serialize(value), cacheTime);
            }
        }

        /// <summary>
        /// 增加/修改
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetValue(string key, byte[] value)
        {
            return redisConnection.GetDatabase().StringSet(key, value, TimeSpan.FromSeconds(120));
        }
        #region by nm
        /// <summary>
        /// 移除指定 Key
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<bool> KeyDelete(string redisKey)
        {
            var db = redisConnection.GetDatabase();
            if (db.KeyExists(redisKey))
                return await db.KeyDeleteAsync(redisKey);
            return true;
        }

        public bool KeyDel(string key)
        {
            return redisConnection.GetDatabase().KeyDelete(key);
            //redisConnection.GetDatabase().st
        }

        /// <summary>
        /// 校验 Key 是否存在
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public async Task<bool> KeyExists(string redisKey)
        {
            return await redisConnection.GetDatabase().KeyExistsAsync(redisKey);
        }

        /// <summary>
        /// 存储一个对象（该对象会被序列化保存）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync<T>(string redisKey, T redisValue, TimeSpan? expiry = null)
        {
            var json = JsonConvert.SerializeObject(redisValue);
            return await redisConnection.GetDatabase().StringSetAsync(redisKey, json, expiry);
        }

        public async Task<bool> StringSetAsyncBatch(List<RedisBatchModel> list)
        {
            var result = true;
            try
            {
                var batch = redisConnection.GetDatabase().CreateBatch();
                foreach (var item in list)
                {
                    batch.StringSetAsync(item.KeyName, item.KeyValue);
                }
                batch.Execute();
            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 获取一个对象（会进行反序列化）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="expiry"></param>
        /// <param name="isDefaultSerialize">户阀时传false，解密用</param>
        /// <returns></returns>
        public async Task<T> StringGetAsync<T>(string redisKey, TimeSpan? expiry = null, bool isDefaultSerialize = true)
        {
            if (isDefaultSerialize)
                return JsonConvert.DeserializeObject<T>(await redisConnection.GetDatabase().StringGetAsync(redisKey));
            //return MessagePackSerializer.Deserialize<T>(await redisConnection.GetDatabase().StringGetAsync(redisKey));
            var value = await redisConnection.GetDatabase().StringGetAsync(redisKey);
            var a= MessagePackSerializer.Deserialize<T>(value);
            return a;
        }
        public async Task<List<T>> GetPatternAsync<T>(string patternKey) => await GetAllAsync<T>(ExistKeys(patternKey));

        public bool CheckExistKey(string patternKey) => ExistKeys(patternKey).Any();
        public string[] ExistKeys(string patternKey)
        {
            var result = (string[])redisConnection.GetDatabase().ScriptEvaluate(LuaScript.Prepare("return redis.call('KEYS',@pattern)"), new { pattern = patternKey });
            return result;
        }
        private async Task<List<T>> GetAllAsync<T>(IEnumerable<string> keys)
        {
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));

            var keyArray = keys.ToArray();
            var values = await redisConnection.GetDatabase().StringGetAsync(keyArray.Select(k => (RedisKey)k).ToArray());

            
            var result = new List<T>();

            //原代码
            //for (int i = 0; i < keyArray.Length; i++)
            //{
            //    var cachedValue = values[i];
            //    if (cachedValue.HasValue && !cachedValue.IsNull)
            //        result.Add(MessagePackSerializer.Deserialize<T>(cachedValue));
            //}

            //wjd 2020-12-14修改
            foreach (var item in values)
            {
                if (item.HasValue && !item.IsNull)
                    result.Add(JsonConvert.DeserializeObject<T>(item));
            }
            return result;
        }

        /// <summary>
        /// 返回在该列表上键所对应的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        private static IEnumerable<string> ConvertStrings<T>(IEnumerable<T> list) where T : struct
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            return list.Select(x => x.ToString());
        }
        #endregion

        #region List-async


        /// <summary>
        /// 移除并返回存储在该键列表的最后一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public T ListRightPopAsync<T>(string redisKey)
        {
            return JsonConvert.DeserializeObject<T>(redisConnection.GetDatabase().ListRightPop(redisKey));
        }
        /// <summary>
        /// 在列表头部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public async Task<long> ListLeftPushAsync<T>(string redisKey, T redisValue)
        {
            var count = redisConnection.GetDatabase().ListLength(redisKey);
            if (count > 10)
            {
                var t = ListRightPopAsync<T>(redisKey);
            }
            return await redisConnection.GetDatabase().ListLeftPushAsync(redisKey, JsonConvert.SerializeObject(redisValue));
        }

        public long GetListCount(string key)
        {
            var count = redisConnection.GetDatabase().ListLength(key);
            return count;
        }

        #endregion List-async

        public delegate void RedisDeletegate(string str);
        public event RedisDeletegate RedisSubMessageEvent;

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="subChannel"></param>
        public void RedisSub(string subChannel)
        {

            redisConnection.GetSubscriber().Subscribe(subChannel, (channel, message) =>
            {
                RedisSubMessageEvent?.Invoke(message); //触发事件

            });

        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task<long> RedisPub(string channel, string msg)
        {
            return await redisConnection.GetSubscriber().PublishAsync(channel, msg);
        }

    }

    public class RedisBatchModel
    {
        /// <summary>
        /// key名称
        /// </summary>
        public string KeyName { get; set; }

        /// <summary>
        /// key值
        /// </summary>
        public string KeyValue { get; set; }
    }
}
