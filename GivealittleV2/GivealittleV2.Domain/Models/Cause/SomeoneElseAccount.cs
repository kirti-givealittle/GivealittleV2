namespace GivealittleV2.Domain.Models.Cause
{
    public class SomeoneElseAccount
    {
        public bool ConsentGiven { get; set; }
        public required string OtherPartyIdentity { get; set; }
        public required string OtherPartyRelationship { get; set; }
    }
}