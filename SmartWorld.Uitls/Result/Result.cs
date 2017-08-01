using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace SmartWorld.Uitls
{
    /// <summary>
    /// 表示业务处理结果类，用于定义一般业务处理返回信息。
    /// </summary>
    /// <typeparam name="T">业务处理结果类型。</typeparam>
    [Serializable]
    public class Result<T>
    {
        public Result()
        {
            Success = true;
        }

        public Result(T value) : this()
        {
            Data = value;
        }

        /// <summary>
        /// Default result.
        /// </summary>
        public static Result<T> Default
        {
            get
            {
                return new Result<T> { Success = true };
            }
        }

        /// <summary>
        /// Failed result with error message.
        /// </summary>
        public static Result<T> Error(string errorMessage)
        {
            return new Result<T> { Success = false, Message = errorMessage };
        }

        /// <summary>
        /// Failed result with error message.
        /// </summary>
        public static Result<T> Error()
        {
            return Error(string.Empty);
        }

        /// <summary>
        /// 获取或设置是否成功返回了正确的数据。
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 获取或设置消息。
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 获取或设置是否有错误发生。
        /// </summary>
        public bool HasException { get; set; }

        /// <summary>
        /// 获取或设置处理结果的值
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 分页
        /// </summary>
        public PagingInfo PagingInfo { get; set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public object Tag { get; set; }
    }

    /// <summary>
    /// 表示业务处理结果类,不附带数据
    /// </summary>
    public class Result : Result<object>
    {
        public new static Result Default
        {
            get
            {
                return new Result { Success = true };
            }
        }

        /// <summary>
        /// Failed result with error message.
        /// </summary>
        public new static Result Error(string errorMessage)
        {
            return new Result { Success = false, Message = errorMessage };
        }

        /// <summary>
        /// Failed result with error message.
        /// </summary>
        public new static Result Error()
        {
            return Error(string.Empty);
        }
    }

    public struct PagingInfo
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int nPage { get; set; }
        /// <summary>
        /// 分页展示数量
        /// </summary>
        public int nPageSize { get; set; }
        /// <summary>
        /// 当前页面地址
        /// </summary>
        public string strPageUrl { get; set; }
        /// <summary>
        /// 总记录数量
        /// </summary>
        public int nSumCount { get; set; }

    }
}
