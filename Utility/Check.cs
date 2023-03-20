namespace Utilities
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;

    /// <summary>
    /// Check
    /// </summary>
    public static class Check
    {
        /// <summary>
        /// Check a value is not null
        /// </summary>
        /// <param name="value">Argument value to test</param>
        /// <param name="message">An optional message</param>
        /// <param name="caller">The caller</param>
        /// <param name="file">The file</param>
        /// <param name="line">The line number</param>
        public static void IsNotNull(
            object value,
            string message = null,
            [CallerMemberName]string caller = "",
            [CallerFilePath]string file = "",
            [CallerLineNumber]int line = 0)
        {
            if (value == null)
            {
                throw BuildError(caller, file, line, message != null ? message : "Value is null");
            }
        }

        /// <summary>
        /// Check a vlaue is null
        /// </summary>
        /// <param name="value">Argument value to test</param>
        /// <param name="message">An optional message</param>
        /// <param name="caller">The caller</param>
        /// <param name="file">The file</param>
        /// <param name="line">The line number</param>
        public static void IsNull(
            object value,
            string message = null,
            [CallerMemberName]string caller = "",
            [CallerFilePath]string file = "",
            [CallerLineNumber]int line = 0)
        {
            if (value != null)
            {
                throw BuildError(caller, file, line, message != null ? message : "Value is not null");
            }
        }

        /// <summary>
        /// Check string is not null or whiteSpace
        /// </summary>
        /// <param name="value">Argument value to test</param>
        /// <param name="message">An optional message</param>
        /// <param name="caller">The caller</param>
        /// <param name="file">The file</param>
        /// <param name="line">The line number</param>
        public static void StringIsNotNullOrWhiteSpace(
            string value,
            string message = null,
            [CallerMemberName]string caller = "",
            [CallerFilePath]string file = "",
            [CallerLineNumber]int line = 0)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw BuildError(caller, file, line, message != null ? message : "Value is null empty or whitespace");
            }
        }

        /// <summary>
        /// Check an expression is true
        /// </summary>
        /// <param name="value">Argument value to test</param>
        /// <param name="message">An optional message</param>
        /// <param name="caller">The caller</param>
        /// <param name="file">The file</param>
        /// <param name="line">The line number</param>
        public static void IsTrue(
            bool value,
            string message = null,
            [CallerMemberName]string caller = "",
            [CallerFilePath]string file = "",
            [CallerLineNumber]int line = 0)
        {
            if (!value)
            {
                throw BuildError(caller, file, line, message != null ? message : "Value is not true");
            }
        }

        /// <summary>
        /// Check an expression is false
        /// </summary>
        /// <param name="value">Argument value to test</param>
        /// <param name="message">An optional message</param>
        /// <param name="caller">The caller</param>
        /// <param name="file">The file</param>
        /// <param name="line">The line number</param>
        public static void IsFalse(
            bool value,
            string message = null,
            [CallerMemberName]string caller = "",
            [CallerFilePath]string file = "",
            [CallerLineNumber]int line = 0)
        {
            if (value)
            {
                throw BuildError(caller, file, line, message != null ? message : "Value is true");
            }
        }

        /// <summary>
        /// Check strings are equal ignoring case
        /// </summary>
        /// <param name="lhs">The LHS to check</param>
        /// <param name="rhs">The RHS to check</param>
        /// <param name="caller">The caller</param>
        /// <param name="file">The file</param>
        /// <param name="line">The line number</param>
        public static void StringsAreEqualIgnoreCase(
            string lhs, 
            string rhs,
            [CallerMemberName]string caller = "",
            [CallerFilePath]string file = "",
            [CallerLineNumber]int line = 0)
        {
            StringsAreEqual(lhs, rhs, StringComparison.OrdinalIgnoreCase, caller, file, line);
        }

        /// <summary>
        /// Check strings are equal
        /// </summary>
        /// <param name="lhs">The LHS to check</param>
        /// <param name="rhs">The RHS to check</param>
        /// <param name="comparison">String comparison</param>
        /// <param name="caller">The caller</param>
        /// <param name="file">The file</param>
        /// <param name="line">The line number</param>
        public static void StringsAreEqual(
            string lhs, 
            string rhs, 
            StringComparison comparison = StringComparison.OrdinalIgnoreCase,
            [CallerMemberName]string caller = "",
            [CallerFilePath]string file = "",
            [CallerLineNumber]int line = 0)
        {
            if (!string.Equals(lhs, rhs, comparison))
            {
                throw BuildError(caller, file, line, "StringComparison.{0} {1} != {2}", comparison, lhs != null ? lhs : "NULL", rhs != null ? rhs : "NULL");
            }
        }

        /// <summary>
        /// Check objects are equal
        /// </summary>
        /// <param name="lhs">The LHS to check</param>
        /// <param name="rhs">The RHS to check</param>
        /// <param name="caller">The caller</param>
        /// <param name="file">The file</param>
        /// <param name="line">The line number</param>
        public static void AreEqual(
            object lhs, 
            object rhs, 
            [CallerMemberName]string caller = "", 
            [CallerFilePath]string file = "", 
            [CallerLineNumber]int line = 0)
        {
            if (!object.Equals(lhs, rhs))
            {
                throw BuildError(caller, file, line, "Object {0} != {1}", lhs != null ? lhs : "NULL", rhs != null ? rhs : "NULL");
            }
        }

        /// <summary>
        /// Argument checks
        /// </summary>
        public static class Argument
        {
            /// <summary>
            /// Check the arguement is not null
            /// </summary>
            /// <param name="argumentName">The argument name</param>
            /// <param name="value">The value to test</param>
            /// <param name="caller">The caller</param>
            /// <param name="file">The file</param>
            /// <param name="line">The line number</param>
            public static void IsNotNull(
                string argumentName, 
                object value, 
                [CallerMemberName]string caller = "", 
                [CallerFilePath]string file = "", 
                [CallerLineNumber]int line = 0)
            {
                if (value == null)
                {
                    throw BuildError(caller, file, line, "Argument {0} == null", argumentName);
                }
            }

            /// <summary>
            /// Check the string arguement is not null or white space
            /// </summary>
            /// <param name="argumentName">The argument name</param>
            /// <param name="value">The value to test</param>
            /// <param name="caller">The caller</param>
            /// <param name="file">The file</param>
            /// <param name="line">The line number</param>
            public static void StringIsNotNullOrWhiteSpace(
                string argumentName, 
                string value, 
                [CallerMemberName]string caller = "", 
                [CallerFilePath]string file = "", 
                [CallerLineNumber]int line = 0)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw BuildError(caller, file, line, "Argument {0} IsNullOrWhiteSpace", argumentName);
                }
            }

            /// <summary>
            /// Check the arguement is true
            /// </summary>
            /// <param name="argumentName">The argument name</param>
            /// <param name="value">The value to test</param>
            /// <param name="caller">The caller</param>
            /// <param name="file">The file</param>
            /// <param name="line">The line number</param>
            public static void IsTrue(
                string argumentName, 
                bool value, 
                [CallerMemberName]string caller = "", 
                [CallerFilePath]string file = "", 
                [CallerLineNumber]int line = 0)
            {
                if (!value)
                {
                    throw BuildError(caller, file, line, "Argument {0} != true", argumentName);
                }
            }


            /// <summary>
            /// Check the arguement is false
            /// </summary>
            /// <param name="argumentName">The argument name</param>
            /// <param name="value">The value to test</param>
            /// <param name="caller">The caller</param>
            /// <param name="file">The file</param>
            /// <param name="line">The line number</param>
            public static void IsFalse(
                string argumentName, 
                bool value, 
                [CallerMemberName]string caller = "", 
                [CallerFilePath]string file = "", 
                [CallerLineNumber]int line = 0)
            {
                if (value)
                {
                    throw BuildError(caller, file, line, "Argument {0} != false", argumentName);
                }
            }

            /// <summary>
            /// Check the argument is not an empty guid
            /// </summary>
            /// <param name="argumentName">The argument name</param>
            /// <param name="value">The value to test</param>
            /// <param name="caller">The caller</param>
            /// <param name="file">The file</param>
            /// <param name="line">The line number</param>
            public static void IsNotEmptyGuid(
                string argumentName,
                Guid value,
                [CallerMemberName]string caller = "",
                [CallerFilePath]string file = "",
                [CallerLineNumber]int line = 0)
            {
                if (value == Guid.Empty)
                {
                    throw BuildError(caller, file, line, "Argument {0} is an empty guid", argumentName);
                }
            }
        }

        /// <summary>
        /// Build error
        /// </summary>
        /// <param name="caller">The caller</param>
        /// <param name="file">The file</param>
        /// <param name="line">The line number</param>
        /// <param name="format">Format string</param>
        /// <param name="args">Format args</param>
        /// <returns>
        /// An <see cref="CheckException"/> instance
        /// </returns>
        private static CheckException BuildError(string caller, string file, int line, string format, params object[] args)
        {
            return BuildError(
                caller: caller, 
                file: file,
                line:line,
                message: string.Format(format, args));
        }

        /// <summary>
        /// Build error
        /// </summary>
        /// <param name="caller">The caller</param>
        /// <param name="file">The file</param>
        /// <param name="line">The line number</param>
        /// <param name="message">The message</param>
        /// <returns>
        /// An <see cref="CheckException"/> instance
        /// </returns>
        private static CheckException BuildError(string caller, string file, int line, string message)
        {
            var sb = new StringBuilder();

            sb.AppendLine(string.Format("error:{0}", message));
            sb.AppendLine(string.Format("caller:{0}", caller));
            sb.AppendLine(string.Format("file:{0}", file));
            sb.AppendLine(string.Format("line:{0}", line));

            return new CheckException(sb.ToString());
        }
    }
}
