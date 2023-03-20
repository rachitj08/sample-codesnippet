namespace Utilities.PBKDF2Hashing
{
    public class PBKDF2Password
    {
        public PBKDF2Password()
        {

        }

        public byte[] Hash { get; set; }
        public byte[] Salt { get; set; }
    }
}