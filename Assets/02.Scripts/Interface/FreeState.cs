public class FreeState : IPlayerState
{
    private PlayerController playerController;

    public FreeState(PlayerController playerController)
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
