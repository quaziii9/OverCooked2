public class HoldState : IPlayerState
{
    private PlayerController playerController;

    public HoldState(PlayerController playerController)
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
