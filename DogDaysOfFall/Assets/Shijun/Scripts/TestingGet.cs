using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ssss
{

    class Something
    {
        //private string name = "N.A";


        private string[] name = new string[3] { "aaaa", "2213213", "ff" };
        
        private byte age = 0;
        public string[] Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public byte Age
        {
            get
            {
                return age;
            }
            set
            {
                age = value;
            }
        }

        public override string ToString()
        {
            return "Name = " + Name[1] + "Age : " + Age;
        }
    }


    public class TestingGet : MonoBehaviour
    {
        Something something = new Something();
        // Start is called before the first frame update
        void Start()
        {
            something.Name[0] = "1";
            something.Name[1] = "2asfada";
            something.Age = 21;

            Debug.Log(something);

            something.Age += 1;
            Debug.Log(something);

        }

        // Update is called once per frame
        void Update()
        {
        
        }





        private float[] score;

        public float[] Score
        {
            get
            {
                return score;
            }
        }
    }



}
