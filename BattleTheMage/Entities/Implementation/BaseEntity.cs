using BattleTheMage.Damage;
using BattleTheMage.Inventory;
using BattleTheMage.Inventory.Implementation;

namespace BattleTheMage.Entities.Implementation;

public abstract class BaseEntity : IAttackingEntity, IInventoryEntity<BaseItemStack>
{
    private readonly string _name;
    public string Name() => _name;

    private double _health;
    public double Health() => _health;
    
    private double _maxHealth;

    public double MaxHealth() => _maxHealth;

    private readonly BaseInventory _baseInventory = new();
    public IInventory<BaseItemStack> Inventory() => _baseInventory;

    public int ArmorClass()
    {
        // 10 is base
        int armorClass = 10;

        foreach (BaseItemStack itemStack in Inventory().ItemStacks())
        {
            if (itemStack.Items()[0] is IArmorItem armorItem)
            {
                armorClass = armorItem.ProcessArmorClass(armorClass + armorItem.ArmorClassModifier());
            }
        }

        return armorClass;
    }

    public void DoHealthDelta(double health)
    {
        _health += health;
    }

    public void DoMaxHealthDelta(double health)
    {
        _maxHealth += health;
    }

    // Search the inventory for weapons
    public List<IAttack> Attacks()
    {
        List<IAttack> attacks = new();

        foreach (BaseItemStack itemStack in Inventory().ItemStacks())
        {
            if (itemStack.Items()[0] is IWeaponItem weapon)
            {
                attacks.Add(weapon.DefaultAttack());
            }
        }

        return attacks;
    }

    public virtual int AttacksPerTurn() => 0;

    protected BaseEntity(string name, double maxHealth)
    {
        _name = name;
        _maxHealth = _health = maxHealth;
    }

    public int InitiativeModifier(int raw) => raw;

    public override string ToString() => Name();

    public virtual Dictionary<IDamageType, int> Weaknesses() => new();

    public virtual Dictionary<IDamageType, int> Resistances() => new();

    public virtual List<ILingeringEffect> LingeringEffects() => new();

    public virtual bool OnDeath(IAttack attack, IAttackingEntity attackingEntity) => true;

    public bool ApplyAttack(IAttack attack, IAttackingEntity attackingEntity)
    {
        double baseDamage = attack.Weapon().BaseDamage();
        IDamageType damageType = attack.Weapon().DamageType();
        
        foreach (IWeaponEffect weaponEffect in attack.WeaponEffects() ?? new())
        {
            baseDamage *= weaponEffect.Multiplier();
            baseDamage -= weaponEffect.Additive();
            damageType = weaponEffect.OverrideDamageType() ?? damageType;
        }

        double? weaknessMultiplier = null;
        if(Weaknesses().ContainsKey(damageType)) weaknessMultiplier =
            damageType.PositiveModifierTable()[Weaknesses()[damageType]];
        double? resistanceMultiplier = null;
        if(Resistances().ContainsKey(damageType)) resistanceMultiplier =
            damageType.NegativeModifierTable()[Resistances()[damageType]];

        double finalMultiplier = weaknessMultiplier ?? resistanceMultiplier ?? 1.0;

        baseDamage *= finalMultiplier;
        
        DoHealthDelta(-baseDamage);
        damageType.OnDamage(this, attackingEntity);
        if (Health() <= 0)
        {
            // Check if we're allowed to die
            return OnDeath(attack, attackingEntity);
        }

        return false;
    }
}