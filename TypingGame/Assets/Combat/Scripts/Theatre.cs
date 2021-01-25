using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Theatre : MonoBehaviour
{
    GameObject player;
    BattleSystem system;
    List<GameObject> enemies = new List<GameObject>();
    List<PositionMover> _moves = new List<PositionMover>();
    public float step_freshhold = 0.1f;
    public float forwardStep, step_speed =10, wait_time_move,wait_time_attack, if_empty_wait=1f;
    


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInChildren<PlayerStats>().gameObject;
        system = FindObjectOfType<BattleSystem>();
    }

    public void SetEnemies(List<EnemyStats> _enemies)
    {
        foreach(EnemyStats enemy in _enemies)
        {
            enemies.Add(enemy.gameObject);
        }
    }

    public void AddEnemy(EnemyStats _enemy)
    {
        enemies.Add(_enemy.gameObject);
    }

    public void InsertEnemy(int i,EnemyStats _enemy)
    {
        enemies.Insert(i,_enemy.gameObject);
    }

    private void Update()
    {
        if (_moves.Count == 0) return;
        foreach (PositionMover _move in _moves)
        {
            if (_move.walking)
            _move._character.transform.position = Vector2.MoveTowards(_move._character.transform.position, _move.end_position, _move.speed*Time.deltaTime);
            if(_move._character.GetComponent<CharacterAnimation>().dead)
            {
                _moves.Remove(_move);
            }
            //if (Vector2.Distance(_move._caracter.transform.position, _move.end_position)< step_freshhold)
            //{
            //    _moves.Remove(_move);
            //}
        }
        foreach (PositionMover _move in _moves)
        {
            
            if (Vector2.Distance(_move._character.transform.position, _move.end_position) < step_freshhold && _move.walking)
            {
                var _character = _move._character;
                _move.walking = false;
                _character.GetComponent<CharacterAnimation>().Idle();
                if (_character.GetComponent<PlayerStats>())
                    _character.GetComponent<CharacterAnimation>().Turn(false);
                else
                    _character.GetComponent<CharacterAnimation>().Turn(true);
                StartCoroutine(WaitTimeMove(_move));
                break;
            }
            
        }
    }


    IEnumerator WaitTimeMove(PositionMover _move)
    {
        yield return new WaitForSeconds(_move.wait_time);
        system.Next_turn();
        _moves.Remove(_move);
    }

    IEnumerator WaitTimeAttack(GameObject _item)
    {
        yield return new WaitForSeconds(wait_time_attack);
        system.Next_turn();
        if (_item != null)
        Destroy(_item);
    }

    IEnumerator If_Empty_Wait()
    {
        yield return new WaitForSeconds(if_empty_wait);
        system.Next_turn();
    }

    public void Step(GameObject _character, bool forward, bool player)
    {
        if ((player&& forward) || (!player && !forward))
        {
            MoveCharacter(_character, new Vector2(_character.transform.position.x + forwardStep, _character.transform.position.y), step_speed, wait_time_move);
        }
        else
        {
            MoveCharacter(_character, new Vector2(_character.transform.position.x - forwardStep, _character.transform.position.y), step_speed, wait_time_move);
        }
    }

    public void MoveCharacter(GameObject _character, Vector2 end_position, float speed, float _waitTime)
    {
        PositionMover new_move = new PositionMover();
        new_move._character = _character;
        new_move.end_position = end_position;
        new_move.speed = speed;
        new_move.wait_time = _waitTime;
        _character.GetComponent<CharacterAnimation>().Move();
        _moves.Add(new_move);

        if (_character.transform.position.x> end_position.x )
        {
            _character.GetComponent<CharacterAnimation>().Turn(true);
        }
        else
        {
            _character.GetComponent<CharacterAnimation>().Turn(false);
        }
    }

    public void Ability_Animation(GameObject _character, Ability ability)
    {
        if (ability._sprite_object == null)
        {
            if(ability.Impact_object == null)
            {
                FindObjectOfType<BattleSystem>().Initiate_Abilities();
            }
            StartCoroutine(If_Empty_Wait());
            return;
        }
        GameObject item = Instantiate(ability._sprite_object, _character.transform.position, _character.transform.rotation) as GameObject;

        item.transform.parent = _character.transform;
        StartCoroutine(WaitTimeAttack(item));
    }

    public void KO_object(GameObject _character)
    {
        _character.GetComponent<CharacterAnimation>().KO();
    }

    

    public class PositionMover
    {
        public GameObject _character;
        public Vector2 end_position;
        public float speed, wait_time;
        public bool walking = true;
    }

    
}
