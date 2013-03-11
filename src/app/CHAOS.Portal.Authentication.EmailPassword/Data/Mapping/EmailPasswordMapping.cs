namespace CHAOS.Portal.Authentication.EmailPassword.Data.Mapping
{
    using System.Collections.Generic;
    using System.Data;

    using CHAOS.Data;
    using CHAOS.Portal.Authentication.EmailPassword.Data.Dto;

    public class EmailPasswordMapping : IReaderMapping<EmailPassword>
    {
        public IEnumerable<EmailPassword> Map(IDataReader reader)
        {
            while(reader.Read())
            {
                yield return new EmailPassword
                    {
                        
                    };
            }
        }
    }
}
