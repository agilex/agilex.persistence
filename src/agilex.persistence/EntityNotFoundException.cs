using System;

namespace agilex.persistence
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string m) : base(m)
        {
            
        }
    }
}