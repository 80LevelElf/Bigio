using System;

namespace UnitTests.Managers
{
    static class ExceptionManager
    {
        //For actions
        public static bool IsThrowException<TException>(Action action)
            where TException : Exception
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(TException))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsThrowException<TException, T>(Action<T> action, T argument)
            where TException : Exception
        {
            try
            {
                action.Invoke(argument);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(TException))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsThrowException<TException, T1, T2>(Action<T1, T2> action
            , T1 argument1, T2 argument2)
            where TException : Exception
        {
            try
            {
                action.Invoke(argument1, argument2);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(TException))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsThrowException<TException, T1, T2, T3>(Action<T1, T2, T3> action
            , T1 argument1, T2 argument2, T3 argument3)
            where TException : Exception
        {
            try
            {
                action.Invoke(argument1, argument2, argument3);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(TException))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsThrowException<TException, T1, T2, T3, T4>(Action<T1, T2, T3, T4> action
            , T1 argument1, T2 argument2, T3 argument3, T4 argument4)
            where TException : Exception
        {
            try
            {
                action.Invoke(argument1, argument2, argument3, argument4);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(TException))
                {
                    return true;
                }
            }
            return false;
        }

        //For functions
        public static bool IsThrowException<TException, TResult>(Func<TResult> function)
            where TException : Exception
        {
            try
            {
                function.Invoke();
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(TException))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsThrowException<TException, T, TResult>(Func<T, TResult> function, T argument)
            where TException : Exception
        {
            try
            {
                function.Invoke(argument);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(TException))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsThrowException<TException, T1, T2, TResult>(Func<T1, T2, TResult> function
            , T1 argument1, T2 argument2)
            where TException : Exception
        {
            try
            {
                function.Invoke(argument1, argument2);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(TException))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsThrowException<TException, T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> function
            , T1 argument1, T2 argument2, T3 argument3)
            where TException : Exception
        {
            try
            {
                function.Invoke(argument1, argument2, argument3);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(TException))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsThrowException<TException, T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> function
            , T1 argument1, T2 argument2, T3 argument3, T4 argument4)
            where TException : Exception
        {
            try
            {
                function.Invoke(argument1, argument2, argument3, argument4);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(TException))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
