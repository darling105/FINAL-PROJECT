using System.Collections;
using System.Collections.Generic;
using System;

public class BossStateFactory
{
    private BossStateMachine _context;
    public BossStateFactory(BossStateMachine currentContext){
        _context = currentContext;
    }
    private BossIdleState _idleState;
    private BossWalk _walkState;
    private BossNormalAttackState _normalAttack;
    private BossRushIn _rushInAttack;
    private BossSpecialAttack _specialAttack;
    private BossRun _runState;
    public BossBaseState Idle(){
        return _idleState ??= new BossIdleState(_context,this);
    }
    public BossBaseState Walk(){
        return _walkState ??= new BossWalk(_context,this);
    }
    public BossBaseState NormalAttack(){
        return _normalAttack ??= new BossNormalAttackState(_context,this);
    }
    public BossBaseState RushInAttack(){
        return _rushInAttack ??= new BossRushIn(_context,this);
    }
    public BossBaseState SpecialAttack(){
        return _specialAttack ??= new BossSpecialAttack(_context,this);
    }
    public BossBaseState Run(){
        return _runState ??= new BossRun(_context,this);
    }


    public BossBaseState GetRandomOutcome((BossBaseState outcome, double probability)[] outcomes,out int indexOfState)
    {
        Random rand = new Random();
        double randomValue = rand.NextDouble(); // Generates a number between 0.0 and 1.0
        double cumulativeProbability = 0.0;

        List<double> newRange = new List<double>();
        foreach (var (outcome, probability) in outcomes){
            cumulativeProbability += probability;
            newRange.Add(cumulativeProbability);
        }

        for(int i = 0;i< newRange.Count;i++){
            if(randomValue <= newRange[i]){
                indexOfState = i;
                return outcomes[i].outcome;

            }
        }
        indexOfState = outcomes.Length - 1;
        return outcomes[outcomes.Length - 1].outcome; // Fallback (should never happen)
    }
}
