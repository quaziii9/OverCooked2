public interface IIngredientState
{
    void Processing(Ingredient ingredient, PlayerInteracteController playerIC);
    void Cooked(Ingredient ingredient, PlayerInteracteController playerIC);
    void Raw(Ingredient ingredient, PlayerInteracteController playerIC);
}
