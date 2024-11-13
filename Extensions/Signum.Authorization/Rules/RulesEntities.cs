using System.Data;

namespace Signum.Authorization.Rules;

[EntityKind(EntityKind.System, EntityData.Master)]
public abstract class RuleEntity<R> : Entity
    where R : class
{
    public Lite<RoleEntity> Role { get; set; }

    public R Resource { get; set; }

    protected override void PreSaving(PreSavingContext ctx)
    {
        this.toStr = this.ToString();
    }

    protected override string? PropertyValidation(PropertyInfo pi)
    {
        if (pi.Name == nameof(Role) && RoleEntity.RetrieveFromCache(Role).IsTrivialMerge)
            return AuthAdminMessage.Role0IsTrivialMerge.NiceToString(Role);

        return base.PropertyValidation(pi);
    }
}

public class RuleQueryEntity : RuleEntity<QueryEntity> 
{
    public QueryAllowed Allowed { get; set; }

    [AutoExpressionField]
    public override string ToString() => As.Expression(() => $"{Resource} for {Role} <- {Allowed}");
}

public class RulePermissionEntity : RuleEntity<PermissionSymbol> 
{
    public bool Allowed { get; set; }

    [AutoExpressionField]
    public override string ToString() => As.Expression(() => $"{Resource} for {Role} <- {Allowed}");
}

public class RuleOperationEntity : RuleEntity<OperationTypeEmbedded> 
{
    public OperationAllowed Allowed { get; set; }

    [AutoExpressionField]
    public override string ToString() => As.Expression(() => $"{Resource} for {Role} <- {Allowed}");
}


public class OperationTypeEmbedded : EmbeddedEntity
{
    public OperationSymbol Operation { get; set; }

    public TypeEntity Type { get; set; }

    [AutoExpressionField]
    public override string ToString() => As.Expression(() => $"{Operation}/{Type}");
}

public class RulePropertyEntity : RuleEntity<PropertyRouteEntity> 
{
    public PropertyAllowed Fallback { get; set; }

    [PreserveOrder, Ignore, QueryableProperty]
    [BindParent]
    public MList<RulePropertyConditionEntity> ConditionRules { get; set; } = new MList<RulePropertyConditionEntity>();

    protected override string? PropertyValidation(PropertyInfo pi)
    {
        if (pi.Name == nameof(ConditionRules))
        {
            var errors = NoRepeatValidatorAttribute.ByKey(ConditionRules, a => a.Conditions.OrderBy(a => a.ToString()).ToString(" & "));

            if (errors != null)
                return ValidationMessage._0HasSomeRepeatedElements1.NiceToString(this.Resource, errors);
        }

        return base.PropertyValidation(pi);
    }
}

[EntityKind(EntityKind.System, EntityData.Master)]
public class RulePropertyConditionEntity : Entity, IEquatable<RulePropertyConditionEntity>, ICanBeOrdered
{
    [NotNullValidator(Disabled = true)]
    public Lite<RulePropertyEntity> RuleProperty { get; set; }

    [PreserveOrder, NoRepeatValidator, CountIsValidator(ComparisonType.GreaterThan, 0)]
    public MList<TypeConditionSymbol> Conditions { get; set; } = new MList<TypeConditionSymbol>();

    public PropertyAllowed Allowed { get; set; }

    public int Order { get; set; }

    public override int GetHashCode() => Conditions.Count ^ Allowed.GetHashCode();

    public override bool Equals(object? obj) => obj is RuleTypeConditionEntity rtc && Equals(rtc);
    public bool Equals(RulePropertyConditionEntity? other)
    {
        if (other == null)
            return false;

        return this.Conditions.ToHashSet().SetEquals(other.Conditions) && this.Allowed == other.Allowed;
    }

    [AutoExpressionField]
    public override string ToString() => As.Expression(() => Allowed.ToString());

}


public class RuleTypeEntity : RuleEntity<TypeEntity>
{
    public TypeAllowed Fallback { get; set; }

    [PreserveOrder, Ignore, QueryableProperty]
    [BindParent]
    public MList<RuleTypeConditionEntity> ConditionRules { get; set; } = new MList<RuleTypeConditionEntity>();

    protected override string? PropertyValidation(PropertyInfo pi)
    {
        if (pi.Name == nameof(ConditionRules))
        {
            var errors = NoRepeatValidatorAttribute.ByKey(ConditionRules, a => a.Conditions.OrderBy(a => a.ToString()).ToString(" & "));

            if (errors != null)
                return ValidationMessage._0HasSomeRepeatedElements1.NiceToString(this.Resource, errors);
        }

        return base.PropertyValidation(pi);
    }
}

