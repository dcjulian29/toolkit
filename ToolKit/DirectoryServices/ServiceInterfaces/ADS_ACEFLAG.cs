//// ReSharper disable InconsistentNaming

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies how security propagates for inherited access-control entries (ACEs) and types of
    /// auditing for a system ACE.
    /// </summary>
    public enum ADS_ACEFLAG
    {
        /// <summary>
        /// Child objects will inherit this access-control entry (ACE). The inherited ACE is
        /// inheritable unless the ADS_ACEFLAG_NO_PROPAGATE_INHERIT_ACE flag is set.
        /// </summary>
        INHERIT_ACE = 0x2,

        /// <summary>
        /// The system will clear the ADS_ACEFLAG_INHERIT_ACE flag for the inherited ACEs of child
        /// objects. This prevents the ACE from being inherited by subsequent generations of objects.
        /// </summary>
        NO_PROPAGATE_INHERIT_ACE = 0x4,

        /// <summary>
        /// Indicates that an inherit-only ACE that does not exercise access control on the object
        /// to which it is attached. If this flag is not set, the ACE is an effective ACE that
        /// exerts access control on the object to which it is attached.
        /// </summary>
        INHERIT_ONLY_ACE = 0x8,

        /// <summary>
        /// Indicates whether or not the ACE was inherited. The system sets this bit.
        /// </summary>
        INHERITED_ACE = 0x10,

        /// <summary>
        /// Indicates whether the inherit flags are valid. The system sets this bit.
        /// </summary>
        VALID_INHERIT_FLAGS = 0x1f,

        /// <summary>
        /// Generates audit messages for successful access attempts, used with ACEs that audit the
        /// system in a system access-control list (SACL).
        /// </summary>
        SUCCESSFUL_ACCESS = 0x40,

        /// <summary>
        /// Generates audit messages for failed access attempts, used with ACEs that audit the
        /// system in a SACL.
        /// </summary>
        FAILED_ACCESS = 0x80
    }
}
