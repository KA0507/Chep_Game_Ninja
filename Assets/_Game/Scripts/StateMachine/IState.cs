using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    // Bắt đầu state
    void OnEnter(Enemy enemy);
    // Thực hiện state
    void OnExecute(Enemy enemy);
    // Kết thúc state
    void OnExit(Enemy enemy);
}
