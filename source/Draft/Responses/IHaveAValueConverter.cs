using System;

namespace Draft.Responses
{
    internal interface IHaveAValueConverter
    {

        Func<IKeyDataValueConverter> ValueConverter { get; set; }

    }
}
