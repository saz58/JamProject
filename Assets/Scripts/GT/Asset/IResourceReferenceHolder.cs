using System;

namespace GT.Asset
{
    public interface IResourceReferenceHolder
    {
        Action DestroyResource { get; set; }
    }
}