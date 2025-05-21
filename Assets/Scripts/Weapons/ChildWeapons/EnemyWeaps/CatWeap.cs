using System.Collections;
using UnityEngine;

public class CatWeap : Weapon
{
    public override IEnumerator SpecialDuration(int i)
    {

        if (i < 2)
        {
            yield return StartCoroutine(PS.Dashing(PS.transform.forward));
        }
        else if(i==2)
        {
            yield return new WaitForSeconds(1f);
            if (Random.Range(0f, 2f) < 1f)
            {
                yield return StartCoroutine(PS.Dashing((PS.transform.right+PS.transform.forward).normalized));
            }
            else
            {
                yield return StartCoroutine(PS.Dashing((-PS.transform.right+PS.transform.forward).normalized));
            }
        }

        yield return base.SpecialDuration(i);
    }
}