[EntityKind(EntityKind.System, EntityData.Master)]
public class RuleTypeConditionEntity : Entity, IEquatable<RuleTypeConditionEntity>, ICanBeOrdered
{
    [NotNullValidator(Disabled = true)]
    public Lite<RuleTypeEntity> RuleType { get; set; }

    [PreserveOrder, NoRepeatValidator, CountIsValidator(ComparisonType.GreaterThan, 0)]
    public MList<TypeConditionSymbol> Conditions { get; set; } = new MList<TypeConditionSymbol>();

    public TypeAllowed Allowed { get; set; }

    public int Order { get; set; }

    public override int GetHashCode() => Conditions.Count ^ Allowed.GetHashCode();

    public override bool Equals(object? obj) => obj is RuleTypeConditionEntity rtc && Equals(rtc);
    public bool Equals(RuleTypeConditionEntity? other)
    {
        if (other == null)
            return false;

        return this.Conditions.ToHashSet().SetEquals(other.Conditions) && this.Allowed == other.Allowed;
    }

    [AutoExpressionField]
    public override string ToString() => As.Expression(() => Allowed.ToString());

}

[DescriptionOptions(DescriptionOptions.Members), InTypeScript(true)]
public enum QueryAllowed
{
    None = 0,
    EmbeddedOnly = 1,
    Allow = 2,
}

[DescriptionOptions(DescriptionOptions.Members), InTypeScript(true)]
public enum OperationAllowed
{
    None = 0,
    DBOnly = 1,
    Allow = 2,
}

[DescriptionOptions(DescriptionOptions.Members), InTypeScript(true)]
public enum PropertyAllowed
{
    None,
    Read,
    Write,
}

[DescriptionOptions(DescriptionOptions.Members)]
public enum TypeAllowed
{
    None = TypeAllowedBasic.None << 2 | TypeAllowedBasic.None,

    DBReadUINone = TypeAllowedBasic.Read << 2 | TypeAllowedBasic.None,
    Read = TypeAllowedBasic.Read << 2 | TypeAllowedBasic.Read,

    DBWriteUINone = TypeAllowedBasic.Write << 2 | TypeAllowedBasic.None,
    DBWriteUIRead = TypeAllowedBasic.Write << 2 | TypeAllowedBasic.Read,
    Write = TypeAllowedBasic.Write << 2 | TypeAllowedBasic.Write
}

public static class TypeAllowedExtensions
{
    public static TypeAllowedBasic GetDB(this TypeAllowed allowed)
    {
        return (TypeAllowedBasic)(((int)allowed >> 2) & 0x03);
    }

    public static TypeAllowedBasic GetUI(this TypeAllowed allowed)
    {
        return (TypeAllowedBasic)((int)allowed & 0x03);
    }

    public static TypeAllowedBasic Get(this TypeAllowed allowed, bool userInterface)
    {
        return userInterface ? allowed.GetUI() : allowed.GetDB();
    }

    public static TypeAllowed Create(TypeAllowedBasic database, TypeAllowedBasic ui)
    {
        TypeAllowed result = (TypeAllowed)(((int)database << 2) | (int)ui);

        if (!Enum.IsDefined(typeof(TypeAllowed), result))
            throw new FormatException("Invalid TypeAllowed");

        return result;
    }

    public static bool IsActive(this TypeAllowed allowed, TypeAllowedBasic basicAllowed)
    {
        return allowed.GetDB() == basicAllowed || allowed.GetUI() == basicAllowed;
    }

    public static string ToStringParts(this TypeAllowed allowed)
    {
        TypeAllowedBasic db = allowed.GetDB();
        TypeAllowedBasic ui = allowed.GetUI();

        if (db == ui)
            return db.ToString();

        return "{0},{1}".FormatWith(db, ui);
    }

    public static PropertyAllowed ToPropertyAllowed(this TypeAllowedBasic ta)
    {
        PropertyAllowed pa =
            ta == TypeAllowedBasic.None ? PropertyAllowed.None :
            ta == TypeAllowedBasic.Read ? PropertyAllowed.Read : PropertyAllowed.Write;
        return pa;
    }

    public static WithConditions<PropertyAllowed> ToPropertyAllowed(this WithConditions<TypeAllowed> taac)
    {
        return new WithConditions<PropertyAllowed>(taac.Fallback.GetUI().ToPropertyAllowed(),
            taac.ConditionRules.Select(cr => new ConditionRule<PropertyAllowed>(cr.TypeConditions, cr.Allowed.GetUI().ToPropertyAllowed())));
    }
}

[InTypeScript(true)]
[DescriptionOptions(DescriptionOptions.Members)]
public enum TypeAllowedBasic
{
    None = 0,
    Read = 1,
    Write = 2,
}
