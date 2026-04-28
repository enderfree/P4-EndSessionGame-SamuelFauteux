public class Damage
{
    private float amount;
    private MoveTypes type;

    public Damage(float amount, MoveTypes types)
    {
        this.amount = amount;
        this.type = types;
    }

    public float Amount { get { return amount; } }
    public MoveTypes Type { get { return type; } }
}