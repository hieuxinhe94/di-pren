namespace Pren.Web.Business.Address
{
    public interface IMaskedAddressService
    {
        MaskedAddressResult MaskInfo(MaskedAddressResult addressResult);
        MaskedAddressResult DecryptAddressResult(string encryptedAddressResult);
    }
}