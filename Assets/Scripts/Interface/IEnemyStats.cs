public interface IEnemyStats
{
    void UpdateHP(float amount);
    float ReduceDmgByDef();
    float Res();
    void DebuffDecreaseDef(float amount);
    bool HasDebuff();
    void hasApplyDebuff(bool state);
}