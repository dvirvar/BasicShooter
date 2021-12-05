using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NavigationController: ViewController
{
    public event Action willDismiss = delegate { };
    public ViewController lastController => controllers.Peek();
    public int count => controllers.Count;
    [SerializeField]
    private ViewController baseController;
    private Stack<ViewController> controllers;

    private void Awake()
    {
        controllers = new Stack<ViewController>();
        push(baseController);
    }

    public bool contains(ViewController controller)
    {
        return controllers.Contains(controller);
    }

    public void popAll()
    {
        for(int i = 0; i<count; i++)
        {
            pop();
        }
    }

    public void push(ViewController controller)
    {
        //Not the base controller
        if (controllers.Count > 0)
        {
            var lastController = this.lastController;
            lastController.SetActive(false);
        }
        controller.navigationController = this;
        controllers.Push(controller);
        controller.SetActive(true);
    }

    public void pop()
    {
        if (controllers.Count > 1) {
            ViewController lastController = controllers.Pop();
            lastController.SetActive(false);
            lastController.navigationController = null;//We do this because we only set active to false and not destroying the Prefab
            ViewController newController = controllers.Peek();
            newController.SetActive(true);
        } else
        {
            willDismiss();
            if (navigationController != null)
            {
                navigationController.pop();
            }
        }
        
    }

    public void pop(int times)
    {
        if (times < 0 || times > count)
        {
            throw new ArgumentException("Times must be greater than -1 and equal or less than count");
        }
        for (int i = 0; i < times; i++)
        {
            pop();
        }
    }

    public void popTo(ViewController controller)
    {
        int indexOfController = -1;
        int counter = -1;
        foreach(ViewController gameObject in controllers)
        {
            counter++;
            if (gameObject == controller)
            {
                indexOfController = counter;
                break;
            }
        }
        if(indexOfController == -1)
        {
            throw new UnityException("controller doesn't exist in navigation controller");
        } else
        {
            int timesOfPopping = controllers.Count - indexOfController + 1;
            pop(timesOfPopping);
        }
    }
}
