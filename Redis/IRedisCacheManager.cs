using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace THMS.Core.API.Redis
{
    /// <summary>
    /// Redis缓存接口
    /// </summary>
    public interface IRedisCacheManager
    {

        //获取 Reids 缓存值
        string GetValue(string key);

        //获取值，并序列化
        TEntity Get<TEntity>(string key);

        //保存
        void Set(string key, object value, TimeSpan cacheTime);

        //判断是否存在
        bool Get(string key);

        //移除某一个缓存值
        void Remove(string key);

        //全部清除
        void Clear();
        Task<bool> KeyDelete(string redisKey);
        Task<bool> KeyExists(string redisKey);

        /// <summary>
        /// 获取一个对象（会进行反序列化）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="expiry"></param>
        /// <param name="isDefaultSerialize">户阀时传false，解密用</param>
        /// <returns></returns>
        Task<T> StringGetAsync<T>(string redisKey, TimeSpan? expiry = null, bool isDefaultSerialize = true);  
        Task<bool> StringSetAsync<T>(string redisKey, T redisValue, TimeSpan? expiry = null);
        Task<bool> StringSetAsyncBatch(List<RedisBatchModel> list);
        bool KeyDel(string key);

        /// <summary>
        /// 移除并返回存储在该键列表的最后一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        T ListRightPopAsync<T>(string redisKey);

        /// <summary>
        /// 在列表头部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        Task<long> ListLeftPushAsync<T>(string redisKey, T redisValue);
        Task<long> RedisPub(string channel, string msg);
        long GetListCount(string key);

        /// <summary>
        /// 模糊检索
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="patternKey">模糊查询条件</param>
        /// <returns></returns>
        Task<List<T>> GetPatternAsync<T>(string patternKey);

        /// <summary>
        /// 判断模糊检索是否存在
        /// </summary>
        /// <param name="patternKey"></param>
        /// <returns></returns>
        bool CheckExistKey(string patternKey);

        /// <summary>
        /// 获取模糊检索名称
        /// </summary>
        /// <param name="patternKey"></param>
        /// <returns></returns>
        string[] ExistKeys(string patternKey);
    }
}
