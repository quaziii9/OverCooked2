public class FreeState : IPlayerState
{
    private PlayerInteracteController playerController;

    public FreeState(PlayerInteracteController playerController)
    {
        this.playerController = playerController;
    }

    public void CatchOrKnockback()
    {
        playerController.CatchOrKnockback();
    }

    public void CookOrThrow()
    {
        playerController.CookOrThrow();
    }

    public void PickupOrPlace()
    {
        playerController.PickupOrPlace();
    }
}
