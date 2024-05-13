public class HoldState : IPlayerState
{
    private PlayerInteracteController playerController;

    public HoldState(PlayerInteracteController playerController)
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
