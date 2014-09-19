
namespace BEAR.cas
{
    /// <summary>
    /// /class
    /// numeric representation of CAS Status codes
    /// </summary>
    public class StatusCAS
    {
        public static int Valid = 1;
        public static int InvalidClientMatter = 2;
        public static int InvalidTimekeeper = 4;
        public static int ConfirmationRequired = 8;
        public static int ApprovalRequired = 16;
        public static int Approved = 32;
        public static int ClosedMatter = 64;
        public static int NonBilledClosedMatter = 128;
        public static int InvalidCostCode = 256;
        public static int InactiveGLString = 512;
        public static int InvalidGLString = 1024;
    }
}
