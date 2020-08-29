using System;
using System.Diagnostics.CodeAnalysis;

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies the access rights to a directory service object.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Naming",
        "CA1707:IdentifiersShouldNotContainUnderscores",
        Justification = "This enumeration represents Win32API which include underscores.")]
    [Flags]
    public enum ADS_RIGHTS : uint
    {
        /// <summary>
        /// The right to delete the object.
        /// </summary>
        DELETE = 0x10000,

        /// <summary>
        /// The right to read data from the security descriptor of the object, not including the
        /// data in the SACL.
        /// </summary>
        READ_CONTROL = 0x20000,

        /// <summary>
        /// The right to modify the discretionary access-control list (DACL) in the object security descriptor.
        /// </summary>
        WRITE_DAC = 0x40000,

        /// <summary>
        /// The right to assume ownership of the object. The user must be an object trustee. The
        /// user cannot transfer the ownership to other users.
        /// </summary>
        WRITE_OWNER = 0x80000,

        /// <summary>
        /// The right to use the object for synchronization. This enables a thread to wait until the
        /// object is in the signaled state.
        /// </summary>
        SYNCHRONIZE = 0x100000,

        /// <summary>
        /// The right to get or set the SACL in the object security descriptor.
        /// </summary>
        ACCESS_SYSTEM_SECURITY = 0x1000000,

        /// <summary>
        /// The right to read permissions on this object, read all the properties on this object,
        /// list this object name when the parent container is listed, and list the contents of this
        /// object if it is a container.
        /// </summary>
        GENERIC_READ = 0x80000000,

        /// <summary>
        /// The right to read permissions on this object, write all the properties on this object,
        /// and perform all validated writes to this object.
        /// </summary>
        GENERIC_WRITE = 0x40000000,

        /// <summary>
        /// The right to read permissions on, and list the contents of, a container object.
        /// </summary>
        GENERIC_EXECUTE = 0x20000000,

        /// <summary>
        /// The right to create or delete child objects, delete a sub-tree, read and write
        /// properties, examine child objects and the object itself, add and remove the object from
        /// the directory, and read or write with an extended right.
        /// </summary>
        GENERIC_ALL = 0x10000000,

        /// <summary>
        /// The right to create child objects of the object. The ObjectType member of an ACE can
        /// contain a GUID that identifies the type of child object whose creation is controlled. If
        /// ObjectType does not contain a GUID, the ACE controls the creation of all child object types.
        /// </summary>
        DS_CREATE_CHILD = 0x1,

        /// <summary>
        /// The right to delete child objects of the object. The ObjectType member of an ACE can
        /// contain a GUID that identifies a type of child object whose deletion is controlled. If
        /// ObjectType does not contain a GUID, the ACE controls the deletion of all child object types.
        /// </summary>
        DS_DELETE_CHILD = 0x2,

        /// <summary>
        /// The right to list child objects of this object. For more information about this right,
        /// see Controlling Object Visibility.
        /// </summary>
        ACTRL_DS_LIST = 0x4,

        /// <summary>
        /// The right to perform an operation controlled by a validated write access right. The
        /// ObjectType member of an ACE can contain a GUID that identifies the validated write. If
        /// ObjectType does not contain a GUID, the ACE controls the rights to perform all valid
        /// write operations associated with the object.
        /// </summary>
        DS_SELF = 0x8,

        /// <summary>
        /// The right to read properties of the object. The ObjectType member of an ACE can contain
        /// a GUID that identifies a property set or property. If ObjectType does not contain a
        /// GUID, the ACE controls the right to read all of the object properties.
        /// </summary>
        DS_READ_PROP = 0x10,

        /// <summary>
        /// The right to write properties of the object. The ObjectType member of an ACE can contain
        /// a GUID that identifies a property set or property. If ObjectType does not contain a
        /// GUID, the ACE controls the right to write all of the object properties.
        /// </summary>
        DS_WRITE_PROP = 0x20,

        /// <summary>
        /// The right to delete all child objects of this object, regardless of the permissions of
        /// the child objects.
        /// </summary>
        DS_DELETE_TREE = 0x40,

        /// <summary>
        /// The right to list a particular object. If the user is not granted such a right, and the
        /// user does not have ADS_RIGHT_ACTRL_DS_LIST set on the object parent, the object is
        /// hidden from the user. This right is ignored if the third character of the dSHeuristics
        /// property is '0' or not set. For more information, see Controlling Object Visibility.
        /// </summary>
        DS_LIST_OBJECT = 0x80,

        /// <summary>
        /// The right to perform an operation controlled by an extended access right. The ObjectType
        /// member of an ACE can contain a GUID that identifies the extended right. If ObjectType
        /// does not contain a GUID, the ACE controls the right to perform all extended right
        /// operations associated with the object.
        /// </summary>
        DS_CONTROL_ACCESS = 0x100
    }
}
