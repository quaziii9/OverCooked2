public interface IIngredientState
{
    void Processing(Ingredient ingredient, Player player);
    void Cooked(Ingredient ingredient, Player player);
    void Raw(Ingredient ingredient, Player player);
}
