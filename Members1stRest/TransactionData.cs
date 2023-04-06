namespace Members1stRest
{
    [PrimaryKey("AccountNumber", new string[] {"Id", "SequenceNumber"})]
    public class TransactionData
    {
        public string? AccountNumber { get; set; }
        public string? Id { get; set; }
        public int? IdType { get; set; }
        public int? CommentCode { get; set; }
        public int? TransferCode { get; set; }
        public int? AdjustmentCode { get; set; }
        public int? RegECode { get; set; }
        public int? RegDCheckCode { get; set; }
        public int? RegDTransferCode { get; set; }
        public int? VoidCode { get; set; }
        public string? SubActionCode { get; set; }
        public int? SequenceNumber { get; set; }
        public string? EffectiveDate { get; set; }
        public string? PostDate { get; set; }
        public int? PostTime { get; set; }
        public decimal? PreviousAvailableBalance { get; set; }
        public int? UserNumber { get; set; }
        public int? UserOverride { get; set; }
        public int? SecurityLevels { get; set; }
        public string? Description { get; set; }
        public string? ActionCode { get; set; }
        public string? SourceCode { get; set; }
        public decimal? BalanceChange { get; set; }
        public decimal? InterestPenalty { get; set; }
        public decimal? NewBalance { get; set; }
        public decimal? FeeAmount { get; set; }
        public decimal? EscrowAmount { get; set; }
        public string? LastTranDate { get; set; }
        public string? MaturityLoanDueDate { get; set; }
        public string? Comment { get; set; }
        public int? Branch { get; set; }
        public int? ConsoleNumber { get; set; }
        public int? BatchSequence { get; set; }
        public decimal? SalesTaxAmount { get; set; }
        public string? ActivityDate { get; set; }
        public decimal? BilledFeeAmount { get; set; }
        public int? ProcessorUser { get; set; }
        public string? MemberBranch { get; set; }
        public int? SubSourceCode { get; set; }
        public int? ConfirmationSequence { get; set; }
        public string? MicrAccountNumber { get; set; }
        public string? MicrRtNumber { get; set; }
        public int? Recurring { get; set; }
        public decimal? FeeExemptCourtesyAmount { get; set; }
        public string? BalSeg001SegmentId { get; set; }
        public string? BalSeg001PmtChangeDate { get; set; }
        public string? InterestEffectiveDate { get; set; }
        public string? BalSeg001PrevFirstPmtDate { get; set; }
        public string? DraftNumber { get; set; }
        public string? TracerNumber { get; set; }
        public string? SubSourceDescription { get; set; }
        public decimal? TransactionAmount { get; set; }
        public string? ConfirmationNumber { get; set; }
    }
}
