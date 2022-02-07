using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCState_Skeleton_Combat : INPCState
{
    //---References
    NPC_AI npc;
    Transform actor;

    //---Pathfinding
    Vector3[] path;
    int targetWaypointIndex = 0;

    //---Combat
    enum tactics
    {
        longSwings,
        shortSwings,
        pierce
    }
    tactics currentTactic;
    float tacticChangeTimer = 0;
    int swingDirection = 1;
    float ChangeDirectionTimer = 0;
    float ChangeDirectionInterval = 1;
    float timeWithThisTactic = 0;
    float TimeLimitForThisTactic = 1;
    float PierceTimer = 0;
    float PierceInterval = 2;



    public void ChangeToThisState(NPC_AI _npc)
    {
        npc = _npc;
        actor = npc.npcController.actor.transform;

        npc.npcController.actor.Input_DrawWeapon();

        if (npc.currentTarget != null)
        {
            PathRequestManager.RequestPath(actor.position, npc.currentTarget.position, OnPathFound);
        }

    }
    public INPCState DoState(NPC_AI _npc)
    {
        if (npc.currentTarget != null)
        {
            PathRequestManager.RequestPath(actor.position, npc.currentTarget.position, OnPathFound);
            FollowPath();
            npc.testpath = path;
            npc.testtargetWaypointIndex = targetWaypointIndex;


            if (tacticChangeTimer > 1f)
            {
                ChooseRandomTactic();
                tacticChangeTimer = 0;
            }

            //currentTactic = tactics.pierce;
            switch (currentTactic)
            {
                case tactics.longSwings:
                    TimeLimitForThisTactic = 5f;
                    AttackPattern_LongSwings();
                    break;

                case tactics.shortSwings:
                    TimeLimitForThisTactic = 5f;
                    AttackPattern_ShortSwings();
                    break;

                case tactics.pierce:
                    TimeLimitForThisTactic = 7f;
                    AttackPattern_PierceEnemy();
                    break;
            }
            return npc.state_skeleton_combat;
        }
        else
        {
            return npc.state_skeleton_chase;
        }
    }

    void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            //Debug.Log("OnPathFound");
            path = newPath;
        }
    }
    void FollowPath()
    {
        if (path != null && path.Length > 0 && targetWaypointIndex < path.Length)
        {
            Vector3 currentWaypoint = path[targetWaypointIndex];
            if (actor.position == currentWaypoint)
            {
                targetWaypointIndex++;
            }

            npc.testNextNode = currentWaypoint;

            npc.npcController.Input_Movement(currentWaypoint - actor.position);
            //actor.position = Vector3.MoveTowards(actor.position, currentWaypoint, 5 * Time.deltaTime);

        }
    }

    void ChooseRandomTactic()
    {
        //Debug.Log("rolling for randon tactic");
        int rand = Mathf.RoundToInt(Random.Range(1, 4));

        switch (rand)
        {
            default:
                currentTactic = tactics.longSwings;
                break;
            case 1:
                currentTactic = tactics.longSwings;
                break;
            case 2:
                currentTactic = tactics.shortSwings;
                break;
            case 3:
                currentTactic = tactics.pierce;
                break;
        }
        //Debug.Log(currentTactic);

    }


    void AttackPattern_LongSwings()
    {
        //npc.npcWeaponController.UpdateSwingDirection(swingDirection);
        ChangeDirectionInterval = 3f;

        if (swingDirection == 0)
        {
            swingDirection = 1;
        }


        if (ChangeDirectionTimer >= ChangeDirectionInterval)
        {
            swingDirection = -swingDirection;
            ChangeDirectionTimer = 0;
        }
        ChangeDirectionTimer += Time.deltaTime;

        //Debug.Log("swingDirection = " + swingDirection);
        npc.npcController.Input_WeaponSwing(swingDirection);



        if (timeWithThisTactic > TimeLimitForThisTactic)
        {
            timeWithThisTactic = 0;
            ChooseRandomTactic();
        }
        timeWithThisTactic += Time.deltaTime;
    }


    void AttackPattern_ShortSwings()
    {
        if (swingDirection == 0)
        {
            swingDirection = 1;
        }

        float maxAngle = 150f;
        Vector3 targetProjection = actor.InverseTransformPoint(npc.currentTarget.position).normalized;
        Vector3 bladeProjection = actor.InverseTransformPoint(npc.npcController.weaponObject.transform.position).normalized;

        float angleBetweenBladeAndEnemy = Vector3.SignedAngle(bladeProjection, targetProjection, -npc.transform.forward);


        if (angleBetweenBladeAndEnemy < -maxAngle)
        {
            swingDirection = -1;
        }
        if (angleBetweenBladeAndEnemy > maxAngle)
        {
            swingDirection = 1;
        }

        npc.npcController.Input_WeaponSwing(swingDirection);


        if (timeWithThisTactic > TimeLimitForThisTactic)
        {
            timeWithThisTactic = 0;
            ChooseRandomTactic();
        }
        timeWithThisTactic += Time.deltaTime;
    }


    void AttackPattern_PierceEnemy()
    {
        float thresholdAngle = 90f; // angle after which enemy changes piercing to swinging
        float maxAngle = 3f;
        float minAngle = 1f;
        Vector3 targetProjection = actor.InverseTransformPoint(npc.currentTarget.position).normalized;
        Vector3 bladeProjection = actor.InverseTransformPoint(npc.npcController.weaponObject.transform.position).normalized;

        float angleBetweenBladeAndEnemy = Vector3.SignedAngle(bladeProjection, targetProjection, -npc.transform.forward);

        if (Mathf.Abs(angleBetweenBladeAndEnemy) > thresholdAngle)
        {
            //currentTactic = tactics.longSwings; // or maybe rand
        }


        if (angleBetweenBladeAndEnemy > maxAngle)
        {
            swingDirection = 1;
        }
        if (angleBetweenBladeAndEnemy < -maxAngle)
        {
            swingDirection = -1;
        }
        if (Mathf.Abs(angleBetweenBladeAndEnemy) <= minAngle)
        {
            swingDirection = 0;
        }
        npc.npcController.Input_WeaponSwing(swingDirection);


        if (PierceTimer >= PierceInterval)
        {
            npc.npcController.Input_WeaponPierce();

            PierceTimer = 0;
        }




        //Debug.Log("angleBetweenBladeAndEnemy: " + angleBetweenBladeAndEnemy);
        //npc.npcWeaponController.UpdateSwingDirection(swingDirection);

        PierceTimer += Time.deltaTime;

        timeWithThisTactic += Time.deltaTime;
        if (timeWithThisTactic > TimeLimitForThisTactic)
        {
            timeWithThisTactic = 0;
            ChooseRandomTactic();
        }
    }




}