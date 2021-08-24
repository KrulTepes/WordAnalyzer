using NLog;
using System;

namespace WordAnalyzer.Common
{
    public static class ConvertTypes
    {
        public static T? ChangeType<T>(object? value)
        {
            var t = typeof(T);

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return default(T);
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return (T)Convert.ChangeType(value!, t!);
        }

        public static object? ChangeType(object? value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t!);
        }

        public static bool TryChangeType<T>(object? value, out T? result)
        {
            try
            {
                var t = typeof(T);
                if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    if (value == null)
                    {
                        result = default(T);
                        return false;
                    }

                    t = Nullable.GetUnderlyingType(t);
                }

                result = (T)Convert.ChangeType(value!, t!);
                return true;
            }
            catch (Exception e)
            {
                var logger = LogManager.GetCurrentClassLogger();
                logger.Error(e, "Change type error.");

                result = default(T);
                return false;
            }
        }

        public static bool TryChangeType(object? value, Type conversion, out object? result)
        {
            try
            {
                var t = conversion;

                if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    if (value == null)
                    {
                        result = null;
                        return false;
                    }

                    t = Nullable.GetUnderlyingType(t);
                }

                result = Convert.ChangeType(value, t!);
                return true;
            }
            catch (Exception e)
            {
                var logger = LogManager.GetCurrentClassLogger();
                logger.Error(e, "Change type error.");

                result = null;
                return false;
            }
        }
    }
}
