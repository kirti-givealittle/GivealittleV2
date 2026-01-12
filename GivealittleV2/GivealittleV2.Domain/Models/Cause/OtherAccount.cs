namespace GivealittleV2.Domain.Models.Cause
{
    public class OtherAccount
    {
        public required string PayeeFirstName { get; set; }
        public required string PayeeLastName { get; set; }
        public required string PayeeEmail { get; set; }
        public required string PayeeContactNumber { get; set; }
        public required bool FundsForPayee { get; set; }
        public SomeoneElseAccount? SomeoneElseAccount { get; set; }

    }
}