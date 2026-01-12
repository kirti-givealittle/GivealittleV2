namespace GivealittleV2.Domain.Models.Cause
{
    public class CausePageDetails
    {
        public required string CauseTitle { get; set; }
        public required string CauseStory { get; set; }
        public required string CausePageDescription { get; set; }
        public required string FundsUtilizationDescription { get; set; }
        public required string PageOwnerBeneficiaryRelationship { get; set; }
        public string? FacebookLink { get; set; }
        public string? XLink { get; set; }
        public string? InstagramLink { get; set; }
        public string? YoutubeLink { get; set; }
        public string? WebLink { get; set; }
    }
}