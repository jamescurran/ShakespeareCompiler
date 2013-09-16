#region Copyright & License summary
/*
 Copyright 2013, James M. Curran, Novel Theory Software

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 */
#endregion

using Shakespeare.Support.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shakespeare.Support
{
    public class Dramaturge
    {
        protected List<Character> Cast { get; private set; }

        protected List<Character> OnStage { get; private set; }

        protected Character FirstPerson { get; set; }

        protected Character SecondPerson { get; set; }

        protected int Comp1 { get; set; }

        protected int Comp2 { get; set; }

        protected bool TruthFlag { get; set; }

        protected int NumberOnStage
        {
            get
            {
                return OnStage.Count;
            }
        }

        protected int NumberInCast
        {
            get
            {
                return Cast.Count;
            }
        }

        protected TextReader In { get; private set; }

        protected TextWriter Out { get; private set; }

        public Dramaturge(TextReader tr, TextWriter tw)
        {
            Cast = new List<Character>();
            OnStage = new List<Character>();
            In = tr;
            Out = tw;
        }
          
        protected void Activate(int lineno, Character character)
        {
            if (!character.OnStage)
                throw new RuntimeException(lineno, "{0} is not on stage, and thus cannot speak!", character.Name);

            /* If there are exactly two people on stage, the other one should be
            second person */
            if (NumberOnStage == 2)
            {
                SecondPerson = OnStage.Single(ch => !ch.Equals(character));
            }
            else
            {
                SecondPerson = null;
            }

            FirstPerson = character;
        }
   
        protected void Assign(int lineno, int value)
        {
            CheckNullCharacter(lineno);
            SecondPerson.Value = value;
        }

        protected void CharInput(int lineno)
        {
            CheckNullCharacter(lineno);
            SecondPerson.Value = In.Read();
        }

        protected void CharOutput(int lineno)
        {
            CheckNullCharacter(lineno);
            if (!SecondPerson.ValueIsCharacter)
                throw new RuntimeException(lineno, "The value {0} does not correspond to a character.", SecondPerson.Value);

            Out.Write((char)SecondPerson.Value);
        }

        protected void EnterScene(int lineno, Character character)
        {
            if (character.OnStage)
                throw new RuntimeException(lineno, "{0} is already on stage, and thus cannot enter!", character.Name);

            character.OnStage = true;
            OnStage.Add(character);
        }

        protected void ExitScene(int lineno, Character character)
        {
            if (!character.OnStage)
                throw new RuntimeException(lineno, "{0}  is not on stage, and thus cannot exit!", character.Name);

            OnStage.Remove(character);
            character.OnStage = false;
        }

        protected void ExitSceneAll(int lineno)
        {
            foreach (var ch in OnStage)
                ch.OnStage = false;
            OnStage.Clear();
        }

        protected Character InitializeCharacter(int lineno, string characterName)
        {
            var chr = new Character(characterName);
            Cast.Add(chr);
            return chr;
        }

        static readonly int[] factorials = new int[] { 0, 1, 2, 6, 24, 120, 720, 5040, 40320, 362880, 3628800, 39916800, 479001600 };

        protected int Factorial(int lineno, int n)
        {
            if (n < 0 || n > 12)
                throw new RuntimeException(lineno,"Unable to compute factorial of {0}.", n);
            return factorials[n];
        }

        protected int Sqrt(int lineno, int value)
        {
            if (value < 0)
                throw new RuntimeException(lineno, "Unable to compute the square root of {0}, since it is negative.", value);
            return (int)Math.Sqrt((double)value);
        }

        protected int Square(int lineno, int value)
        {
            return value * value;
        }

        protected int Twice(int lineno, int value)
        {
            return 2 * value;
        }

        protected int Cube(int lineno, int value)
        {
            return value * value * value;
        }

        protected void IntInput(int lineno)
        {
            CheckNullCharacter(lineno);
            var buf = In.ReadLine();
            long lng;
            if (Int64.TryParse(buf, out lng))
            {
                if (Int32.MinValue > lng || lng > Int32.MaxValue)
                    throw new RuntimeException(lineno, "{0} 's heart whispered an integer that was out of range.", SecondPerson.Name);
                SecondPerson.Value = (int)lng;
            }
            else
                throw new RuntimeException(lineno, "{0}'s heart whispered something that was not a valid integer.", SecondPerson.Name);
        }

        protected void IntOutput(int lineno)
        {
            CheckNullCharacter(lineno);

            Out.Write("{0}", SecondPerson.Value);
        }

        protected void Pop(int lineno)
        {
            CheckNullCharacter(lineno);
            SecondPerson.Pop(lineno);
        }

        protected void Push(int lineno, int value)
        {
            CheckNullCharacter(lineno);
            SecondPerson.Push(lineno, value);
        }

        protected int ValueOf(int lineno, Character character)
        {
            return character.Value;
        }

        private void CheckNullCharacter(int lineno)
        {
            if (SecondPerson == null)
            {
                if (NumberOnStage == 1)
                    throw new RuntimeException(lineno, "Erroneous use of second person pronoun. There is only one character on stage!");
                else
                    throw new RuntimeException(lineno, "Ambiguous use of second person pronoun. There are more than two characters on stage!");
            }
        }
    }
}