namespace acme.Models;

public class RedeemResponse {
  public DateTime Date {get; set;}
  public int Code {get; set;}
  public string? Message {get;set;}
  public string? ProdId {get;set;}
}
