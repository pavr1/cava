using System.Globalization;

namespace cava.Custom.Cultures
{
    public class CostaRicanCulture : CultureInfo
    {
        private readonly Calendar cal;
        private readonly Calendar[] optionals;

        public CostaRicanCulture()
            : this("es-CR", true)
        {
            this.DateTimeFormat = new CultureInfo("es-CR").DateTimeFormat;   
        }

        public CostaRicanCulture(string cultureName, bool useUserOverride) : base(cultureName, useUserOverride)
        {
            //Your Custom Currency Numbers Calendar Culture Code
        }

        public override Calendar Calendar
        {
            get { return cal; }
        }

        public override Calendar[] OptionalCalendars
        {
            get { return optionals; }
        }
    }
}